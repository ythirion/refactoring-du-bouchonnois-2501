using Bouchonnois.Domain.Exceptions;
using Bouchonnois.Service.Exceptions;

namespace Bouchonnois.Domain
{
    public class Chasseur
    {
        public required string Nom { get; init; }
        public int BallesRestantes { get; set; }
        public int NbGalinettes { get; set; }

        public void Tire()
        {              
            if (BallesRestantes == 0)
            {
                throw new TasPlusDeBallesMonVieuxChasseALaMain();
            }
            BallesRestantes--;
        }
    }
}