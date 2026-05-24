#if __INCLUS_THIS_ACTION__
using Cataloger.Core.Entities.Directories.Dto;
using Cataloger.Core.Repositories;
using Zzz.App.Core.Actions;
using Zzz.App.Core.Actions.Http;
using Zzz.App.Core.Logging;
using Zzz.App.Core.Securite;

namespace Cataloger.Business.Directories
{
    public class GetListDirectoryRequete : Requete
    {
    }

    public class GetListDirectoryReponse : Reponse
    {
        [HttpBody]
        public IEnumerable<DirectoryDto> ListDirectory { get; set; } = default(IEnumerable<DirectoryDto>)!;
    }

    [GetApi("directories", "Retourne l'ensemble des enregistrements Directory")]
    public class GetListDirectoryAction : SecureActionBase<GetListDirectoryRequete, GetListDirectoryReponse>
    {
        private readonly IDirectoryRepository directoryRepository;

        public GetListDirectoryAction(
            ILogger logger,
            IGestionnaireSecurite gs,
            IDirectoryRepository directoryRepository
        ) : base(logger, gs)
        {
            this.directoryRepository = directoryRepository;
        }

        public override bool VerifierPermissions(GetListDirectoryRequete requete)
        {
            return true;
        }

        protected override GetListDirectoryReponse ExecuterSiAutorise(GetListDirectoryRequete requete)
        {
            var reponse = this.CreerReponse();
            reponse.ListDirectory = this.directoryRepository.ObtenirListe<DirectoryDto>();
            return reponse;
        }
    }
}
#endif // __INCLUS_THIS_ACTION__
