using BaseWinform.Presenters;
using Cataloger.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cataloger.Presenters.Bases
{
    public class CatalogerChildPresenter<I> : BasePresenter<I>, ICatalogerChildPresenter where I : ICatalogerChildView
    {
        public CatalogerChildPresenter(I presenter)
        : base(presenter) {
        }
    }
}
