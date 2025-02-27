using Bouchonnois.Domain.Exceptions;
using Bouchonnois.Service;
using Bouchonnois.Service.Exceptions;

namespace Bouchonnois.Domain
{
    public class PartieDeChasse
    {
        private readonly Func<DateTime> _timeProvider;
        public PartieDeChasse( )
        {
            Events = [];
            _timeProvider = () => DateTime.Today;
        }
        
        public Guid Id { get; set; }
        public required List<Chasseur> Chasseurs { get; set; }
        public required Terrain Terrain { get; set; }
        public PartieStatus Status { get; set; }
        public List<Event> Events { get; set; }

        public void FaireTirerChasseur(string chasseur)
        {
            var partieDeChasse = this; // super intelligenbt ça d'extraire partieDeChasse
            if (partieDeChasse.Status == PartieStatus.Apéro)
            {
                partieDeChasse.Events.Add(new Event(_timeProvider(),
                    $"{chasseur} veut tirer -> On tire pas pendant l'apéro, c'est sacré !!!"));
                // _repository.Save(partieDeChasse);
                
                throw new OnTirePasPendantLapéroCestSacré();
            }

        }
    }
}