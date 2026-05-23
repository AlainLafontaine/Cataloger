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
    public class CreateSystemParameterRequete : Requete
    {
        [HttpBody]
        public SystemParameterDto SystemParameter { get; set; } = default(SystemParameterDto)!;
    }

    public class CreateSystemParameterReponse : Reponse
    {
        [HttpBody]
        public SystemParameterDto SystemParameter { get; set; } = default(SystemParameterDto)!;
    }

    [PostApi("systems-parameters", "Crée un enregistrement SystemParameter")]
    public class CreateSystemParameterAction : SecureActionBase<CreateSystemParameterRequete, CreateSystemParameterReponse>
    {
        private readonly IConnexion connexion;
        private readonly ISystemParameterRepository systemParameterRepository;

        public CreateSystemParameterAction(
            ILogger logger,
            IGestionnaireSecurite gs,
            IConnexion connexion,
            ISystemParameterRepository systemParameterRepository
        ) : base(logger, gs)
        {
            this.connexion = connexion;
            this.systemParameterRepository = systemParameterRepository;
        }

        public override bool VerifierPermissions(CreateSystemParameterRequete requete)
        {
            return true;
        }

        protected override CreateSystemParameterReponse ExecuterSiAutorise(CreateSystemParameterRequete requete)
        {
            var reponse = this.CreerReponse();

            if (requete.SystemParameter == null)
            {
                reponse.AddMsg(new BadRequestHttpActionMessage("SystemParameter est obligatoire"));
                return reponse;
            }

            reponse.AddMsg(this.ValiderEntite(requete.SystemParameter));

            if (reponse.EstEchec)
            {
                return reponse;
            }

            this.systemParameterRepository.Ajouter(requete.SystemParameter);
            this.connexion.Save();

            reponse.SystemParameter = requete.SystemParameter;

            return reponse;
        }
    }
}
#endif // __INCLUS_THIS_ACTION__
