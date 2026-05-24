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
    public class CreateDirectoryRequete : Requete
    {
        [HttpBody]
        public DirectoryDto Directory { get; set; } = default(DirectoryDto)!;
    }

    public class CreateDirectoryReponse : Reponse
    {
        [HttpBody]
        public DirectoryDto Directory { get; set; } = default(DirectoryDto)!;
    }

    [PostApi("directories", "Crée un enregistrement Directory")]
    public class CreateDirectoryAction : SecureActionBase<CreateDirectoryRequete, CreateDirectoryReponse>
    {
        private readonly IConnexion connexion;
        private readonly IDirectoryRepository directoryRepository;

        public CreateDirectoryAction(
            ILogger logger,
            IGestionnaireSecurite gs,
            IConnexion connexion,
            IDirectoryRepository directoryRepository
        ) : base(logger, gs)
        {
            this.connexion = connexion;
            this.directoryRepository = directoryRepository;
        }

        public override bool VerifierPermissions(CreateDirectoryRequete requete)
        {
            return true;
        }

        protected override CreateDirectoryReponse ExecuterSiAutorise(CreateDirectoryRequete requete)
        {
            var reponse = this.CreerReponse();

            if (requete.Directory == null)
            {
                reponse.AddMsg(new BadRequestHttpActionMessage("Directory est obligatoire"));
                return reponse;
            }

            reponse.AddMsg(this.ValiderEntite(requete.Directory));

            if (reponse.EstEchec)
            {
                return reponse;
            }

            this.directoryRepository.Ajouter(requete.Directory);
            this.connexion.Save();

            reponse.Directory = requete.Directory;

            return reponse;
        }
    }
}
#endif // __INCLUS_THIS_ACTION__
