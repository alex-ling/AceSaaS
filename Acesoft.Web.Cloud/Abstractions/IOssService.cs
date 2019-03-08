using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json.Linq;

namespace Acesoft.Web.Cloud
{
    public interface IOssService
    {
        JObject GetSignature(string bucket, string folder);
        void DeleteFile(string bucket, string key);
    }
}
