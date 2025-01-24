using Bouchonnois.Domain;
using Bouchonnois.Service;
using Bouchonnois.Service.Exceptions;
using Bouchonnois.Tests.Doubles;

namespace Bouchonnois.Tests.Service;

public class Tirer
{
    [Fact]
    public void AvecUnChasseurAyantDesBalles()
    {
        var id = Guid.NewGuid();
        var repository = new PartieDeChasseRepositoryForTests();

        var dédé = UnChasseur("Dédé") with {BallesRestantes = 20};
        var bernard = UnChasseur("Bernard") with { BallesRestantes = 8 };
        var robert = UnChasseur("Robert") with { BallesRestantes = 12 };
        
        repository.Add(new PartieDeChasse
        {
            Id = id,
            Chasseurs = new List<Chasseur>
            {
                dédé.Build(),
                bernard.Build(),
                robert.Build()
            },
            Terrain = new Terrain
            {
                Nom = "Pitibon sur Sauldre",
                NbGalinettes = 3
            },
            Status = PartieStatus.EnCours,
            Events = new List<Event>()
        });

        var service = new PartieDeChasseService(repository, () => DateTime.Now);

        service.Tirer(id, "Bernard");

        var savedPartieDeChasse = repository.SavedPartieDeChasse();
        savedPartieDeChasse.Id.Should().Be(id);
        savedPartieDeChasse.Status.Should().Be(PartieStatus.EnCours);
        savedPartieDeChasse.Terrain.Should().BeEquivalentTo(new Terrain()
        {
            Nom = "Pitibon sur Sauldre",
            NbGalinettes = 3
        });
        savedPartieDeChasse.Chasseurs[1].BallesRestantes.Should().Be(7);

        var bernardAprèsLeTir = bernard with { BallesRestantes = 7 };
        savedPartieDeChasse.Chasseurs.Should().BeEquivalentTo(new Chasseur[]
        {
            dédé.Build(),
            bernardAprèsLeTir.Build(),
            robert.Build()            
        });
    }

    private static ChasseurBuilder UnChasseur(string nom)
    {
        return new ChasseurBuilder(nom, 0);
    }

    [Fact]
    public void AvecUnChasseurAyantDesBallesDevraitConsommerUneBalle()
    {
        // GIVEN
        var id = Guid.NewGuid();
        var repository = new PartieDeChasseRepositoryForTests();
        
        repository.Add(new PartieDeChasse
        {
            Id = id,
            Chasseurs = new List<Chasseur>
            {
                new() { Nom = "Bernard", BallesRestantes = 8 },
            },
            Terrain = new Terrain
            {
                Nom = "Pitibon sur Sauldre",
                NbGalinettes = 3
            },
            Status = PartieStatus.EnCours,
            Events = new List<Event>()
        });

        var service = new PartieDeChasseService(repository, () => DateTime.Now);

        // WHEN
        service.Tirer(id, "Bernard");

        // THEN
        var savedPartieDeChasse = repository.SavedPartieDeChasse();
        savedPartieDeChasse.Chasseurs[0].BallesRestantes.Should().Be(7);
    }

    [Fact]
    public void EchoueCarPartieNexistePas()
    {
        var id = Guid.NewGuid();
        var repository = new PartieDeChasseRepositoryForTests();
        var service = new PartieDeChasseService(repository, () => DateTime.Now);
        var tirerQuandPartieExistePas = () => service.Tirer(id, "Bernard");

        tirerQuandPartieExistePas.Should()
            .Throw<LaPartieDeChasseNexistePas>();
        repository.SavedPartieDeChasse().Should().BeNull();
    }

