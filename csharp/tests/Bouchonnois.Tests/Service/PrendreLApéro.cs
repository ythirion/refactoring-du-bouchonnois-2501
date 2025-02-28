using Bouchonnois.Service.Exceptions;
using static Bouchonnois.Tests.Service.ChasseurBuilder;
using static Bouchonnois.Tests.Service.PartieDeChasseBuilder;

namespace Bouchonnois.Tests.Service;

public class PrendreLApéro : PartieDeChasseTestContext
{
    [Fact]
    public void QuandLaPartieEstEnCours()
    {
        var id = Guid.NewGuid();
        EtantDonné(
            UnePartieDeChasse(id)
            .EnCours()
            .OuParticipent(UnChasseur(), UnChasseur(), UnChasseur())
        );

        QuandOnPrendLapéro(id);

        CEstLapéro(id);
    }


    [Fact]
    public void EchoueCarPartieNexistePas()
    {
        var id = Guid.NewGuid();
        var prendreLapéro = () => QuandOnPrendLapéro(id);

        prendreLapéro.Should().Throw<LaPartieDeChasseNexistePas>();
        AucunePartieDeChasseNEstSauvée();
    }

    [Fact]
    public void EchoueSiLesChasseursSontDéjaEnApero()
    {
        var id = Guid.NewGuid();
        EtantDonné(
            UnePartieDeChasse(id)
            .LorsDeLApéro()
            .OuParticipent(UnChasseur(), UnChasseur(), UnChasseur()));


        var prendreLapéroPendantLapéro = () => QuandOnPrendLapéro(id);
        prendreLapéroPendantLapéro.Should().Throw<OnEstDéjàEnTrainDePrendreLapéro>();
        AucunePartieDeChasseNEstSauvée();
    }

    [Fact]
    public void EchoueSiLaPartieDeChasseEstTerminée()
    {
        var id = Guid.NewGuid();
        EtantDonné(
            UnePartieDeChasse(id)
            .Terminée()
            .OuParticipent(UnChasseur(), UnChasseur(), UnChasseur()));
        
        var prendreLapéroPendantLapéro = () => QuandOnPrendLapéro(id);
        prendreLapéroPendantLapéro.Should().Throw<OnPrendPasLapéroQuandLaPartieEstTerminée>();
        AucunePartieDeChasseNEstSauvée();
    }
}