using BaseWinform.Attributes;
using Cataloger.Presenters.Bases;
using Cataloger.Views;

namespace Cataloger.Presenters.LibrariesMngr
{
    [PresenterURL("libraries-manager")]
    public class LibraryMngrPresenter : CatalogerPresenter<ILibraryMngrView>
    {
        public LibraryMngrPresenter(
            ILibraryMngrView view
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
