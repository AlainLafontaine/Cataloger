namespace BaseWinform.EventsArgs
{
    public class AfficheChildComposanteEventArgs : EventArgs
    {
        public Type ChildComposanteType { get; private set; }

        public AfficheChildComposanteEventArgs(Type childComposanteType)
        {
            ChildComposanteType = childComposanteType;
        }
    }
}
