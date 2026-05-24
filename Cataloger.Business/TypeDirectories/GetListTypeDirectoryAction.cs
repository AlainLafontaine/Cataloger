#if __INCLUS_THIS_ACTION__
using Cataloger.Core.Entities.TypeDirectories.Dto;
using Cataloger.Core.Repositories;
using Zzz.App.Core.Actions;
using Zzz.App.Core.Actions.Http;
using Zzz.App.Core.Logging;
using Zzz.App.Core.Securite;

namespace Cataloger.Business.TypeDirectories
{
    public class GetListTypeDirectoryRequete : Requete
    {
    }

    public class GetListTypeDirectoryReponse : Reponse
    {
        [HttpBody]
        public IEnumerable<TypeDirectoryDto> ListTypeDirectory { get; set; } = default(IEnumerable<TypeDirectoryDto>)!;
    }

    [GetApi("type-directories", "Retourne l'ensemble des enregistrements TypeDirectory")]
    public class GetListTypeDirectoryAction : SecureActionBase<GetListTypeDirectoryRequete, GetListTypeDirectoryReponse>
    {
        private readonly ITypeDirectoryRepository typeDirectoryRepository;

        public GetListTypeDirectoryAction(
            ILogger logger,
            IGestionnaireSecurite gs,
            ITypeDirectoryRepository typeDirectoryRepository
        ) : base(logger, gs)
        {
            this.typeDirectoryRepository = typeDirectoryRepository;
        }

        public override bool VerifierPermissions(GetListTypeDirectoryRequete requete)
        {
            return true;
        }

        protected override GetListTypeDirectoryReponse ExecuterSiAutorise(GetListTypeDirectoryRequete requete)
        {
            var reponse = this.CreerReponse();
            reponse.ListTypeDirectory = this.typeDirectoryRepository.ObtenirListe<TypeDirectoryDto>();
            return reponse;
        }
    }
}
#endif // __INCLUS_THIS_ACTION__
