using BaseWinform.EventsArgs;
using BaseWinform.Utilitaires;
using DevExpress.Utils.Extensions;
using DevExpress.XtraEditors;
using System.ComponentModel;

namespace BaseWinform.Controls
{
    public enum TypeAccesDirectItem
    {
        Puce,
        Liste
    }

    public partial class IterateurDataCtrl : BaseCtrl
    {
        public int SelectedIndex { get => selectedIndex; set => MajIndexCourant(AccesItem.DirectItem, value); }

        private TypeAccesDirectItem typeAccesDirectItem;

        // ----- Déclaration pour le support de Items
        private readonly BindingList<IterateurItem> items = new ();
        private int selectedIndex = -1;

        // ----- Les propriétés du contrôle 
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("BaseWinform")]
        [Description("Type pour sélectionner directement un item")]
        public TypeAccesDirectItem TypeAccesDirectItem
        {
            get => typeAccesDirectItem;
            set
            {
                typeAccesDirectItem = value;
                
                // Réinitialiser à l'état initiale
                items.Clear();
                selectedIndex = -1;
                accesDirectDataCtrl.Controls.Clear();

                // Création des ou du contrôle pour l'accès directe à un item
                switch (typeAccesDirectItem)
                {
                    case TypeAccesDirectItem.Puce:
                        // Ajoute les puces
                        CreerPuces();
                        break;

                    case TypeAccesDirectItem.Liste:
                        CreerListBox();
                        break;
                }
            }
        }
        public int Count { get => items.Count; }

        // ----- Événements -----

        [Category("BaseWinform")]
        [Description("Notifie le parent que l'item sélectionné a changé.")]
        public event EventHandler<EventsArgs.ItemChangedEventArgs>? SelectedItemChanged;

        [Category("BaseWinform")]
        [Description("Notifie le parent avant le changement de la sélectionne.")]
        public event EventHandler<EventsArgs.BeforeItemSelectionChangedEventArgs>? BeforeSelectedItemChanged;

        public int NbrPuce { get; set; } = 5;

        private enum AccesItem
        {
            Premier,
            Precedent,
            Suivant,
            Dernier,
            DirectItem
        }

        public IterateurDataCtrl() : base()
        {
            InitializeComponent();

            // État initial des boutons
            btnPremier.Enabled = false;
            btnPrecedent.Enabled = false;
            btnSuivant.Enabled = false; 
            btnDernier.Enabled = false;
        }

        // ----- API utilitaire -----

        public void Clear()
        {
            // État initial des boutons
            btnPremier.Enabled = false;
            btnPrecedent.Enabled = false;
            btnSuivant.Enabled = false;
            btnDernier.Enabled = false;

            items.Clear();
            selectedIndex = -1;
            lblItemCourant.Text = "Aucun résultat";

            if (typeAccesDirectItem == TypeAccesDirectItem.Liste)
            {
                ComboBoxEdit cboxDescription = (ComboBoxEdit)accesDirectDataCtrl.Controls[0];

                cboxDescription.Properties.Items.Clear();
                cboxDescription.EditValue = null;
            }
        }

        public int Add<T>(T item, string? description = null)
        {
            IterateurItem iterateurItem = new (item!, description);

            items.Add(iterateurItem);

            if (typeAccesDirectItem == TypeAccesDirectItem.Liste)
            {
                ComboBoxEdit cboxDescription = (ComboBoxEdit)accesDirectDataCtrl.Controls[0];

                if (description != null)
                {
                    cboxDescription.Properties.Items.Add(description);
                }
                else
                {
                    cboxDescription.Properties.Items.Add(item);
                }
            }

            return items.Count;
        }

        public T ObtenirItem<T>(int index) { return (T)items[selectedIndex].item;  }

        private void btnPremier_Click(object sender, EventArgs e) { MajIndexCourant(AccesItem.Premier); }
        private void btnPrecedent_Click(object sender, EventArgs e) { MajIndexCourant(AccesItem.Precedent); }
        private void btnSuivant_Click(object sender, EventArgs e) { MajIndexCourant(AccesItem.Suivant); }
        private void btnDernier_Click(object sender, EventArgs e) { MajIndexCourant(AccesItem.Dernier); }

