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
    public class ModifyTypeDirectoryRequete : Requete
    {
        public long TypeDirectoryId { get; set; }

        [HttpBody]
        public TypeDirectoryDto TypeDirectory { get; set; } = default(TypeDirectoryDto)!;
    }

    public class ModifyTypeDirectoryReponse : Reponse
    {
        public TypeDirectoryDto TypeDirectory { get; set; } = default(TypeDirectoryDto)!;
    }

    [PutApi("type-directories/{typedirectoryid}", "Modifie un TypeDirectory")]
    public class ModifyTypeDirectoryAction : SecureActionBase<ModifyTypeDirectoryRequete, ModifyTypeDirectoryReponse>
    {
        private readonly IConnexion connexion;
        private readonly ITypeDirectoryRepository typeDirectoryRepository;

        public ModifyTypeDirectoryAction(
            ILogger logger,
            IGestionnaireSecurite gs,
            IConnexion connexion,
            ITypeDirectoryRepository typeDirectoryRepository
        ) : base(logger, gs)
        {
            this.connexion = connexion;
            this.typeDirectoryRepository = typeDirectoryRepository;
        }

        public override bool VerifierPermissions(ModifyTypeDirectoryRequete requete)
        {
            return true;
        }

        protected override ModifyTypeDirectoryReponse ExecuterSiAutorise(ModifyTypeDirectoryRequete requete)
        {
            var reponse = this.CreerReponse();

            if (requete.TypeDirectoryId == default)
            {
                reponse.AddMsg(new BadRequestHttpActionMessage("TypeDirectoryId est obligatoire"));
            }

            if (requete.TypeDirectory == null)
            {
                reponse.AddMsg(new BadRequestHttpActionMessage("TypeDirectory est obligatoire"));
                return reponse;
            }

            TypeDirectoryDto exist = typeDirectoryRepository.Obtenir<TypeDirectoryDto>(new { TypeDirectoryId = requete.TypeDirectoryId });

            if (exist == null)
            {
                reponse.AddMsg(new NotFoundHttpActionMessage("TypeDirectory non trouvé"));
            }

            reponse.AddMsg(this.ValiderEntite(requete.TypeDirectory));

            if (reponse.EstEchec)
            {
                return reponse;
            }

            this.typeDirectoryRepository.Modifier(requete.TypeDirectory);
            this.connexion.Save();

            reponse.TypeDirectory = requete.TypeDirectory;
            return reponse;
        }
    }
}
#endif // __INCLUS_THIS_ACTION__
