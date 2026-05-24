#if __INCLUS_THIS_ACTION__
using Cataloger.Core.Entities.Categories.Dto;
using Cataloger.Core.Repositories;
using Zzz.App.Core.Actions;
using Zzz.App.Core.Actions.Http;
using Zzz.App.Core.Entites;
using Zzz.App.Core.Logging;
using Zzz.App.Core.Securite;

namespace Cataloger.Business.Categories
{
    public class GetCategoryRequete : Requete
    {
        public long CategoryId { get; set; }
    }

    public class GetCategoryReponse : Reponse
    {
        [HttpBody]
        public CategoryDto Category { get; set; } = default(CategoryDto)!;
    }

    [GetApi("categories/{categoryid}", "Retourne une Category selon son identifiant")]
    public class GetCategoryAction : SecureActionBase<GetCategoryRequete, GetCategoryReponse>
    {
        private readonly ICategoryRepository categoryRepository;

        public GetCategoryAction(
            ILogger logger,
            IGestionnaireSecurite gs,
            ICategoryRepository categoryRepository
        ) : base(logger, gs)
        {
            this.categoryRepository = categoryRepository;
        }

        public override bool VerifierPermissions(GetCategoryRequete requete)
        {
            return true;
        }

        protected override GetCategoryReponse ExecuterSiAutorise(GetCategoryRequete requete)
        {
            var reponse = this.CreerReponse();

            if (requete.CategoryId == default)
            {
                reponse.AddMsg(new BadRequestHttpActionMessage("CategoryId est obligatoire"));
                return reponse;
            }

            reponse.Category = this.categoryRepository.Obtenir<CategoryDto>(new { CategoryId = requete.CategoryId });

            if (reponse.Category == null)
            {
                reponse.AddMsg(new NotFoundHttpActionMessage("Category non trouvée"));
                return reponse;
            }

            return reponse;
        }
    }
}
#endif // __INCLUS_THIS_ACTION__
