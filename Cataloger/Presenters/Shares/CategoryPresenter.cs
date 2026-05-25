using BaseWinform.Attributes;
using Cataloger.Presenters.Bases;
using Cataloger.Views;

namespace Cataloger.Presenters.Shares
{
    public class CategoryPresenter : CatalogerChildPresenter<ICategoryView>
    {

        public CategoryPresenter(
            ICategoryView view
        )
        : base(view)
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
