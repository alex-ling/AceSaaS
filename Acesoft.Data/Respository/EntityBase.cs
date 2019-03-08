using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Util;
using Dapper.Contrib.Extensions;

namespace Acesoft.Data
{
    public class EntityBase : IEntity
    {
        [Computed]
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

        public EntityBase InitializeId()
        {
            Id = App.IdWorker.NextId();
            return this;
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
