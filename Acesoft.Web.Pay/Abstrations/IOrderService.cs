using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Acesoft.Data;
using Acesoft.Web.Pay.Entity;
using Essensoft.AspNetCore.Payment.Alipay;

namespace Acesoft.Web.Pay
{
    public interface IOrderService : IService<Pay_Order>
    {
        Pay_Order GetByRef(long refId);
        int Update(long id, decimal payMoney, string payTime, PayType payType);
    }
}
