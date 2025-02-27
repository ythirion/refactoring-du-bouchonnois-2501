using FluentAssertions.Extensions;
using static Bouchonnois.Tests.Service.ChasseurBuilder;
using static Bouchonnois.Tests.Service.PartieDeChasseBuilder;

namespace Bouchonnois.Tests.Service;

public class ScenarioTests : PartieDeChasseTestContext
{
    public ScenarioTests()
    {
        _time = new DateTime(2024, 4, 25, 9, 0, 0);
    }
    
    [Fact]
    public Task DéroulerUnePartie()
    {
        var Dédé = "Dédé";
        var Bernard = "Bernard";
        var Robert = "Robert";
        PartieDeChasseTestContext context = new PartieDeChasseTestContext().EtantDonnéUnePartieDémarée(
                                   UnePartieDeChasse()
                                       .Sur(new TerrainBuilder("Pitibon sur Sauldre", 4))
                                       .OuParticipent(
                                                      UnChasseur().Nommé(Dédé).AvecDesBalles(20),
                                                      UnChasseur().Nommé(Bernard).AvecDesBalles(8),
                                                      UnChasseur().Nommé(Robert).AvecDesBalles(12)
                                                     )

                                  )
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
            .Apres(1.Minutes())
            .LeChasseurTireSurUneGalinette(Dédé)
            .Apres(26.Minutes())
            .LeChasseurTireSurUneGalinette(Robert)
            .Apres(10.Minutes())
            .LesChasseursPrennentLapéro()
            .Apres(170.Minutes())
            .LesChasseursReprennentLaPartie()
            .Apres(11.Minutes())
            .LeChasseurTire(Bernard)
            .Apres(1.Seconds())
            .LeChasseurTire(Bernard)
            .Apres(1.Seconds())
            .LeChasseurTire(Bernard)
            .Apres(1.Seconds())
            .LeChasseurTire(Bernard)
            .Apres(1.Seconds())
            .LeChasseurTire(Bernard)
            .Apres(1.Seconds())
            .LeChasseurTire(Bernard)
            .Apres(1.Seconds())
            .LeChasseurTire(Bernard)
            .Apres(19.Minutes())
            .LeChasseurTireSurUneGalinette(Robert)
            .Apres(30.Minutes())
            .TerminerLaPartie();
        
        return Verify(context.ConsulterStatus());
    }
}