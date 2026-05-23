#if __INCLUS_THIS_ACTION__
using Cataloger.Core.Entities.SystemsParameters.Dto;
using Cataloger.Core.Repositories;
using Zzz.App.Core.Actions;
using Zzz.App.Core.Actions.Http;
using Zzz.App.Core.Logging;
using Zzz.App.Core.Securite;

namespace Cataloger.Business.ParametresSystemes
{
    public class GetListSystemParameterRequete : Requete
    {
    }

    public class GetListSystemParameterReponse : Reponse
    {        
        [HttpBody]
        public IEnumerable<SystemParameterDto> ListSystemParameter { get; set; } = default(IEnumerable<SystemParameterDto>)!;
    }

    [GetApi("systems-parameters", "Retourne l'ensemble des enregistrements ParametreSysteme")]
    public class GetListSystemParameterAction : SecureActionBase<GetListSystemParameterRequete, GetListSystemParameterReponse>
    {
        private readonly ISystemParameterRepository systemParameterRepository;

        public GetListSystemParameterAction(
            ILogger logger,
            IGestionnaireSecurite gs,
            ISystemParameterRepository systemParameterRepository
        ) : base(logger, gs)
        {
            this.systemParameterRepository = systemParameterRepository;
        }

        public override bool VerifierPermissions(GetListSystemParameterRequete requete)
        {
            return true;
        }

        protected override GetListSystemParameterReponse ExecuterSiAutorise(GetListSystemParameterRequete requete)
        {
            var reponse = this.CreerReponse();
            reponse.ListSystemParameter = this.systemParameterRepository.ObtenirListe<SystemParameterDto>();
            return reponse;
        }
    }
}
#endif // __INCLUS_THIS_ACTION__
