using Bouchonnois.Service;
using Bouchonnois.Service.Exceptions;
using Bouchonnois.Tests.Doubles;
using FluentAssertions.Extensions;
using static Bouchonnois.Tests.Service.ChasseurBuilder;
using static Bouchonnois.Tests.Service.PartieDeChasseBuilder;

namespace Bouchonnois.Tests.Service;

public class ScenarioTests : PartieDeChasseTestContext
{
    [Fact]
    public Task DéroulerUnePartie()
    {
        var Dédé = "Dédé";
        var Bernard = "Bernard";
        var Robert = "Robert";
        EtantDonnéUnePartieDémarée(
            UnePartieDeChasse()
                .Sur(new TerrainBuilder("Pitibon sur Sauldre",4))
                .OuParticipent(
                    UnChasseur().Nommé(Dédé).AvecDesBalles(20),
                    UnChasseur().Nommé(Bernard).AvecDesBalles(8),
                    UnChasseur().Nommé(Robert).AvecDesBalles(12)
                )
            
        ).A(new DateTime(2024, 4, 25, 9, 0, 0))
        .Apres(10.Minutes())
        .LeChasseurTire(Dédé)
        .Apres(30.Minutes())
        .LeChasseurTireSurUneGalinette(Robert)
        .Apres(20.Minutes())
        .LesChasseursPrennentLapéro()
        .Apres(1.Hours())
        .LesChasseursReprennentLaPartie()
        .Apres(2.Minutes())
        .LeChasseurTire(Bernard)
        .Apres(1.Minutes())
        .LeChasseurTire(Bernard)
        .Apres(26.Minutes())
        .LeChasseurTireSurUneGalinette(Robert)
        .Apres(10.Minutes())
        .LesChasseursPrennentLapéro()
        .Apres(170.Minutes())
        .LesChasseursReprennentLaPartie()
        .Apres(11.Minutes())
        .LeChasseurTire(Bernard)
        
        

        time = time.Add(TimeSpan.FromSeconds(1));
        service.Tirer(id, Bernard);

        time = time.Add(TimeSpan.FromSeconds(1));
        service.Tirer(id, Bernard);

        time = time.Add(TimeSpan.FromSeconds(1));
        service.Tirer(id, Bernard);

        time = time.Add(TimeSpan.FromSeconds(1));
        service.Tirer(id, Bernard);

        time = time.Add(TimeSpan.FromSeconds(1));
        service.Tirer(id, Bernard);

        time = time.Add(TimeSpan.FromSeconds(1));

        try
        {
            service.Tirer(id, Bernard);
        }
        catch
        {
            // ignored : because why not
        }

        time = time.Add(TimeSpan.FromMinutes(19));
        service.TirerSurUneGalinette(id, Robert);

        time = time.Add(TimeSpan.FromMinutes(30));
        service.TerminerLaPartie(id);
        return Verify(service.ConsulterStatus(id));
    }
}