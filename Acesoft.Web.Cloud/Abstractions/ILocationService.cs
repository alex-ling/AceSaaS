using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Web.Cloud
{
    public interface ILocationService
    {
        Location GetLoc(string longitude, string latitude);
    }
}
