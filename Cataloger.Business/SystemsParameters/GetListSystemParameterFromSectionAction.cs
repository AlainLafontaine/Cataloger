#if __INCLUS_THIS_ACTION__
using Cataloger.Core.Entities.SystemsParameters.Dto;
using Cataloger.Core.Repositories;
using Zzz.App.Core.Actions;
using Zzz.App.Core.Actions.Http;
using Zzz.App.Core.Extensions;
using Zzz.App.Core.Logging;
using Zzz.App.Core.Securite;

namespace Cataloger.Business.ParametresSystemes
{
    public class GetListSystemParameterFromSectionRequete : Requete
    {
        public string Section { get; set; } = string.Empty;
    }

    public class GetListSystemParameterFromSectionReponse : Reponse
    {        
        [HttpBody]
        public IEnumerable<SystemParameterDto> ListSystemParameter { get; set; } = default(IEnumerable<SystemParameterDto>)!;
    }

    [GetApi("systems-parameters/sections/{section}", "Retourne l'ensemble des enregistrements d'une section donnée en paramètre")]
    public class GetListSystemParameterFromSectionAction : SecureActionBase<GetListSystemParameterFromSectionRequete, GetListSystemParameterFromSectionReponse>
    {
        private readonly ISystemParameterRepository systemParameterRepository;

        public GetListSystemParameterFromSectionAction(
            ILogger logger,
            IGestionnaireSecurite gs,
            ISystemParameterRepository systemParameterRepository
        ) : base(logger, gs)
        {
            this.systemParameterRepository = systemParameterRepository;
        }

        public override bool VerifierPermissions(GetListSystemParameterFromSectionRequete requete)
        {
            return true;
        }

        protected override GetListSystemParameterFromSectionReponse ExecuterSiAutorise(GetListSystemParameterFromSectionRequete requete)
        {
            var reponse = this.CreerReponse();

            if (requete.Section.IsNullOrEmpty())
            {
                reponse.AddMsg("Section est obligatoire");
                return reponse;
            }

            reponse.ListSystemParameter = this.systemParameterRepository.ObtenirListe<SystemParameterDto>(new { Section = requete.Section});

            return reponse;
        }
    }
}
#endif // __INCLUS_THIS_ACTION__
