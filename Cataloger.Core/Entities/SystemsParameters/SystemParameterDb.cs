using Zzz.App.Core.Donnees;

namespace Cataloger.Core.Entities.SystemsParameters
{
    [TableBd("CAT_SYSTE_PARAM")]
    public class SystemParameterDb
    {
        [ClePrimaire]
        [ChampBd("SPM_NO_SEQ")]
        [SequenceBd("CAT_SPM_NO_SEQ")]
        public long SystemParameterId {  get; set; }

        [ChampBd("SPM_SECTION")]
        public string Section { get; set; } = string.Empty;

        [ChampBd("SPM_KEY")]
        public string Key { get; set; } = string.Empty;

        [ChampBd("SPM_DESCRIPTION")]
        public string Description { get; set; } = string.Empty;

        [ChampBd("SPM_VAL_STR")]
        public string? ValString { get; set; } = null;

        [ChampBd("SPM_VAL_LONG")]
        public long? ValLong { get; set; } = null;

        [ChampBd("SPM_VAL_DOUBLE")]
        public double? ValDouble { get; set; } = null;

        [ChampBd("SPM_VAL_DATE")]
        public DateTime? ValDate { get; set; } = null;

        [ChampBd("SPM_VAL_BOOL")]
        public bool? ValBooleen { get; set; } = null;

        [ChampBd("SPM_VAL_CHAR")]
        public char? ValChar { get; set; } = null;
    }
}