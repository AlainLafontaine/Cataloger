#if __INCLUS_THIS_ACTION__
using Cataloger.Core.Entities.Directories.Dto;
using Cataloger.Core.Repositories;
using Zzz.App.Core.Actions;
using Zzz.App.Core.Actions.Http;
using Zzz.App.Core.Donnees;
using Zzz.App.Core.Entites;
using Zzz.App.Core.Logging;
using Zzz.App.Core.Securite;

namespace Cataloger.Business.Directories
{
    public class DeleteDirectoryRequete : Requete
    {
        public long DirectoryId { get; set; }
    }

    public class DeleteDirectoryReponse : Reponse
    {
    }

    [DeleteApi("directories/{directoryid}", "Supprime un Directory selon son identifiant")]
    public class DeleteDirectoryAction : SecureActionBase<DeleteDirectoryRequete, DeleteDirectoryReponse>
    {
        private readonly IConnexion connexion;
        private readonly IDirectoryRepository directoryRepository;

        public DeleteDirectoryAction(
            ILogger logger,
            IGestionnaireSecurite gs,
            IConnexion connexion,
            IDirectoryRepository directoryRepository
        ) : base(logger, gs)
        {
            this.connexion = connexion;
            this.directoryRepository = directoryRepository;
        }

        public override bool VerifierPermissions(DeleteDirectoryRequete requete)
        {
            return true;
        }

        protected override DeleteDirectoryReponse ExecuterSiAutorise(DeleteDirectoryRequete requete)
        {
            var reponse = this.CreerReponse();

            if (requete.DirectoryId == default)
            {
                reponse.AddMsg(new BadRequestHttpActionMessage("DirectoryId est obligatoire"));
                return reponse;
            }

            var directory = this.directoryRepository.Obtenir<DirectoryDto>(new { DirectoryId = requete.DirectoryId });

            if (directory == null)
            {
                reponse.AddMsg(new NotFoundHttpActionMessage("Directory non trouvé"));
                return reponse;
            }

            this.directoryRepository.Supprimer(directory);
            this.connexion.Save();

            return reponse;
        }
    }
}
#endif // __INCLUS_THIS_ACTION__
