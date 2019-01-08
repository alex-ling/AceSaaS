using System;
using System.Collections.Generic;
using System.Text;

using Dapper.Contrib.Extensions;

using Acesoft.Util;

namespace Acesoft.Data
{
    public interface IEntity
    {
        long Id { get; set; }
        string HashId { get; }
    }

    public class EntityBase : IEntity
    {
        public string HashId { get; private set; }

        private long id;
        [ExplicitKey]
        public long Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
                HashId = NaryHelper.FromNary(value, 36);
            }
        }

        public void InitInsert()
        {
            Id = DataContext.IdWorker.NextId();
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
            return Id.GetHashCode();
        }
    }
}