        private bool MajIndexCourant(AccesItem accessItem, int? indexItemSelected = null)
        {
            if (items.Count == 0) return false;

            int indexAvenir = selectedIndex;
            
            switch (accessItem)
            {
                case AccesItem.Premier:
                    indexAvenir = 0;
                    break;

                case AccesItem.Precedent:
                    indexAvenir--;
                    break;

                case AccesItem.Suivant:
                    indexAvenir++;
                    break;

                case AccesItem.Dernier:
                    indexAvenir = items.Count() - 1;
                    break;

                case AccesItem.DirectItem:
                    indexAvenir = (int)indexItemSelected!;
                    break;

                default:
                    indexAvenir = -1;
                    break;
            }

            BeforeItemSelectionChangedEventArgs beforeItemSelectionChangedArgs = new(selectedIndex, indexAvenir);

            BeforeSelectedItemChanged?.Invoke(this, beforeItemSelectionChangedArgs);
            if (beforeItemSelectionChangedArgs.AnnulerSelection) return false;

            selectedIndex = indexAvenir;

            // Gestion des boutons pour le prochain appelle
            bool backup = Enabled;

            Enabled = true;
            btnPremier.Enabled = selectedIndex > 0;
            btnPrecedent.Enabled = btnPremier.Enabled;
            btnSuivant.Enabled = selectedIndex != (items.Count - 1);
            btnDernier.Enabled = btnSuivant.Enabled;
            Enabled = backup;

            // Affiche l'indexe
            if (items.Count() != 0)
            {
                lblItemCourant.Text = $"Item {selectedIndex + 1} / {items.Count}";
            }
            else
            {
                lblItemCourant.Text = "Aucun résultat";
            }

            switch (typeAccesDirectItem)
            {
            case TypeAccesDirectItem.Puce:
                {
                    if (items.Count() > NbrPuce)
                    {
                        // Afficher les valeurs dans les puces
                        int borneInf = selectedIndex - NbrPuce / 2;
                        int borneSup = borneInf + NbrPuce;

                        if (borneInf < 1) { borneInf = 1; }
                        if (borneSup >= items.Count()) { borneInf = items.Count() + 1 - NbrPuce; }

                        int id = borneInf;
                        foreach (LabelControl puceCtrl in accesDirectDataCtrl.Controls)
                        {
                            int itemIndex = id - 1;
                            IterateurItem item = items[itemIndex];

                            puceCtrl.ForeColor = (itemIndex == selectedIndex) ? Color.Red : Color.Black;
                            puceCtrl.Tag = itemIndex;
                            puceCtrl.Text = $"{id}";
                            puceCtrl.ToolTip = item.description ?? "";
                            id++;
                        }
                    }
                    else
                    {
                        int nbrItem = items.Count();
                        int borneInf = (int)(Math.Floor(NbrPuce / 2.0 + 0.5) - Math.Floor(nbrItem / 2.0) + ((NbrPuce % 2 == 0 && nbrItem % 2 == 0) ? 1 : 0));
                        int borneSup = (borneInf - 1) + nbrItem;

                        foreach (LabelControl puceCtrl in accesDirectDataCtrl.Controls)
                        {
                            puceCtrl.Text = "";
                            puceCtrl.ToolTip = null;
                            puceCtrl.Tag = null;
                            puceCtrl.Enabled = false;
                        }

                        int indexItem = 0;
                        for (int index = borneInf - 1; index < borneSup; index++)
                        {
                            LabelControl puceCtrl = (LabelControl)accesDirectDataCtrl.Controls[index];
                            IterateurItem item = items[indexItem];

                            puceCtrl.Enabled = true;
                            puceCtrl.Text = $"{indexItem + 1}";
                            puceCtrl.ToolTip = item.description ?? "";
                            puceCtrl.Tag = indexItem;
                            puceCtrl.ForeColor = (indexItem == selectedIndex) ? Color.Red : Color.Black;

                            indexItem++;
                        }
                    }
                }
                break;
    
            case TypeAccesDirectItem.Liste:
                {
                    ComboBoxEdit cboxDescription = (ComboBoxEdit)accesDirectDataCtrl.Controls[0];

                    cboxDescription.SelectedIndexChanged -= cboxDescription_SelectedIndexChanged;
                    cboxDescription.SelectedIndex = selectedIndex;
                    cboxDescription.SelectedIndexChanged += cboxDescription_SelectedIndexChanged;
                }
                break;
            }

            // Notification au parent de index courant
            var args = new EventsArgs.ItemChangedEventArgs(items[selectedIndex].item, selectedIndex, items[selectedIndex].description);

            SelectedItemChanged?.Invoke(this, args);

            return true;
        }

        private void CreerPuces()
        {
            var labelControl = new DevExpress.XtraEditors.LabelControl();
            int height = accesDirectDataCtrl.Size.Height;
            int width = accesDirectDataCtrl.Size.Width / NbrPuce;

            for (int index = 0; index < NbrPuce; index++)
            {
                var lblCtrl = new LabelControl();


                lblCtrl.Size = new Size(width, height);
                lblCtrl.Location = new Point(index * width, 0);
                lblCtrl.Text = ((char)(65 + index)).ToString();
                lblCtrl.Appearance.Font = new Font("Tahoma", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
                lblCtrl.Appearance.Options.UseFont = true;
                lblCtrl.Appearance.Options.UseTextOptions = true;
                lblCtrl.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                lblCtrl.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                lblCtrl.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
                lblCtrl.Name = $"lblPos_{index}";
                lblCtrl.Click += (s, e) => MajIndexCourant(AccesItem.DirectItem, (int)((LabelControl)s!).Tag!);

                accesDirectDataCtrl.AddControl(lblCtrl);
            }
        }

        private void CreerListBox()
        {
            ComboBoxEdit cboxDescription = new();

            cboxDescription.Name = "cboxDescription";
            cboxDescription.Location = new Point(0, 8);
            cboxDescription.Width = accesDirectDataCtrl.Width;
            cboxDescription.Properties.Appearance.Font = new Font("Tahoma", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cboxDescription.Properties.Appearance.Options.UseFont = true;
            cboxDescription.Properties.AppearanceDropDown.Font = new Font("Tahoma", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cboxDescription.Properties.AppearanceDropDown.Options.UseFont = true;
            cboxDescription.Size = new Size(accesDirectDataCtrl.Width, accesDirectDataCtrl.Height);
            cboxDescription.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cboxDescription.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            cboxDescription.SelectedIndexChanged += cboxDescription_SelectedIndexChanged;

            accesDirectDataCtrl.AddControl(cboxDescription);
        }

        private void cboxDescription_SelectedIndexChanged(object? sender, EventArgs? e)
        {
            ComboBoxEdit cboxDescription = (ComboBoxEdit)accesDirectDataCtrl.Controls[0];

            if (cboxDescription.SelectedIndex == selectedIndex) return; 

            if (!MajIndexCourant(AccesItem.DirectItem, cboxDescription.SelectedIndex))
            {
                cboxDescription.SelectedIndex = selectedIndex;
            }
        }
    }
}