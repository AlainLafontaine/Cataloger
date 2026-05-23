using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;
using System.IO;

namespace BaseWinform.Utilitaires
{
    public class WinformStateHelper
    {
        private WinformStateStore winformStateStore = new WinformStateStore();

        /// <summary 
        /// Fabrique une clé unique pour ce formulaire (type + Name).
        /// </summary>
        public string MakeKey(Form form) => $"{form.GetType().FullName}:{form.Name}";

        public bool Contains(string key) => winformStateStore.Contains(key);
            
        public void Remove(string key) => winformStateStore.Remove(key);

        /// <summary>
        /// Sauvegarde l'état complet du formulaire dans le store en mémoire.
        /// </summary>
        public void SaveFormState(Form form, string key)
        {
            WinformState state = new();

            foreach (var ctrl in EnumerateControls(form))
            {
                // Sauvegarde des valeurs des contrôles standards et DevExpress editors
                SaveControlValue(state, ctrl);

                // Sauvegarde des Layouts DevExpress (GridView, LayoutControl, etc.)
                SaveDevExpressLayout(state, ctrl);
            }

            winformStateStore.Save(key, state);
        }

        /// <summary>
        /// Restaure l'état du formulaire si présent dans le store.
        /// </summary>
        public void RestoreFormState(Form form, string key)
        {
            if (!winformStateStore.TryGet(key, out WinformState? state))
                return;

            ArgumentNullException.ThrowIfNull(state); 
            foreach (var ctrl in EnumerateControls(form))
            {
                RestoreControlValue(state, ctrl);
                RestoreDevExpressLayout(state, ctrl);
            }
        }

        /// <summary>
        /// Parcours récursif de tous les contrôles (y compris conteneurs).
        /// </summary>
        private IEnumerable<Control> EnumerateControls(Control root)
        {
            var stack = new Stack<Control>();
            stack.Push(root);
            while (stack.Count > 0)
            {
                var c = stack.Pop();
                foreach (Control child in c.Controls)
                    stack.Push(child);
                yield return c;
            }
        }

        #region Sauvegarde / Restauration des valeurs des contrôles

        private bool SaveControl<T>(
            WinformState state, 
            Control ctrl,
            Action<WinformState, T> saveState
        ) where T : Control
        {
            if (ctrl is T)
            {
                saveState(state, (T)ctrl);
                return true;
            }

            return false;
        }

        private void SaveControlValue(WinformState state, Control ctrl)
        {
            // Exige un Name unique pour identifier
            if (string.IsNullOrWhiteSpace(ctrl.Name)) return;

            // Contrôles WinForms standard
            // TextBox
            if (SaveControl<TextBox>(
                state,
                ctrl,
                (WinformState s, TextBox c) => s.Controls[c.Name] = c.Text
            )) return;

            // CheckBox
            if (SaveControl<CheckBox>(
                state,
                ctrl,
                (WinformState s, CheckBox c) => s.Controls[c.Name] = c.Checked
            )) return;

            // System.Windows.Forms.ComboBox
            if (SaveControl<System.Windows.Forms.ComboBox>(
                state,
                ctrl,
                (WinformState s, System.Windows.Forms.ComboBox c) =>
                {
                    s.Controls[$"{c.Name}#SelectedIndex"] = c.SelectedIndex;
                    s.Controls[$"{c.Name}#Text"] = c.Text;
                }
            )) return;

            // NumericUpDown
            if (SaveControl<NumericUpDown>(
                state,
                ctrl,
                (WinformState s, NumericUpDown c) => s.Controls[c.Name] = c.Value
            )) return;

            // NumericUpDown
            if (SaveControl<DateTimePicker>(
                state,
                ctrl,
                (WinformState s, DateTimePicker c) => s.Controls[c.Name] = c.Value
            )) return;

            // NumericUpDown
            if (SaveControl<RadioButton>(
                state,
                ctrl,
                (WinformState s, RadioButton c) => s.Controls[c.Name] = c.Checked
            )) return;

            // Éditeurs DevExpress (tous dérivent de BaseEdit)
            // TextEdit
            if (SaveControl<TextEdit>(
                state,
                ctrl,
                (WinformState s, TextEdit c) => s.Controls[c.Name] = c.Text
            )) return;

            // CheckEdit
            if (SaveControl<CheckEdit>(
                state,
                ctrl,
                (WinformState s, CheckEdit c) => s.Controls[c.Name] = c.Checked
            )) return;

            // RadioGroup
            if (SaveControl<RadioGroup>(
                state,
                ctrl,
                (WinformState s, RadioGroup c) => s.Controls[c.Name] = c.SelectedIndex
            )) return;

            // RadioGroup
            if (SaveControl<ToggleSwitch>(
                state,
                ctrl,
                (WinformState s, ToggleSwitch c) => s.Controls[c.Name] = c.IsOn
            )) return;

            // ComboBoxEdit
            if (SaveControl<ComboBoxEdit>(
                state,
                ctrl,
                (WinformState s, ComboBoxEdit c) =>
                {
                    s.Controls[$"{c.Name}#SelectedIndex"] = c.SelectedIndex;
                    s.Controls[$"{c.Name}#Text"] = c.Text;
                    s.Controls[$"{c.Name}#Item"] = c.Properties.Items;
                }
            )) return;

            // SpinEdit - decimal
            if (SaveControl<SpinEdit>(
                state,
                ctrl,
                (WinformState s, SpinEdit c) => s.Controls[c.Name] = c.Value
            )) return;

            // DateEdit - généralement DateTime?
            if (SaveControl<DateEdit>(
                state,
                ctrl,
                (WinformState s, DateEdit c) => s.Controls[c.Name] = c.EditValue 
            )) return;
        }

