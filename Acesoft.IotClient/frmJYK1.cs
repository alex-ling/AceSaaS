using Acesoft.IotNet.Iot;
using Acesoft.Util;
using SuperSocket.ClientEngine;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace Acesoft.IotClient
{
    public partial class frmJYK1 : Form
    {
        private delegate void OutputAction(string msg);
        private delegate void SetBoxText(TextBox box, string text);
        private Dictionary<string, string> sessions = new Dictionary<string, string>();
        private EasyClient client;
        private IotReceiveFilter filter;

        public frmJYK1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lblInfo.Text = "包头：封包头部内容(16进制文本)\n\n" 
                + "加/解密Key：各租户请上AceSaaS平台查阅\n\n"
                + "日志中{}内为包内容长度及命令内容长度\n\n\n\n"
                + "参数类型说明：\n\n\n"
                + "1.整形：直接转16进制\n\n"
                + "2.数字：整数部分+小数部分(最后1字节)\n\n"
                + "3.字节位：低位开始0~7分别可表示7个布尔值\n\n";
        }

        private async void btnConn_Click(object sender, EventArgs e)
        {
            this.btnConn.Enabled = false;
            this.Cursor = Cursors.WaitCursor;

            filter = new IotReceiveFilter(header.Text.Trim(), Convert.ToInt32(key.Text.Trim(), 16));

            client = new EasyClient();
            client.Closed += (s, arg) => Output("Client-Conn: disonnected from server.");
            client.Error += (s, arg) => Output($"Error: {arg.Exception.GetMessage()}");

            client.Initialize(filter, req =>
            {
                Output($"Rece-加密数据: {req.HeaderHex}-{req.Length.ToString("{00}")} {req.EncryptedBody.ToHex()}");
                Output($"Rece-解密数据: {req.HeaderHex}-{req.Length.ToString("{00}")} {req.BodyHex}");
                Output($"Rece-拆包解验: {req.HeaderHex}-{req.Length.ToString("{00}")} {req.Device.Mac}-{req.SessionId} {req.Command} {req.Crc16}");

                if (req.Command.IsResponse)
                {
                    var cmd = req.Command.Name;
                    if (cmd == "80F1")
                    {
                        if (req.Command.DataHex.StartsWith("00"))
                        {
                            sessions[req.Device.Mac] = req.SessionId;
                            Output($"Client-Login: Success with SessionId-{req.SessionId}");
                        }
                        else
                        {
                            Output($"Client-Login: Fail with response Code-{req.Command.DataHex}");
                        }
                    }
                }
                else
                {
                    var body = "";
                    switch (req.Command.Name)
                    {
                        case "00A1":
                            SetBox(txtOn, NaryHelper.HexToInt(req.Command.DataHex).ToString());
                            break;
                    }
                    var res = req.CreateResponse("00" + body);
                    Send(res);
                    Send();
                }
            });

            var ip = IPAddress.Parse(txtIP.Text.Trim().Split(':')[0]);
            var port = int.Parse(txtIP.Text.Trim().Split(':')[1]);
            if (await client.ConnectAsync(new IPEndPoint(ip, port)))
            {
                Output($"Connect to server [{txtIP.Text.Trim()}] success!");
            }
            else
            {
                Output($"Connect to server [{txtIP.Text.Trim()}] fail!");
            }

            this.Cursor = Cursors.Default;
            this.btnConn.Enabled = true;
        }

        private void Send()
        {
            var rnd = new Random();
            if (chkInterval.Checked)
            {
                SetBox(txtInt, (rnd.Next(1, 255) - 128).ToString());
                SetBox(txtNum, $"{rnd.Next(1, 255) - 128}.{rnd.Next(0, 255)}");
                SetBox(txtBit, rnd.Next(0, 255).ToString());
            }

            List<byte> list = new List<byte>();
            list.Add(byte.Parse(txtOn.Text));
            list.AddRange(EncodingHelper.HexToBytes(int.Parse(txtInt.Text).ToYmHex(2)));
            list.AddRange(EncodingHelper.HexToBytes(double.Parse(txtNum.Text).ToYmHex(4)));
            list.Add(byte.Parse(txtBit.Text));

            var req = IotRequest.CreateRequest(filter, txtMac.Text.Trim(), "0001", list.ToArray().ToHex());
            if (sessions.ContainsKey(req.Device.Mac))
            {
                req.SessionId = sessions[req.Device.Mac];
            }
            Send(req);
        }

        private void Send(IotRequest req)
        {
            var data = req.BuildBytes();

            Output($"Send-发送数据: {req.HeaderHex}-{req.Length.ToString("{00}")}-{req.Device.Mac}-{req.SessionId} {req.Command}");
            Output($"Send-封包加验: {req.HeaderHex}-{req.Length.ToString("{00}")} {req.BodyHex}");
            Output($"Send-加密数据: {req.HeaderHex}-{req.Length.ToString("{00}")} {req.EncryptedBody.ToHex()}");

            try
            {
                client.Send(data);
            }
            catch (Exception ex)
            {
                Output("Error: " + ex.GetMessage());
            }
        }

        private void Output(string msg)
        {
            if (txtOut.InvokeRequired)
            {
                txtOut.Invoke(new OutputAction(Output), msg);
                return;
            }
            txtOut.AppendText(msg);
            txtOut.AppendText("\r\n");
            if (txtOut.Lines.Length > 50)
            {
                txtOut.Clear();
            }
            txtOut.ScrollToCaret();
        }

        private void SetBox(TextBox box, string text)
        {
            if (box.InvokeRequired)
            {
                box.Invoke(new SetBoxText(SetBox), box, text);
            }
            else
            {
                box.Text = text;
            }
        }

        private void btnDisconn_Click(object sender, EventArgs e)
        {
            this.client.Close();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            var wifi = EncodingHelper.ToBytes(txtWifi.Text).ToHex();
            var hard = NaryHelper.ToYmHex(txtHard.Text.Trim(), 4);
            var soft = NaryHelper.ToYmHex(txtSoft.Text.Trim(), 4);

            var req = IotRequest.CreateRequest(filter, txtMac.Text.Trim(), "00F1", soft + hard + wifi);
            Send(req);
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            new Thread(Poller).Start();
        }

        private void Poller()
        {
            do
            {
                Send();
                Thread.Sleep(5000);
            }
            while (chkInterval.Checked);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtOut.Clear();
        }
    }
}
