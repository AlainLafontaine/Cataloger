using Zzz.App.Core.Donnees;

namespace Cataloger.Core.Entities.Directories
{
    [TableBd("CAT_DIRECTORY")]
    public class DirectoryDb
    {
        [ClePrimaire]
        [ChampBd("DIR_NO_SEQ")]
        [SequenceBd("CAT_DIR_NO_SEQ")]
        public long DirectoryId { get; set; }

        [ChampBd("DIR_NAME")]
        public string Name { get; set; } = string.Empty;

        [ChampBd("TDR_NO_SEQ")]
        public long TypeDirectoryId { get; set; }

        [ChampBd("CTG_NO_SEQ")]
        public long CategoryId { get; set; }
    }
}
