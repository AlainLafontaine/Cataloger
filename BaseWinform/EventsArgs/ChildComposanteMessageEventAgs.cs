using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseWinform.EventsArgs
{
    public class ChildComposanteMessageEventAgs : EventArgs
    {
        private dynamic TypeMessage { get; set; }
        private dynamic Value { get; set; }

        public ChildComposanteMessageEventAgs(
            dynamic typeMessage,
            dynamic value
        ) 
        {  
            this.TypeMessage = typeMessage; 
            this.Value = value;
        }

        public bool TypeMessageEstType<T>() { return typeof(T) == GetTypeMessage();  }

        public T GetTypeMessage<T>() { return this.TypeMessage; }
        public T GetValue<T>() { return this.Value; }

        private Type? GetTypeMessage() => (TypeMessage is null) ? null : ((object)TypeMessage).GetType();
    }
}
