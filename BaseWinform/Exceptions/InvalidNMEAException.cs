using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseWinform.Exceptions
{
    public class InvalidNMEAException : Exception
    {
        public InvalidNMEAException() { }

        public InvalidNMEAException(string message)
        : base(message)
        {
        }

        public InvalidNMEAException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
