using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseWinform.EventsArgs
{
    /// <summary>
    /// Contenant pour les transferts de la correspondance entre les différents
    /// objet
    /// </summary>
    public class CorrespondanceEventArgs : EventArgs
    {
        private dynamic TypeCorrespondance { get; set; }
        private dynamic Value { get; set; }

        public CorrespondanceEventArgs(
            dynamic typeCorrespondance,
            dynamic value
        )
        {
            this.TypeCorrespondance = typeCorrespondance;
            this.Value = value;
        }

        public bool TypeMessageEstType<T>() { return typeof(T) == GetTypeMessage(); }

        public T GetTypeMessage<T>() { return this.TypeCorrespondance; }
        public T GetValue<T>() { return this.Value; }

        private Type? GetTypeMessage() => (TypeCorrespondance is null) ? null : ((object)TypeCorrespondance).GetType();
    }
}
