#if __INCLUS_THIS_ACTION__
using Cataloger.Core.Entities.Directories.Dto;
using Cataloger.Core.Repositories;
using Zzz.App.Core.Actions;
using Zzz.App.Core.Actions.Http;
using Zzz.App.Core.Entites;
using Zzz.App.Core.Logging;
using Zzz.App.Core.Securite;

namespace Cataloger.Business.Directories
{
    public class GetDirectoryRequete : Requete
    {
        public long DirectoryId { get; set; }
    }

    public class GetDirectoryReponse : Reponse
    {
        [HttpBody]
        public DirectoryDto Directory { get; set; } = default(DirectoryDto)!;
    }

    [GetApi("directories/{directoryid}", "Retourne un Directory selon son identifiant")]
    public class GetDirectoryAction : SecureActionBase<GetDirectoryRequete, GetDirectoryReponse>
    {
        private readonly IDirectoryRepository directoryRepository;

        public GetDirectoryAction(
            ILogger logger,
            IGestionnaireSecurite gs,
            IDirectoryRepository directoryRepository
        ) : base(logger, gs)
        {
            this.directoryRepository = directoryRepository;
        }

        public override bool VerifierPermissions(GetDirectoryRequete requete)
        {
            return true;
        }

        protected override GetDirectoryReponse ExecuterSiAutorise(GetDirectoryRequete requete)
        {
            var reponse = this.CreerReponse();

            if (requete.DirectoryId == default)
            {
                reponse.AddMsg(new BadRequestHttpActionMessage("DirectoryId est obligatoire"));
                return reponse;
            }

            reponse.Directory = this.directoryRepository.Obtenir<DirectoryDto>(new { DirectoryId = requete.DirectoryId });

            if (reponse.Directory == null)
            {
                reponse.AddMsg(new NotFoundHttpActionMessage("Directory non trouvé"));
                return reponse;
            }

            return reponse;
        }
    }
}
#endif // __INCLUS_THIS_ACTION__
