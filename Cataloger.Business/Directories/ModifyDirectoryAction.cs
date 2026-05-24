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
    public class ModifyDirectoryRequete : Requete
    {
        public long DirectoryId { get; set; }

        [HttpBody]
        public DirectoryDto Directory { get; set; } = default(DirectoryDto)!;
    }

    public class ModifyDirectoryReponse : Reponse
    {
        public DirectoryDto Directory { get; set; } = default(DirectoryDto)!;
    }

    [PutApi("directories/{directoryid}", "Modifie un Directory")]
    public class ModifyDirectoryAction : SecureActionBase<ModifyDirectoryRequete, ModifyDirectoryReponse>
    {
        private readonly IConnexion connexion;
        private readonly IDirectoryRepository directoryRepository;

        public ModifyDirectoryAction(
            ILogger logger,
            IGestionnaireSecurite gs,
            IConnexion connexion,
            IDirectoryRepository directoryRepository
        ) : base(logger, gs)
        {
            this.connexion = connexion;
            this.directoryRepository = directoryRepository;
        }

        public override bool VerifierPermissions(ModifyDirectoryRequete requete)
        {
            return true;
        }

        protected override ModifyDirectoryReponse ExecuterSiAutorise(ModifyDirectoryRequete requete)
        {
            var reponse = this.CreerReponse();

            if (requete.DirectoryId == default)
            {
                reponse.AddMsg(new BadRequestHttpActionMessage("DirectoryId est obligatoire"));
            }

            if (requete.Directory == null)
            {
                reponse.AddMsg(new BadRequestHttpActionMessage("Directory est obligatoire"));
                return reponse;
            }

            DirectoryDto exist = directoryRepository.Obtenir<DirectoryDto>(new { DirectoryId = requete.DirectoryId });

            if (exist == null)
            {
                reponse.AddMsg(new NotFoundHttpActionMessage("Directory non trouvé"));
            }

            reponse.AddMsg(this.ValiderEntite(requete.Directory));

            if (reponse.EstEchec)
            {
                return reponse;
            }

            this.directoryRepository.Modifier(requete.Directory);
            this.connexion.Save();

            reponse.Directory = requete.Directory;
            return reponse;
        }
    }
}
#endif // __INCLUS_THIS_ACTION__
