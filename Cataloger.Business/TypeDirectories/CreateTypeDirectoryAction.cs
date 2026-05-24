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
    public class CreateTypeDirectoryRequete : Requete
    {
        [HttpBody]
        public TypeDirectoryDto TypeDirectory { get; set; } = default(TypeDirectoryDto)!;
    }

    public class CreateTypeDirectoryReponse : Reponse
    {
        [HttpBody]
        public TypeDirectoryDto TypeDirectory { get; set; } = default(TypeDirectoryDto)!;
    }

    [PostApi("type-directories", "Crée un enregistrement TypeDirectory")]
    public class CreateTypeDirectoryAction : SecureActionBase<CreateTypeDirectoryRequete, CreateTypeDirectoryReponse>
    {
        private readonly IConnexion connexion;
        private readonly ITypeDirectoryRepository typeDirectoryRepository;

        public CreateTypeDirectoryAction(
            ILogger logger,
            IGestionnaireSecurite gs,
            IConnexion connexion,
            ITypeDirectoryRepository typeDirectoryRepository
        ) : base(logger, gs)
        {
            this.connexion = connexion;
            this.typeDirectoryRepository = typeDirectoryRepository;
        }

        public override bool VerifierPermissions(CreateTypeDirectoryRequete requete)
        {
            return true;
        }

        protected override CreateTypeDirectoryReponse ExecuterSiAutorise(CreateTypeDirectoryRequete requete)
        {
            var reponse = this.CreerReponse();

            if (requete.TypeDirectory == null)
            {
                reponse.AddMsg(new BadRequestHttpActionMessage("TypeDirectory est obligatoire"));
                return reponse;
            }

            reponse.AddMsg(this.ValiderEntite(requete.TypeDirectory));

            if (reponse.EstEchec)
            {
                return reponse;
            }

            this.typeDirectoryRepository.Ajouter(requete.TypeDirectory);
            this.connexion.Save();

            reponse.TypeDirectory = requete.TypeDirectory;

            return reponse;
        }
    }
}
#endif // __INCLUS_THIS_ACTION__
