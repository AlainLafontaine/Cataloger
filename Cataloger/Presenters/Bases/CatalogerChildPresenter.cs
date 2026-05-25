using BaseWinform.Presenters;
using Cataloger.Views;

namespace Cataloger.Presenters.Bases
{
    public class CatalogerChildPresenter<I> : ChildPresenter<I>, ICatalogerChildPresenter where I : ICatalogerChildView
    {
        public new dynamic Parent
        {
            get => (base._parent != null) ? base._parent : throw new NotImplementedException();
        }

        public CatalogerChildPresenter() { }
    }
}