        private bool RestoreControl<T>(
            WinformState state,
            Control ctrl,
            Action<WinformState, T> restoreState
        ) where T : Control
        {
            if (ctrl is T)
            {
                restoreState(state, (T)ctrl);
                return true;
            }

            return false;
        }

        private void RestoreControlValue(WinformState state, Control ctrl)
        {
            // Exige un Name unique pour identifier
            if (string.IsNullOrWhiteSpace(ctrl.Name)) return;

            // WinForms standard
            // TextBox
            if (RestoreControl<TextBox>(
                state,
                ctrl,
                (WinformState s, TextBox c) =>
                {
                    if (s.Controls.TryGetValue(c.Name, out var tbVal))
                        c.Text = tbVal?.ToString() ?? string.Empty;
                }
            )) return;

            if (RestoreControl<CheckBox>(
                state,
                ctrl,
                (WinformState s, CheckBox c) =>
                {
                    if (s.Controls.TryGetValue(c.Name, out var cbVal) && cbVal is bool b1)
                        c.Checked = b1;
                }
            )) return;

            if (RestoreControl<System.Windows.Forms.ComboBox>(
                state,
                ctrl,
                (WinformState s, System.Windows.Forms.ComboBox c) =>
                {
                    if (s.Controls.TryGetValue($"{c.Name}#SelectedIndex", out var si) && si is int i)
                        c.SelectedIndex = i;

                    if (s.Controls.TryGetValue($"{c.Name}#Text", out var t))
                        c.Text = t?.ToString() ?? c.Text;
                }
            )) return;

            if (RestoreControl<NumericUpDown>(
                state,
                ctrl,
                (WinformState s, NumericUpDown c) =>
                {
                    if (s.Controls.TryGetValue(c.Name, out var ndVal) && ndVal is decimal d)
                        c.Value = ClampDecimal(d, c.Minimum, c.Maximum);
                }
            )) return;

            if (RestoreControl<DateTimePicker>(
                state,
                ctrl,
                (WinformState s, DateTimePicker c) =>
                {
                    if (s.Controls.TryGetValue(c.Name, out var dtVal) && dtVal is DateTime dt)
                        c.Value = dt;
                }
            )) return;

            if (RestoreControl<RadioButton>(
                state,
                ctrl,
                (WinformState s, RadioButton c) =>
                {
                    if (s.Controls.TryGetValue(c.Name, out var rbVal) && rbVal is bool b2)
                        c.Checked = b2;
                }
            )) return;

            // DevExpress
            // TextEdit
            if (RestoreControl<TextEdit>(
                state,
                ctrl,
                (WinformState s, TextEdit c) =>
                {
                    if (s.Controls.TryGetValue(c.Name, out var teVal))
                        c.Text = teVal?.ToString() ?? string.Empty;
                }
            )) return;

            // CheckEdit
            if (RestoreControl<CheckEdit>(
                state,
                ctrl,
                (WinformState s, CheckEdit c) =>
                {
                    if (s.Controls.TryGetValue(c.Name, out var ceVal) && ceVal is bool cb)
                        c.Checked = cb;
                }
            )) return;

            // ComboBoxEdit
            if (RestoreControl<ComboBoxEdit>(
                state,
                ctrl,
                (WinformState s, ComboBoxEdit c) =>
                {
                    if (s.Controls.TryGetValue($"{c.Name}#SelectedIndex", out var si) && si is int i)
                        c.SelectedIndex = i;

                    if (s.Controls.TryGetValue($"{c.Name}#Text", out var t1))
                        c.Text = t1?.ToString() ?? c.Text;

                    if (s.Controls.TryGetValue($"{c.Name}#Item", out var t2))
                    {
                        var items = s.Controls[$"{c.Name}#Item"];
                        
                        if (items != null)
                        {
//                            foreach (var item in items)
                            {

                            }
                        }
                        //c.Properties?.Items = s.Controls[$"{c.Name}#Item"];
                    }
                    c.Text = t2?.ToString() ?? c.Text;

                }
            )) return;

            // SpinEdit
            if (RestoreControl<SpinEdit>(
                state,
                ctrl,
                (WinformState s, SpinEdit c) =>
                {
                    if (s.Controls.TryGetValue(c.Name, out var seVal))
                    {
                        if (seVal is decimal dec)
                            c.Value = ClampDecimal(dec, Convert.ToDecimal(c.Properties.MinValue), Convert.ToDecimal(c.Properties.MaxValue));
                        else if (decimal.TryParse(seVal?.ToString(), out var parsed))
                            c.Value = ClampDecimal(parsed, Convert.ToDecimal(c.Properties.MinValue), Convert.ToDecimal(c.Properties.MaxValue));
                    }
                }
            )) return;

            // DateEdit
            if (RestoreControl<DateEdit>(
                state,
                ctrl,
                (WinformState s, DateEdit c) =>
                {
                    if (s.Controls.TryGetValue(c.Name, out var deVal))
                    {
                        // EditValue peut être DateTime ou null
                        if (deVal is DateTime dt)
                            c.EditValue = dt;
                        else
                            c.EditValue = deVal; // fallback
                    }
                }
            )) return;

            // RadioGroup
            if (RestoreControl<RadioGroup>(
                state,
                ctrl,
                (WinformState s, RadioGroup c) =>
                {
                    if (s.Controls.TryGetValue(c.Name, out var rgVal) && rgVal is int idx)
                        c.SelectedIndex = idx;
                }
            )) return;

            // ToggleSwitch
            if (RestoreControl<ToggleSwitch>(
                state,
                ctrl,
                (WinformState s, ToggleSwitch c) =>
                {
                    if (s.Controls.TryGetValue(c.Name, out var tsVal) && tsVal is bool on)
                        c.IsOn = on;
                }
            )) return;
        }

