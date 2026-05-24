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
    public class ModifyCategoryRequete : Requete
    {
        public long CategoryId { get; set; }

        [HttpBody]
        public CategoryDto Category { get; set; } = default(CategoryDto)!;
    }

    public class ModifyCategoryReponse : Reponse
    {
        public CategoryDto Category { get; set; } = default(CategoryDto)!;
    }

    [PutApi("categories/{categoryid}", "Modifie une Category")]
    public class ModifyCategoryAction : SecureActionBase<ModifyCategoryRequete, ModifyCategoryReponse>
    {
        private readonly IConnexion connexion;
        private readonly ICategoryRepository categoryRepository;

        public ModifyCategoryAction(
            ILogger logger,
            IGestionnaireSecurite gs,
            IConnexion connexion,
            ICategoryRepository categoryRepository
        ) : base(logger, gs)
        {
            this.connexion = connexion;
            this.categoryRepository = categoryRepository;
        }

        public override bool VerifierPermissions(ModifyCategoryRequete requete)
        {
            return true;
        }

        protected override ModifyCategoryReponse ExecuterSiAutorise(ModifyCategoryRequete requete)
        {
            var reponse = this.CreerReponse();

            if (requete.CategoryId == default)
            {
                reponse.AddMsg(new BadRequestHttpActionMessage("CategoryId est obligatoire"));
            }

            if (requete.Category == null)
            {
                reponse.AddMsg(new BadRequestHttpActionMessage("Category est obligatoire"));
                return reponse;
            }

            CategoryDto exist = categoryRepository.Obtenir<CategoryDto>(new { CategoryId = requete.CategoryId });

            if (exist == null)
            {
                reponse.AddMsg(new NotFoundHttpActionMessage("Category non trouvée"));
            }

            reponse.AddMsg(this.ValiderEntite(requete.Category));

            if (reponse.EstEchec)
            {
                return reponse;
            }

            this.categoryRepository.Modifier(requete.Category);
            this.connexion.Save();

            reponse.Category = requete.Category;
            return reponse;
        }
    }
}
#endif // __INCLUS_THIS_ACTION__
