#if __INCLUS_THIS_ACTION__
using Cataloger.Core.Entities.Categories.Dto;
using Cataloger.Core.Repositories;
using Zzz.App.Core.Actions;
using Zzz.App.Core.Actions.Http;
using Zzz.App.Core.Donnees;
using Zzz.App.Core.Entites;
using Zzz.App.Core.Logging;
using Zzz.App.Core.Securite;

namespace Cataloger.Business.Categories
{
    public class CreateCategoryRequete : Requete
    {
        [HttpBody]
        public CategoryDto Category { get; set; } = default(CategoryDto)!;
    }

    public class CreateCategoryReponse : Reponse
    {
        [HttpBody]
        public CategoryDto Category { get; set; } = default(CategoryDto)!;
    }

    [PostApi("categories", "Crée un enregistrement Category")]
    public class CreateCategoryAction : SecureActionBase<CreateCategoryRequete, CreateCategoryReponse>
    {
        private readonly IConnexion connexion;
        private readonly ICategoryRepository categoryRepository;

        public CreateCategoryAction(
            ILogger logger,
            IGestionnaireSecurite gs,
            IConnexion connexion,
            ICategoryRepository categoryRepository
        ) : base(logger, gs)
        {
            this.connexion = connexion;
            this.categoryRepository = categoryRepository;
        }

        public override bool VerifierPermissions(CreateCategoryRequete requete)
        {
            return true;
        }

        protected override CreateCategoryReponse ExecuterSiAutorise(CreateCategoryRequete requete)
        {
            var reponse = this.CreerReponse();

            if (requete.Category == null)
            {
                reponse.AddMsg(new BadRequestHttpActionMessage("Category est obligatoire"));
                return reponse;
            }

            reponse.AddMsg(this.ValiderEntite(requete.Category));

            if (reponse.EstEchec)
            {
                return reponse;
            }

            this.categoryRepository.Ajouter(requete.Category);
            this.connexion.Save();

            reponse.Category = requete.Category;

            return reponse;
        }
    }
}
#endif // __INCLUS_THIS_ACTION__
