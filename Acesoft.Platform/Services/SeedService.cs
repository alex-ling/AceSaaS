using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Data;
using Acesoft.Platform.Entity;
using Acesoft.Util;

namespace Acesoft.Platform.Services
{
    public class SeedService : Service<Sys_Seed>, ISeedService
    {
        public static readonly object SyncObj = new object();

        public Sys_Seed GetSeed(string seed)
        {
            var sql = "select* from sys_seed where name=@seed";
            return Session.QueryFirst<Sys_Seed>(sql, new
            {
                seed
            });
        }

        public string Create(string name, string prefix, int length, bool autoSave, int? nary)
        {
            var value = "";
            name += prefix;

            lock (SyncObj)
            {
                var seed = GetSeed(name);
                if (seed != null && seed.Value.Length != prefix.Length + length)
                {
                    base.Delete(seed);
                    seed = null;
                }

                if (seed == null)
                {
                    value = $"{prefix}{"1".PadLeft(length, '0')}";

                    seed = new Sys_Seed();
                    seed.InitializeId();
                    seed.Name = name;
                    seed.Value = $"{prefix}{"0".PadLeft(length, '0')}";
                    seed.DCreate = DateTime.Now;
                    seed.DUpdate = DateTime.Now;
                    base.Insert(seed);
                }
                else
                {
                    var preVal = seed.Value.Substring(prefix.Length).ToUpper();
                    string curVal = null;

                    if (!nary.HasValue)
                    {
                        if (int.TryParse(preVal, out int result))
                        {
                            curVal = (++result).ToString();
                        }
                        else
                        {
                            nary = 36;
                        }
                    }

                    if (!curVal.HasValue())
                    {
                        curVal = NaryHelper.FromNary(NaryHelper.ToNary(curVal, nary.Value) + 1, nary.Value);
                    }

                    value = prefix + curVal.PadLeft(length, '0');
                }

                if (autoSave)
                {
                    Save(name, value);
                }
                
                return value;
            }
        }

        public void Save(string name, string value)
        {
            var seed = GetSeed(name);
            seed.Value = value;
            seed.DUpdate = DateTime.Now;
            base.Update(seed);
        }
    }
}
