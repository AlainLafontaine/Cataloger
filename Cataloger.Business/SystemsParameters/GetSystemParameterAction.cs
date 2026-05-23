#if __INCLUS_THIS_ACTION__
using Cataloger.Core.Entities.SystemsParameters.Dto;
using Cataloger.Core.Repositories;
using Zzz.App.Core.Actions;
using Zzz.App.Core.Actions.Http;
using Zzz.App.Core.Entites;
using Zzz.App.Core.Logging;
using Zzz.App.Core.Securite;

namespace Cataloger.Business.ParametresSystemes
{
    public class GetSystemParameterRequete : Requete
    {
        public string Section { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
    }

    public class GetSystemParameterReponse : Reponse
    {        
        [HttpBody]
        public SystemParameterDto SystemParameter { get; set; } = default(SystemParameterDto)!;
    }

    [GetApi("systems-parameters/sections/{section}/keys/{key}", "Retourne un ParametreSysteme selon la section et la clef en paramètre")]
    public class GetSystemParameterAction : SecureActionBase<GetSystemParameterRequete, GetSystemParameterReponse>
    {
        private readonly ISystemParameterRepository systemParameterRepository;

        public GetSystemParameterAction(
            ILogger logger,
            IGestionnaireSecurite gs,
            ISystemParameterRepository systemParameterRepository
        ) : base(logger, gs)
        {
            this.systemParameterRepository = systemParameterRepository;
        }

        public override bool VerifierPermissions(GetSystemParameterRequete requete)
        {
            return true;
        }

        protected override GetSystemParameterReponse ExecuterSiAutorise(GetSystemParameterRequete requete)
        {
            var reponse = this.CreerReponse();

            if (requete.Section == default)
            {
                reponse.AddMsg("Section est obligatoire");
                return reponse;
            }

            if (requete.Key == default)
            {
                reponse.AddMsg("clef est obligatoire");
                return reponse;
            }

            reponse.SystemParameter = this.systemParameterRepository.Obtenir<SystemParameterDto>(new { Section = requete.Section, Key = requete.Key });

            if (reponse.SystemParameter ==  null)
            {
                reponse.AddMsg(new NotFoundHttpActionMessage("ParametreSysteme non trouvé"));
                return reponse;
            }

            return reponse;
        }
    }
}
#endif // __INCLUS_THIS_ACTION__
