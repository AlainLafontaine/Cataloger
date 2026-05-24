#if __INCLUS_THIS_ACTION__
using Cataloger.Core.Entities.TypeDirectories.Dto;
using Cataloger.Core.Repositories;
using Zzz.App.Core.Actions;
using Zzz.App.Core.Actions.Http;
using Zzz.App.Core.Entites;
using Zzz.App.Core.Logging;
using Zzz.App.Core.Securite;

namespace Cataloger.Business.TypeDirectories
{
    public class GetTypeDirectoryRequete : Requete
    {
        public long TypeDirectoryId { get; set; }
    }

    public class GetTypeDirectoryReponse : Reponse
    {
        [HttpBody]
        public TypeDirectoryDto TypeDirectory { get; set; } = default(TypeDirectoryDto)!;
    }

    [GetApi("type-directories/{typedirectoryid}", "Retourne un TypeDirectory selon son identifiant")]
    public class GetTypeDirectoryAction : SecureActionBase<GetTypeDirectoryRequete, GetTypeDirectoryReponse>
    {
        private readonly ITypeDirectoryRepository typeDirectoryRepository;

        public GetTypeDirectoryAction(
            ILogger logger,
            IGestionnaireSecurite gs,
            ITypeDirectoryRepository typeDirectoryRepository
        ) : base(logger, gs)
        {
            this.typeDirectoryRepository = typeDirectoryRepository;
        }

        public override bool VerifierPermissions(GetTypeDirectoryRequete requete)
        {
            return true;
        }

        protected override GetTypeDirectoryReponse ExecuterSiAutorise(GetTypeDirectoryRequete requete)
        {
            var reponse = this.CreerReponse();

            if (requete.TypeDirectoryId == default)
            {
                reponse.AddMsg(new BadRequestHttpActionMessage("TypeDirectoryId est obligatoire"));
                return reponse;
            }

            reponse.TypeDirectory = this.typeDirectoryRepository.Obtenir<TypeDirectoryDto>(new { TypeDirectoryId = requete.TypeDirectoryId });

            if (reponse.TypeDirectory == null)
            {
                reponse.AddMsg(new NotFoundHttpActionMessage("TypeDirectory non trouvé"));
                return reponse;
            }

            return reponse;
        }
    }
}
#endif // __INCLUS_THIS_ACTION__
