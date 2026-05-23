using BaseWinform.Attributes;
using Cataloger.Views;
using Cataloger.Communs;
using Cataloger.Composantes;
using Cataloger.Core.Entities.SystemsParameters.Dto;
using DevExpress.LookAndFeel;

namespace Cataloger.Composants
{
    public partial class SystemParameter : CatalogerComposante, ISystemParameterView
    {
        public SystemParameterDto? SkinStyleActif { get; set; } 

        public SystemParameter()
        {
            InitializeComponent();
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
            }
            cbboxSkinStyle.SelectedIndexChanged += cbboxSkinStyle_SelectedIndexChanged;
        }

        private void cbboxSkinStyle_SelectedIndexChanged(object? sender, EventArgs e)
        {
            WrapItem<SystemParameterDto>? item = (WrapItem<SystemParameterDto>)cbboxSkinStyle.Properties.Items[cbboxSkinStyle.SelectedIndex];

            // skinStyle!.ValString = item.Data.ValString;
            // systemParametersPresenter.ModifySystemParameter(skinStyle);
        }
    }
}
