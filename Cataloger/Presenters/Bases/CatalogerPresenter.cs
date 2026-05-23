using BaseWinform.Presenters;
using Cataloger.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cataloger.Presenters.Bases
{
    public class CatalogerPresenter<I> : BasePresenter<I>, ICatalogerPresenter where I : ICatalogerView
    {
        public CatalogerPresenter(I presenter) 
        : base(presenter) 
        {
        }
    }
}
