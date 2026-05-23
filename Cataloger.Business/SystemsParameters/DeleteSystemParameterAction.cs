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
    public class DeleteSystemParameterRequete : Requete
    {
        public string Section { get; set; } = string.Empty;
        public string? Key { get; set; } = null;
    }

    public class DeleteSystemParameterReponse : Reponse
    {
    }

    [DeleteApi("systems-parameters/sections/{section}/keys/{key}", "Supprime un paramètre système selon une section et une clef")]
    [DeleteApi("systems-parameters/sections/{section}", "Supprime l'ensemble des paramètres systèmes selon d'une section")]
    public class DeleteSystemParameterAction : SecureActionBase<DeleteSystemParameterRequete, DeleteSystemParameterReponse>
    {
        private readonly IConnexion connexion;
        private readonly ISystemParameterRepository systemParameterRepository;

        public DeleteSystemParameterAction(
            ILogger logger,
            IGestionnaireSecurite gs,
            IConnexion connexion,
            ISystemParameterRepository systemParameterRepository
        ) : base(logger, gs)
        {
            this.connexion = connexion;
            this.systemParameterRepository = systemParameterRepository;
        }

        public override bool VerifierPermissions(DeleteSystemParameterRequete requete)
        {
            return true;
        }

        protected override DeleteSystemParameterReponse ExecuterSiAutorise(DeleteSystemParameterRequete requete)
        {
            var reponse = this.CreerReponse();

            if (requete.Section == default)
            {
                reponse.AddMsg(new BadRequestHttpActionMessage("Section est obligatoire"));
                return reponse;
            }

            if (requete.Key != null)
            {
                var parametreSysteme = this.systemParameterRepository.Obtenir<SystemParameterDto>(new { Section = requete.Section, Clef = requete.Key });

                if (parametreSysteme == null)
                {
                    reponse.AddMsg(new NotFoundHttpActionMessage("ParametreSysteme non trouvé"));
                    return reponse;
                }

                this.systemParameterRepository.Supprimer(parametreSysteme);
                this.connexion.Save();
            }
            else
            {
                IEnumerable<SystemParameterDto> ListeParametreSysteme = this.systemParameterRepository.ObtenirListe<SystemParameterDto>(new { Section = requete.Section });

                if (ListeParametreSysteme == null)
                {
                    reponse.AddMsg(new NotFoundHttpActionMessage("ParametreSystemes non trouvé"));
                    return reponse;
                }

                this.systemParameterRepository.Supprimer(ListeParametreSysteme);
                this.connexion.Save();
            }

            return reponse;
        }
    }
}
#endif // __INCLUS_THIS_ACTION__