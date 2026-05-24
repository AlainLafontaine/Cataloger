using BaseWinform.Attributes;
using Cataloger.Presenters.Bases;
using Cataloger.Views;

namespace Cataloger.Presenters.Propects
{
    [PresenterURL("propects")]
    public class PropectPresenter : CatalogerPresenter<IPropectView>
    {
        public PropectPresenter(
            IPropectView view
        ) : base(view)
        {
        }

        public override void InitPresenter(object? sender, EventArgs? e)
        {
            base.InitPresenter(sender, e);
        }

        public override void ReleasePresenter()
        {
            base.ReleasePresenter();
        }
    }
}
