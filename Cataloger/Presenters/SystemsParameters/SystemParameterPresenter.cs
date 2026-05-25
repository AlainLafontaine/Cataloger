using BaseWinform.Attributes;
using Cataloger.Communs;
using Cataloger.Core.Entities.SystemsParameters.Dto;
using Cataloger.Presenters.Bases;
using Cataloger.Views;
using DevExpress.LookAndFeel;


namespace Cataloger.Presenters.SystemsParameters
{
    [PresenterURL("systems-parameters")]
    public class SystemParameterPresenter : CatalogerPresenter<ISystemParameterView>
    {
        private readonly SystemParameterService systemParameterService;

        public SystemParameterPresenter(
            ISystemParameterView view,
            SystemParameterService systemParameterService
        ) : base(view)
        { 
            this.systemParameterService = systemParameterService;
        }

        public override void InitPresenter(object? sender, EventArgs? e)
        {
            base.InitPresenter(sender, e);

            IEnumerable<SystemParameterDto> skinStyles = systemParameterService.GetListParametrSystemParameterFromSection("Skin style");
            SystemParameterDto skinStyleActif = skinStyles.First(x => x.Key == "Actif");

            Composante!.LoadSkinStyles(skinStyles, skinStyleActif);
            Composante.OnSkinStyleChanged += HandleSkinStyleChanged;
        }

        public override void ReleasePresenter()
        {
            Composante!.OnSkinStyleChanged -= HandleSkinStyleChanged;

            base.ReleasePresenter(); 
        }


        private void HandleSkinStyleChanged(object? sender, EventArgs e)
        {
            SystemParameterDto skinStyleActif = Composante!.SkinStyleActif!;

            systemParameterService.ModifySystemParameter(skinStyleActif);
            UserLookAndFeel.Default.SetSkinStyle(Composante!.SkinStyleActif!.ValString);
        }
    }
}
