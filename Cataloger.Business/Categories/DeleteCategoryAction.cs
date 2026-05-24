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
    public class DeleteCategoryRequete : Requete
    {
        public long CategoryId { get; set; }
    }

    public class DeleteCategoryReponse : Reponse
    {
    }

    [DeleteApi("categories/{categoryid}", "Supprime une Category selon son identifiant")]
    public class DeleteCategoryAction : SecureActionBase<DeleteCategoryRequete, DeleteCategoryReponse>
    {
        private readonly IConnexion connexion;
        private readonly ICategoryRepository categoryRepository;

        public DeleteCategoryAction(
            ILogger logger,
            IGestionnaireSecurite gs,
            IConnexion connexion,
            ICategoryRepository categoryRepository
        ) : base(logger, gs)
        {
            this.connexion = connexion;
            this.categoryRepository = categoryRepository;
        }

        public override bool VerifierPermissions(DeleteCategoryRequete requete)
        {
            return true;
        }

        protected override DeleteCategoryReponse ExecuterSiAutorise(DeleteCategoryRequete requete)
        {
            var reponse = this.CreerReponse();

            if (requete.CategoryId == default)
            {
                reponse.AddMsg(new BadRequestHttpActionMessage("CategoryId est obligatoire"));
                return reponse;
            }

            var category = this.categoryRepository.Obtenir<CategoryDto>(new { CategoryId = requete.CategoryId });

            if (category == null)
            {
                reponse.AddMsg(new NotFoundHttpActionMessage("Category non trouvée"));
                return reponse;
            }

            this.categoryRepository.Supprimer(category);
            this.connexion.Save();

            return reponse;
        }
    }
}
#endif // __INCLUS_THIS_ACTION__
