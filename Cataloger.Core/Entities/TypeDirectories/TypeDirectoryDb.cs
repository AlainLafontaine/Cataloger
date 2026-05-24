using Zzz.App.Core.Donnees;

namespace Cataloger.Core.Entities.TypeDirectories
{
    [TableBd("CAT_TYPE_DIRECTORY")]
    public class TypeDirectoryDb
    {
        [ClePrimaire]
        [ChampBd("TDR_NO_SEQ")]
        [SequenceBd("CAT_TDR_NO_SEQ")]
        public long TypeDirectoryId { get; set; }

        [ChampBd("TDR_NAME")]
        public string Name { get; set; } = string.Empty;
    }
}