        private decimal ClampDecimal(decimal value, decimal min, decimal max)
            => value < min ? min : (value > max ? max : value);

        #endregion

        #region Sauvegarde / Restauration des layouts DevExpress (en mémoire via Base64)

        private void SaveDevExpressLayout(WinformState state, Control ctrl)
        {
            if (string.IsNullOrWhiteSpace(ctrl.Name))
                return;

            // GridControl -> GridView (layout colonnes, ordres, tri, grouping, etc.)
            if (ctrl is GridControl gc && gc.MainView is GridView gv)
            {
                using var ms = new MemoryStream();
                gv.SaveLayoutToStream(ms);
                state.DevExpressLayouts[ctrl.Name] = Convert.ToBase64String(ms.ToArray());
                return;
            }

            // LayoutControl -> positions, visibilités, tailles
            if (ctrl is LayoutControl lc)
            {
                using var ms = new MemoryStream();
                lc.SaveLayoutToStream(ms);
                state.DevExpressLayouts[ctrl.Name] = Convert.ToBase64String(ms.ToArray());
                return;
            }

            // Vous pouvez étendre ici pour DockManager, etc. (si SaveLayoutToStream/RestoreLayoutFromStream disponibles)
            // Exemple:
            // if (ctrl is DevExpress.XtraBars.Docking.DockManager dm) { ... }
        }

        private void RestoreDevExpressLayout(WinformState state, Control ctrl)
        {
            if (string.IsNullOrWhiteSpace(ctrl.Name))
                return;

            if (state.DevExpressLayouts.TryGetValue(ctrl.Name, out var base64) && !string.IsNullOrEmpty(base64))
            {
                var bytes = Convert.FromBase64String(base64);
                using var ms = new MemoryStream(bytes);

                if (ctrl is GridControl gc && gc.MainView is GridView gv)
                {
                    gv.RestoreLayoutFromStream(ms);
                    return;
                }

                if (ctrl is LayoutControl lc)
                {
                    lc.RestoreLayoutFromStream(ms);
                    return;
                }
            }
        }

        #endregion

    }
}
