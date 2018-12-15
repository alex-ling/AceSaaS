using System;
using System.Collections.Generic;
using System.Text;

using Dapper.Contrib.Extensions;
using Acesoft.NetCore.Util;

namespace Acesoft.Data
{
    public interface IEntity
    {
        long Id { get; set; }

        string GetHashId();
    }

    public class EntityBase : IEntity
    {
        private string hashId;

        [ExplicitKey]
        public long Id { get; set; }

        public void InitInsert()
        {
            Id = App.IdWorker.NextId();
        }

        public string GetHashId()
        {
            if (!hashId.HasValue())
            {
                hashId = NaryHelper.FromNary(Id, 36);
            }
            return hashId;
        }

        public override bool Equals(object obj)
        {
            if (obj is EntityBase o && Id == o.Id)
            {
                return true;
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Convert.ToInt32(Id);
        }
    }
}
