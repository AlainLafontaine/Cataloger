using BaseWinform.Forms;

namespace BaseWinform.Services
{
    public class MyAppContext : ApplicationContext
    {
        private BaseForm? currentForm = null;
        private BaseForm? formAFermer = null;

        private readonly System.Windows.Forms.Timer demandeFermetureFormPrecedente;

        public MyAppContext() : base()
        {
            demandeFermetureFormPrecedente = new() { Interval = 0_100 };
            demandeFermetureFormPrecedente.Tick += (s, e) => FermetureForm();
        }

        public void ShowForm(BaseForm formToShow)
        {
            Size? size = null;
            Point? point = null;

            if (currentForm != null)
            {
                size = currentForm.Size;
                point = currentForm.Location;
                formAFermer = currentForm;
            }

            currentForm = formToShow;
            currentForm.Show();

            formToShow.FormClosed += (s, e) =>
            {
                if (Application.OpenForms.Count == 0)
                    ExitThread(); // Quitte si plus aucun form ouvert
            };

            if (size != null && point != null)
            {
                currentForm.Location = (Point)point;
                currentForm.Size =(Size)size;
            }

            if (formAFermer != null)
            {
                formAFermer.Activate();
                demandeFermetureFormPrecedente.Start();
            }
        }

        private void FermetureForm()
        {
            demandeFermetureFormPrecedente.Stop();
            formAFermer?.Close();   // Fermer le formulaire précédent
        }
    }
}
