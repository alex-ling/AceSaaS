using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Acesoft.Util
{
    public class WebServiceHelper
    {
        private static ConcurrentDictionary<string, string> xmlnss = new ConcurrentDictionary<string, string>();

        public static string Execute(string url, string method, IDictionary<string, object> @params)
        {
            var xmlns = xmlnss.GetOrAdd(url, key =>
            {
                var xml = HttpHelper.HttpGet(url + "?wsdl");
                var doc = new XmlDocument();
                doc.LoadXml(xml);
                return doc.SelectSingleNode("//@targetNamespace").Value;
            });

            var soapXml = new StringBuilder();
            soapXml.Append($"<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:tok=\"{xmlns}\">");
            soapXml.AppendLine("<soapenv:Body>");
            soapXml.AppendLine($"<tok:{method}>");
            foreach (var p in @params)
            {
                soapXml.AppendLine($"<{p.Key}>{p.Value}</{p.Key}>");
            }
            soapXml.AppendLine($"</tok:{method}>");
            soapXml.AppendLine("</soapenv:Body>");
            soapXml.AppendLine("</soapenv:Envelope>");

            var xmlRes = HttpHelper.HttpPost(url, soapXml.ToString(), null, HttpHelper.ContentTypeText);
            var soapRes = new XmlDocument();
            soapRes.LoadXml(xmlRes);
            var result = soapRes.SelectNodes("//return");
            if (result.Count > 0)
            {
                return result[0].InnerXml;
            }
            return null;
        }
    }
}
