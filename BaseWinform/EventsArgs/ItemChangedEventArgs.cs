using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseWinform.EventsArgs
{
    public class ItemChangedEventArgs : EventArgs
    {
        private object item;
        private int index;
        private string? description;

        public ItemChangedEventArgs(object item, int index, string? description) 
        { 
            this.item = item;
            this.index = index;
            this.description = description;
        }

        public int Index { get => index;  }
        
        public string? Description { get => description; }

        public T ObtenirSelectedItem<T>() { return (T)item; }

    }
}
