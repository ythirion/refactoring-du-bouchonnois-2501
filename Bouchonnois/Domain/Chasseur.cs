namespace Bouchonnois.Domain
{
    public class Chasseur
    {
        public required string Nom { get; init; }
        public int BallesRestantes { get; set; }
        public int NbGalinettes { get; set; }
    }
}