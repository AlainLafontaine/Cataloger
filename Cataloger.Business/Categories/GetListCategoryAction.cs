#if __INCLUS_THIS_ACTION__
using Cataloger.Core.Entities.Categories.Dto;
using Cataloger.Core.Repositories;
using Zzz.App.Core.Actions;
using Zzz.App.Core.Actions.Http;
using Zzz.App.Core.Logging;
using Zzz.App.Core.Securite;

namespace Cataloger.Business.Categories
{
    public class GetListCategoryRequete : Requete
    {
    }

    public class GetListCategoryReponse : Reponse
    {
        [HttpBody]
        public IEnumerable<CategoryDto> ListCategory { get; set; } = default(IEnumerable<CategoryDto>)!;
    }

    [GetApi("categories", "Retourne l'ensemble des enregistrements Category")]
    public class GetListCategoryAction : SecureActionBase<GetListCategoryRequete, GetListCategoryReponse>
    {
        private readonly ICategoryRepository categoryRepository;

        public GetListCategoryAction(
            ILogger logger,
            IGestionnaireSecurite gs,
            ICategoryRepository categoryRepository
        ) : base(logger, gs)
        {
            this.categoryRepository = categoryRepository;
        }

        public override bool VerifierPermissions(GetListCategoryRequete requete)
        {
            return true;
        }

        protected override GetListCategoryReponse ExecuterSiAutorise(GetListCategoryRequete requete)
        {
            var reponse = this.CreerReponse();
            reponse.ListCategory = this.categoryRepository.ObtenirListe<CategoryDto>();
            return reponse;
        }
    }
}
#endif // __INCLUS_THIS_ACTION__
