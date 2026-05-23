namespace BaseWinform.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class WinformURLAttribute : Attribute
    {
        public string WinformURL { get; }

        public WinformURLAttribute(string winformURL) {
            WinformURL = winformURL;
        }
    }
}
