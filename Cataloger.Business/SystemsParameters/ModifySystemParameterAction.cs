#if __INCLUS_THIS_ACTION__
using Cataloger.Core.Entities.SystemsParameters.Dto;
using Cataloger.Core.Repositories;
using Zzz.App.Core.Actions;
using Zzz.App.Core.Actions.Http;
using Zzz.App.Core.Donnees;
using Zzz.App.Core.Entites;
using Zzz.App.Core.Logging;
using Zzz.App.Core.Securite;

namespace Cataloger.Business.ParametresSystemes
{
    public class ModifyParametreSystemeRequete : Requete
    {   
        public long SystemParameterId { get; set; }

        [HttpBody]
        public SystemParameterDto SystemParameter { get; set; } = default(SystemParameterDto)!;
    }

    public class ModifyParametreSystemeReponse : Reponse
    {
        public SystemParameterDto SystemParameter { get; set; } = default(SystemParameterDto)!;
    }

    [PutApi("systems-parameters/{systemparameterid}", "Modifie un paramètre système")]
    public class ModifySystemParameterAction : SecureActionBase<ModifyParametreSystemeRequete, ModifyParametreSystemeReponse>
    {
        private readonly IConnexion connexion;
        private readonly ISystemParameterRepository systemParameterRepository;

        public ModifySystemParameterAction(
            ILogger logger,
            IGestionnaireSecurite gs,
            IConnexion connexion,
            ISystemParameterRepository systemParameterRepository
        ) : base(logger, gs)
        {
            this.connexion = connexion;
            this.systemParameterRepository = systemParameterRepository;
        }
        public override bool VerifierPermissions(ModifyParametreSystemeRequete requete)
        {
            return true;
        }

        protected override ModifyParametreSystemeReponse ExecuterSiAutorise(ModifyParametreSystemeRequete requete)
        {
            var reponse = this.CreerReponse();

            if (requete.SystemParameterId == default)
            {
                reponse.AddMsg(new BadRequestHttpActionMessage("ParametreSystemeId est obligatoire"));
            }

            if (requete.SystemParameter == null)
            { 
                reponse.AddMsg(new BadRequestHttpActionMessage("ParametreSysteme est obligatoire"));
                return reponse;
            }

            SystemParameterDto exist = systemParameterRepository.Obtenir<SystemParameterDto>(new  { SystemParameterId = requete.SystemParameterId });

            if (exist == null)
            {
                reponse.AddMsg(new BadRequestHttpActionMessage("ParametreSysteme est obligatoire"));
            }

            reponse.AddMsg(this.ValiderEntite(requete.SystemParameter));

            if (reponse.EstEchec)
            {
                return reponse;
            }

            this.systemParameterRepository.Modifier(requete.SystemParameter);
            this.connexion.Save();

            reponse.SystemParameter = requete.SystemParameter;
            return reponse;
        }
    }
}
#endif // __INCLUS_THIS_ACTION__
