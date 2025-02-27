using Bouchonnois.Service;

namespace Bouchonnois.Domain
{
    public class PartieDeChasse
    {
        public PartieDeChasse()
        {
            Events = [];
        }
        public Guid Id { get; set; }
        public required List<Chasseur> Chasseurs { get; set; }
        public required Terrain Terrain { get; set; }
        public PartieStatus Status { get; set; }
        public List<Event> Events { get; set; }

        public void FaireTirerChasseur(string chasseur)
        {
            var partieDeChasse = this;
            
        }
    }
}