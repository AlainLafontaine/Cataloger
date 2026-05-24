using Zzz.App.Core.Donnees;

namespace Cataloger.Core.Entities.Categories
{
    [TableBd("CAT_CATEGORY")]
    public class CategoryDb
    {
        [ClePrimaire]
        [ChampBd("CTG_NO_SEQ")]
        [SequenceBd("CAT_CTG_NO_SEQ")]
        public long CategoryId { get; set; }

        [ChampBd("CTG_NAME")]
        public string Name { get; set; } = string.Empty;
    }
}
