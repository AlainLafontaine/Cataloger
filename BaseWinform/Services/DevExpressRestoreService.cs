using DevExpress.Utils;
using DevExpress.XtraEditors;
using System.IO;

namespace BaseWinform.Services
{
    public class DevExpressRestoreService : WinformService
    {
        private readonly WorkspaceManager workspaceManager;

        public DevExpressRestoreService() {
            workspaceManager = new WorkspaceManager();
            InitializeWorkspaceManager();
        }

        public string MakeKey(Form form)
        {
            // Définir la form comme cible
            workspaceManager.TargetControl = form;
            return ObtenirKey(form);
        }

        public bool Contains(string key) => workspaceManager.Workspaces.Any(ws => ws.Name == key);

        public void Remove(string key)
        {
            var ws = workspaceManager.Workspaces.FirstOrDefault(w => w.Name.Equals(key, StringComparison.OrdinalIgnoreCase));

            if (ws != null)
            {
                workspaceManager.Workspaces.Remove(ws);
            }
        }

        public void RestoreFormState(Form _, string key) => LoadWorkspace(key);

        public void SaveFormState(Form _, string key) => SaveWorkspace(key);

        private void InitializeWorkspaceManager()
        {
            // Optionnel : configurer le comportement
            workspaceManager.AllowTransitionAnimation = DefaultBoolean.False;

            // Événements optionnels
            workspaceManager.BeforeApplyWorkspace += BeforeApplyWorkspace;
            workspaceManager.AfterApplyWorkspace += AfterApplyWorkspace;
        }

        private void LoadWorkspace(string workspaceId, string? workspaceFile = null)
        {
            try
            {
                if (workspaceFile is not null && File.Exists(workspaceFile))
                {
                    workspaceManager.LoadWorkspace(workspaceId, workspaceFile);
                    workspaceManager.ApplyWorkspace(workspaceId);
                }
                else
                {
                    workspaceManager.ApplyWorkspace(workspaceId);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(
                    $"Erreur lors du chargement: {ex.Message}",
                    "Erreur",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }

        private void SaveWorkspace(string workspaceId, string? workspaceFile = null)
        {
            try
            {
                workspaceManager.CaptureWorkspace(workspaceId);
                if (workspaceFile is not null)
                {
                    workspaceManager.SaveWorkspace(workspaceId, workspaceFile);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(
                    $"Erreur lors de la sauvegarde: {ex.Message}",
                    "Erreur",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private string ObtenirKey(Form form)
        {
            return $"{form.GetType().FullName}:{form.Name}";
        }

        private void BeforeApplyWorkspace(object? sender, EventArgs e)
        {
            // Logique avant restauration si nécessaire
            Console.WriteLine("Restauration de l'état...");
        }

        private void AfterApplyWorkspace(object? sender, EventArgs e)
        {
            // Logique après restauration si nécessaire
            Console.WriteLine("État restauré avec succès");
        }
    }
}
