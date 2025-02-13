using FluentAssertions.Extensions;
using static Bouchonnois.Tests.Service.ChasseurBuilder;
using static Bouchonnois.Tests.Service.PartieDeChasseBuilder;

namespace Bouchonnois.Tests.Service;

public class ScenarioTests
{
    [Fact]
    public Task DéroulerUnePartie()
    {
        var Dédé = "Dédé";
        var Bernard = "Bernard";
        var Robert = "Robert";
        PartieDeChasseTestContext.EtantDonnéUnePartieDémarée(
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
            .Apres(1.Seconds());

        try
        {
            LeChasseurTire(Bernard);
        }
        catch
        {
            // ignored : because why not
        }
        
        Apres(19.Minutes())
            .LeChasseurTireSurUneGalinette(Robert)
            .Apres(30.Minutes())
            .TerminerLaPartie();

        return Verify(ConsulterStatus());
        
        // var time = new DateTime(2024, 4, 25, 9, 0, 0);
        // var repository = new PartieDeChasseRepositoryForTests();
        // var service = new PartieDeChasseService(repository, () => time);
        // var chasseurs = new List<(string, int)>
        // {
        //     ("Dédé", 20),
        //     ("Bernard", 8),
        //     ("Robert", 12)
        // };
        // var terrainDeChasse = ("Pitibon sur Sauldre", 4);
        // var id = service.Demarrer(
        //     terrainDeChasse,
        //     chasseurs
        // );
        //
        // time = time.Add(TimeSpan.FromMinutes(10));
        // service.Tirer(id, "Dédé");
        //
        // time = time.Add(TimeSpan.FromMinutes(30));
        // service.TirerSurUneGalinette(id, "Robert");
        //
        // time = time.Add(TimeSpan.FromMinutes(20));
        // service.PrendreLapéro(id);
        //
        // time = time.Add(TimeSpan.FromHours(1));
        // service.ReprendreLaPartie(id);
        //
        // time = time.Add(TimeSpan.FromMinutes(2));
        // service.Tirer(id, "Bernard");
        //
        // time = time.Add(TimeSpan.FromMinutes(1));
        // service.Tirer(id, "Bernard");
        //
        // time = time.Add(TimeSpan.FromMinutes(1));
        // service.TirerSurUneGalinette(id, "Dédé");
        //
        // time = time.Add(TimeSpan.FromMinutes(26));
        // service.TirerSurUneGalinette(id, "Robert");
        //
        // time = time.Add(TimeSpan.FromMinutes(10));
        // service.PrendreLapéro(id);
        //
        // time = time.Add(TimeSpan.FromMinutes(170));
        // service.ReprendreLaPartie(id);
        //
        // time = time.Add(TimeSpan.FromMinutes(11));
        // service.Tirer(id, "Bernard");
        //
        // time = time.Add(TimeSpan.FromSeconds(1));
        // service.Tirer(id, "Bernard");
        //
        // time = time.Add(TimeSpan.FromSeconds(1));
        // service.Tirer(id, "Bernard");
        //
        // time = time.Add(TimeSpan.FromSeconds(1));
        // service.Tirer(id, "Bernard");
        //
        // time = time.Add(TimeSpan.FromSeconds(1));
        // service.Tirer(id, "Bernard");
        //
        // time = time.Add(TimeSpan.FromSeconds(1));
        // service.Tirer(id, "Bernard");
        //
        // time = time.Add(TimeSpan.FromSeconds(1));
        //
        // try
        // {
        //     service.Tirer(id, "Bernard");
        // }
        // catch
        // {
        //     // ignored : because why not
        // }
        //
        // time = time.Add(TimeSpan.FromMinutes(19));
        // service.TirerSurUneGalinette(id, "Robert");
        //
        // time = time.Add(TimeSpan.FromMinutes(30));
        // service.TerminerLaPartie(id);
        //return Verify(service.ConsulterStatus(id));
    }
}