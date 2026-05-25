using System;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraEditors;

namespace BaseWinform.Composantes
{
    [ProvideProperty("IgnoreIsDirty", typeof(BaseEdit))]
    public class IgnoreIsDirtyExtender : Component, IExtenderProvider
    {
        private readonly Dictionary<BaseEdit, bool> _values = new();

        // Indique à WinForms que l’extension s’applique à BaseEdit
        public bool CanExtend(object extendee)
        {
            return extendee is BaseEdit;
        }

        // Lecture de la propriété
        public bool GetIgnoreIsDirty(BaseEdit control)
        {
            return _values.TryGetValue(control, out var value) && value;
        }

        // Écriture de la propriété
        public void SetIgnoreIsDirty(BaseEdit control, bool value)
        {
            _values[control] = value;
        }
    }

    public static class BaseEditIgnoreIsDirtyHelper
    {
        private static readonly Dictionary<BaseEdit, bool> _values = new();

        public static void SetIgnoreIsDirty(BaseEdit edit, bool value = true)
        {
            _values[edit] = value;
        }

        public static bool GetIgnoreIsDirty(BaseEdit edit)
        {
            return _values.TryGetValue(edit, out var value) && value;
        }
    }
}
