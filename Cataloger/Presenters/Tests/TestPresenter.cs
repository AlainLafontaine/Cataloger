using BaseWinform.Attributes;
using Cataloger.Presenters.Bases;
using Cataloger.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cataloger.Presenters.Tests
{
    [PresenterURL("tests")]
    public class TestPresenter : CatalogerPresenter<ITestView>
    {
        public TestPresenter(
           ITestView view
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
