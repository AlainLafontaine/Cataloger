#if __INCLUS_THIS_ACTION__
using Cataloger.Core.Entities.TypeDirectories.Dto;
using Cataloger.Core.Repositories;
using Zzz.App.Core.Actions;
using Zzz.App.Core.Actions.Http;
using Zzz.App.Core.Donnees;
using Zzz.App.Core.Entites;
using Zzz.App.Core.Logging;
using Zzz.App.Core.Securite;

namespace Cataloger.Business.TypeDirectories
{
    public class DeleteTypeDirectoryRequete : Requete
    {
        public long TypeDirectoryId { get; set; }
    }

    public class DeleteTypeDirectoryReponse : Reponse
    {
    }

    [DeleteApi("type-directories/{typedirectoryid}", "Supprime un TypeDirectory selon son identifiant")]
    public class DeleteTypeDirectoryAction : SecureActionBase<DeleteTypeDirectoryRequete, DeleteTypeDirectoryReponse>
    {
        private readonly IConnexion connexion;
        private readonly ITypeDirectoryRepository typeDirectoryRepository;

        public DeleteTypeDirectoryAction(
            ILogger logger,
            IGestionnaireSecurite gs,
            IConnexion connexion,
            ITypeDirectoryRepository typeDirectoryRepository
        ) : base(logger, gs)
        {
            this.connexion = connexion;
            this.typeDirectoryRepository = typeDirectoryRepository;
        }

        public override bool VerifierPermissions(DeleteTypeDirectoryRequete requete)
        {
            return true;
        }

        protected override DeleteTypeDirectoryReponse ExecuterSiAutorise(DeleteTypeDirectoryRequete requete)
        {
            var reponse = this.CreerReponse();

            if (requete.TypeDirectoryId == default)
            {
                reponse.AddMsg(new BadRequestHttpActionMessage("TypeDirectoryId est obligatoire"));
                return reponse;
            }

            var typeDirectory = this.typeDirectoryRepository.Obtenir<TypeDirectoryDto>(new { TypeDirectoryId = requete.TypeDirectoryId });

            if (typeDirectory == null)
            {
                reponse.AddMsg(new NotFoundHttpActionMessage("TypeDirectory non trouvé"));
                return reponse;
            }

            this.typeDirectoryRepository.Supprimer(typeDirectory);
            this.connexion.Save();

            return reponse;
        }
    }
}
#endif // __INCLUS_THIS_ACTION__
