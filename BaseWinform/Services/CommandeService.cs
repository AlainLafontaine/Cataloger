using BaseWinform.Interfaces;

namespace BaseWinform.Services
{
    public class CommandeService : WinformService
    {
        private int posCourante = 0;
        private List<ICommande> commandes = new List<ICommande>();

        public bool PermettreDo { get; set; } = false;
        public bool PermettreUndo { get; set; } = false;

        public CommandeService() { }    

        public bool Do(ICommande commande) 
        {
            bool succes = commande.Do();

            if (succes)
            {
                commandes.Add(commande);
                posCourante++;
            }

            return succes; 
        }

        public bool Undo()
        {
            bool succes = commandes[posCourante - 1].Undo();

            if (succes)
            {
                posCourante--;
            }

            return posCourante > 0;
        }
    }
}