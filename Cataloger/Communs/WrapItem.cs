using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cataloger.Communs
{
    public class WrapItem<D>
    {
        private readonly D data;
        private readonly string description;

        public WrapItem(D data, string description)
        {
            this.data = data;
            this.description = description;
        }

        public override string ToString() => description;

        public D Data => data;
    }
}
