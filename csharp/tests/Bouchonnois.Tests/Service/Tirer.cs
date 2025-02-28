using Bouchonnois.Domain.Exceptions;
using Bouchonnois.Service.Exceptions;
using static Bouchonnois.Tests.Service.PartieDeChasseBuilder;
using static Bouchonnois.Tests.Service.ChasseurBuilder;

namespace Bouchonnois.Tests.Service;

public class Tirer : PartieDeChasseTestContext
{
    [Fact]
    public void AvecUnChasseurAyantDesBallesDevraitConsommerUneBalle()
    {
        var id = Guid.NewGuid();
        EtantDonné(
            UnePartieDeChasse(id)
                .EnCours()
                .OuParticipe(UnChasseur().Nommé("Bernard").AvecDesBalles(8))
        );
        
        QuandLeChasseurTire(id, "Bernard");
        
        IlReste(7, "Bernard");
    }

    [Fact]
    public void EchoueCarPartieNexistePas()
    {
        
        EtantDonné(
            UnePartieDeChasse(Guid.NewGuid())
                .EnCours()
                .OuParticipe(UnChasseur().Nommé("Bernard"))
        );
        
        var tirer = () => QuandLeChasseurTire(Guid.NewGuid(), "Bernard");
        
        tirer.Should().Throw<LaPartieDeChasseNexistePas>();
        AucunePartieDeChasseNEstSauvée();
    }

    [Fact]
    public void EchoueAvecUnChasseurNayantPlusDeBalles()
    {
        var id = Guid.NewGuid();

        EtantDonné(
            UnePartieDeChasse(id)
                .EnCours()
                .OuParticipe(UnChasseur().Nommé("Bernard").AvecDesBalles(0))
        );

        var tirer = () => QuandLeChasseurTire(id, "Bernard");

        tirer.Should().Throw<TasPlusDeBallesMonVieuxChasseALaMain>();
    }

    [Fact]
    public void EchoueCarLeChasseurNestPasDansLaPartie()
    {
        var id = Guid.NewGuid();

        EtantDonné(
            UnePartieDeChasse(id)
                .EnCours()
                .OuParticipe(UnChasseur().Nommé("Bernard"))
        );

        var tirer = () => QuandLeChasseurTire(id, "Roger");

        tirer.Should().Throw<ChasseurInconnu>();
    }

    [Fact]
    public void EchoueSiLesChasseursSontEnApero()
    {
        var id = Guid.NewGuid();

        EtantDonné(
            UnePartieDeChasse(id)
                .LorsDeLApéro()
                .OuParticipe(UnChasseur().Nommé("Bernard"))
        );

        var tirer = () => QuandLeChasseurTire(id, "Bernard");

        tirer.Should().Throw<OnTirePasPendantLapéroCestSacré>();
    }

    [Fact]
    public void EchoueSiLaPartieDeChasseEstTerminée()
    {
        var id = Guid.NewGuid();

        EtantDonné(
            UnePartieDeChasse(id)
                .Terminée()
                .OuParticipe(UnChasseur().Nommé("Bernard"))
        );
        
        var tirer = () => QuandLeChasseurTire(id, "Bernard");
        
        tirer.Should().Throw<OnTirePasQuandLaPartieEstTerminée>();
    }
}