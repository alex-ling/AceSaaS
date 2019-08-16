using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Platform
{
    public interface ISeedService
    {
        string Create(string seed, string prefix, int length, bool autoSave, int? nary);
        void Save(string seed, string value);
    }
}