    [Fact]
    public void EchoueAvecUnChasseurNayantPlusDeBalles()
    {
        var id = Guid.NewGuid();
        var repository = new PartieDeChasseRepositoryForTests();

        repository.Add(new PartieDeChasse
        {
            Id = id,
            Chasseurs = new List<Chasseur>
            {
                new() { Nom = "Dédé", BallesRestantes = 20 },
                new() { Nom = "Bernard", BallesRestantes = 0 },
                new() { Nom = "Robert", BallesRestantes = 12 },
            },
            Terrain = new Terrain
            {
                Nom = "Pitibon sur Sauldre",
                NbGalinettes = 3
            },
            Status = PartieStatus.EnCours,
            Events = new List<Event>()
        });

        var service = new PartieDeChasseService(repository, () => DateTime.Now);
        var tirerSansBalle = () => service.Tirer(id, "Bernard");

        tirerSansBalle.Should()
            .Throw<TasPlusDeBallesMonVieuxChasseALaMain>();
    }

    [Fact]
    public void EchoueCarLeChasseurNestPasDansLaPartie()
    {
        var id = Guid.NewGuid();
        var repository = new PartieDeChasseRepositoryForTests();

        repository.Add(new PartieDeChasse
        {
            Id = id,
            Chasseurs = new List<Chasseur>
            {
                new() { Nom = "Dédé", BallesRestantes = 20 },
                new() { Nom = "Bernard", BallesRestantes = 8 },
                new() { Nom = "Robert", BallesRestantes = 12 },
            },
            Terrain = new Terrain
            {
                Nom = "Pitibon sur Sauldre",
                NbGalinettes = 3
            },
            Status = PartieStatus.EnCours
        });

        var service = new PartieDeChasseService(repository, () => DateTime.Now);
        var chasseurInconnuVeutTirer = () => service.Tirer(id, "Chasseur inconnu");

        chasseurInconnuVeutTirer.Should()
            .Throw<ChasseurInconnu>();
        repository.SavedPartieDeChasse().Should().BeNull();
    }

    [Fact]
    public void EchoueSiLesChasseursSontEnApero()
    {
        var id = Guid.NewGuid();
        var repository = new PartieDeChasseRepositoryForTests();

        repository.Add(new PartieDeChasse
        {
            Id = id,
            Chasseurs = new List<Chasseur>
            {
                new() { Nom = "Dédé", BallesRestantes = 20 },
                new() { Nom = "Bernard", BallesRestantes = 8 },
                new() { Nom = "Robert", BallesRestantes = 12 },
            },
            Terrain = new Terrain
            {
                Nom = "Pitibon sur Sauldre",
                NbGalinettes = 3
            },
            Status = PartieStatus.Apéro,
            Events = new List<Event>()
        });

        var service = new PartieDeChasseService(repository, () => DateTime.Now);
        var tirerEnPleinApéro = () => service.Tirer(id, "Chasseur inconnu");

        tirerEnPleinApéro.Should()
            .Throw<OnTirePasPendantLapéroCestSacré>();
    }

    [Fact]
    public void EchoueSiLaPartieDeChasseEstTerminée()
    {
        var id = Guid.NewGuid();
        var repository = new PartieDeChasseRepositoryForTests();

        repository.Add(new PartieDeChasse
        {
            Id = id,
            Chasseurs = new List<Chasseur>
            {
                new() { Nom = "Dédé", BallesRestantes = 20 },
                new() { Nom = "Bernard", BallesRestantes = 8 },
                new() { Nom = "Robert", BallesRestantes = 12 },
            },
            Terrain = new Terrain
            {
                Nom = "Pitibon sur Sauldre",
                NbGalinettes = 3
            },
            Status = PartieStatus.Terminée,
            Events = new List<Event>()
        });

        var service = new PartieDeChasseService(repository, () => DateTime.Now);
        var tirerQuandTerminée = () => service.Tirer(id, "Chasseur inconnu");

        tirerQuandTerminée.Should()
            .Throw<OnTirePasQuandLaPartieEstTerminée>();
    }
}

public record ChasseurBuilder(string Nom, int BallesRestantes)
{
    public Chasseur Build()
    {
        return new() { Nom = this.Nom, BallesRestantes = this.BallesRestantes };
    }
}