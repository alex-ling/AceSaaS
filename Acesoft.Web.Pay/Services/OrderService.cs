using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acesoft.Data;
using Acesoft.Web.Pay.Entity;
using Essensoft.AspNetCore.Payment.Alipay;
using Essensoft.AspNetCore.Payment.Alipay.Notify;

namespace Acesoft.Web.Pay.Services
{
    public class OrderService : Service<Pay_Order>, IOrderService
    {
        public Pay_Order GetByRef(long refId)
        {
            var sql = "select * from pay_order where ref_id=@refid";
            return Session.QueryFirst<Pay_Order>(sql, new { refId });
        }

        public int Update(long id, decimal payMoney, string payTime, int payType)
        {
            var sql = "update pay_order set " +
                "dupdate=@dupdate," +
                "pay_money=@paymoney," +
                "pay_time=@paytime," +
                "pay_type=@paytype," +
                "status=2 " +
                "where id=@id";
            return Session.Execute(sql, new
            {
                id,
                dupdate = DateTime.Now,
                payMoney,
                payTime,
                payType
            });
        }

        public async Task<int> AlipayNotify(IAlipayNotifyClient client, long id)
        {
            try
            {
                var notify = await client.ExecuteAsync<AlipayTradePagePayReturn>(App.Context.Request);
                return Update(id,
                    notify.TotalAmount.ToObject<decimal>(),
                    notify.Timestamp,
                    (int)PayType.Alipay);
            }
            catch
            {
                return 0;
            }
        }
    }
}