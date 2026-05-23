namespace BaseWinform.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class PresenterURLAttribute : Attribute
    {
        public string PresenterURL { get; }

        public PresenterURLAttribute(string presenterURL)
        {
            PresenterURL = presenterURL;
        }
    }
}
