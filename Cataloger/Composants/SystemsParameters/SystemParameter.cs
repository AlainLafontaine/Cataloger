using BaseWinform.Attributes;
using BaseWinform.Composantes;
using Cataloger.Communs;
using Cataloger.Composantes;
using Cataloger.Core.Entities.SystemsParameters.Dto;
using Cataloger.Views;
using DevExpress.LookAndFeel;

namespace Cataloger.Composants
{
    public partial class SystemParameter : CatalogerComposante, ISystemParameterView
    {
        public event EventHandler? OnSkinStyleChanged;


        public SystemParameterDto? SkinStyleActif { get; set; } 

        public SystemParameter()
        {
            InitializeComponent();
            BaseEditIgnoreIsDirtyHelper.SetIgnoreIsDirty(cbboxSkinStyle, true);
        }

        public void LoadSkinStyles(
            IEnumerable<SystemParameterDto> skinStyles,
            SystemParameterDto skinStyleActif
        ) 
        {
            SkinStyleActif = skinStyleActif;

            cbboxSkinStyle.SelectedIndexChanged -= cbboxSkinStyle_SelectedIndexChanged;
            foreach (SystemParameterDto skinStyle in skinStyles)
            {
                if (skinStyle.Key != "Actif")
                {
                    int index = cbboxSkinStyle.Properties.Items.Add(new WrapItem<SystemParameterDto>(skinStyle, skinStyle.Description));

                    if (skinStyleActif.ValString == skinStyle.ValString)
                    {
                        cbboxSkinStyle.SelectedIndex = index;
                    }
                }
                else
                {
                    SkinStyleActif = skinStyle;
                }
            }
            cbboxSkinStyle.SelectedIndexChanged += cbboxSkinStyle_SelectedIndexChanged;
        }

        private void cbboxSkinStyle_SelectedIndexChanged(object? sender, EventArgs e)
        {
            WrapItem<SystemParameterDto>? item = (WrapItem<SystemParameterDto>)cbboxSkinStyle.Properties.Items[cbboxSkinStyle.SelectedIndex];

            SkinStyleActif!.ValString = item.Data.ValString;
            OnSkinStyleChanged?.Invoke(this, e);
        }
    }
}
