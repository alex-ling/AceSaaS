using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Data
{
    public interface IEntity
    {
        long Id { get; set; }
        string HashId { get; }
    }
}
