using Bouchonnois.Domain;
using Bouchonnois.Service;
using Bouchonnois.Service.Exceptions;
using Bouchonnois.Tests.Doubles;

namespace Bouchonnois.Tests.Service;

public class TerminerLaPartieDeChasse
{
    [Fact]
    public void QuandLaPartieEstEnCoursEt1SeulChasseurGagne()
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
                new() { Nom = "Robert", BallesRestantes = 12, NbGalinettes = 2 },
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
        var meilleurChasseur = service.TerminerLaPartie(id);

        var savedPartieDeChasse = repository.SavedPartieDeChasse();
        savedPartieDeChasse.Id.Should().Be(id);
        savedPartieDeChasse.Status.Should().Be(PartieStatus.Terminée);
        savedPartieDeChasse.Terrain.Nom.Should().Be("Pitibon sur Sauldre");
        savedPartieDeChasse.Terrain.NbGalinettes.Should().Be(3);
        savedPartieDeChasse.Chasseurs.Should().HaveCount(3);
        savedPartieDeChasse.Chasseurs[0].Nom.Should().Be("Dédé");
        savedPartieDeChasse.Chasseurs[0].BallesRestantes.Should().Be(20);
        savedPartieDeChasse.Chasseurs[0].NbGalinettes.Should().Be(0);
        savedPartieDeChasse.Chasseurs[1].Nom.Should().Be("Bernard");
        savedPartieDeChasse.Chasseurs[1].BallesRestantes.Should().Be(8);
        savedPartieDeChasse.Chasseurs[1].NbGalinettes.Should().Be(0);
        savedPartieDeChasse.Chasseurs[2].Nom.Should().Be("Robert");
        savedPartieDeChasse.Chasseurs[2].BallesRestantes.Should().Be(12);
        savedPartieDeChasse.Chasseurs[2].NbGalinettes.Should().Be(2);

        meilleurChasseur.Should().Be("Robert");
    }

    [Fact]
    public void QuandLaPartieEstEnCoursEt1SeulChasseurDansLaPartie()
    {
        var id = Guid.NewGuid();
        var repository = new PartieDeChasseRepositoryForTests();

        repository.Add(new PartieDeChasse
        {
            Id = id,
            Chasseurs = new List<Chasseur>
            {
                new() { Nom = "Robert", BallesRestantes = 12, NbGalinettes = 2 }
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
        var meilleurChasseur = service.TerminerLaPartie(id);

        var savedPartieDeChasse = repository.SavedPartieDeChasse();
        savedPartieDeChasse.Id.Should().Be(id);
        savedPartieDeChasse.Status.Should().Be(PartieStatus.Terminée);
        savedPartieDeChasse.Terrain.Nom.Should().Be("Pitibon sur Sauldre");
        savedPartieDeChasse.Terrain.NbGalinettes.Should().Be(3);
        savedPartieDeChasse.Chasseurs.Should().HaveCount(1);
        savedPartieDeChasse.Chasseurs[0].Nom.Should().Be("Robert");
        savedPartieDeChasse.Chasseurs[0].BallesRestantes.Should().Be(12);
        savedPartieDeChasse.Chasseurs[0].NbGalinettes.Should().Be(2);

        meilleurChasseur.Should().Be("Robert");
    }

    [Fact]
    public void QuandLaPartieEstEnCoursEt2ChasseursExAequo()
    {
        var id = Guid.NewGuid();
        var repository = new PartieDeChasseRepositoryForTests();

        repository.Add(new PartieDeChasse
        {
            Id = id,
            Chasseurs = new List<Chasseur>
            {
                new() { Nom = "Dédé", BallesRestantes = 20, NbGalinettes = 2 },
                new() { Nom = "Bernard", BallesRestantes = 8, NbGalinettes = 2 },
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
        var meilleurChasseur = service.TerminerLaPartie(id);

        var savedPartieDeChasse = repository.SavedPartieDeChasse();
        savedPartieDeChasse.Id.Should().Be(id);
        savedPartieDeChasse.Status.Should().Be(PartieStatus.Terminée);
        savedPartieDeChasse.Terrain.Nom.Should().Be("Pitibon sur Sauldre");
        savedPartieDeChasse.Terrain.NbGalinettes.Should().Be(3);
        savedPartieDeChasse.Chasseurs.Should().HaveCount(3);
        savedPartieDeChasse.Chasseurs[0].Nom.Should().Be("Dédé");
        savedPartieDeChasse.Chasseurs[0].BallesRestantes.Should().Be(20);
        savedPartieDeChasse.Chasseurs[0].NbGalinettes.Should().Be(2);
        savedPartieDeChasse.Chasseurs[1].Nom.Should().Be("Bernard");
        savedPartieDeChasse.Chasseurs[1].BallesRestantes.Should().Be(8);
        savedPartieDeChasse.Chasseurs[1].NbGalinettes.Should().Be(2);
        savedPartieDeChasse.Chasseurs[2].Nom.Should().Be("Robert");
        savedPartieDeChasse.Chasseurs[2].BallesRestantes.Should().Be(12);
        savedPartieDeChasse.Chasseurs[2].NbGalinettes.Should().Be(0);

        meilleurChasseur.Should().Be("Dédé, Bernard");
    }

    [Fact]
    public void QuandLaPartieEstEnCoursEtToutLeMondeBrocouille()
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
            Status = PartieStatus.EnCours,
            Events = new List<Event>()
        });

        var service = new PartieDeChasseService(repository, () => DateTime.Now);
        var meilleurChasseur = service.TerminerLaPartie(id);

        var savedPartieDeChasse = repository.SavedPartieDeChasse();
        savedPartieDeChasse.Id.Should().Be(id);
        savedPartieDeChasse.Status.Should().Be(PartieStatus.Terminée);
        savedPartieDeChasse.Terrain.Nom.Should().Be("Pitibon sur Sauldre");
        savedPartieDeChasse.Terrain.NbGalinettes.Should().Be(3);
        savedPartieDeChasse.Chasseurs.Should().HaveCount(3);
        savedPartieDeChasse.Chasseurs[0].Nom.Should().Be("Dédé");
        savedPartieDeChasse.Chasseurs[0].BallesRestantes.Should().Be(20);
        savedPartieDeChasse.Chasseurs[0].NbGalinettes.Should().Be(0);
        savedPartieDeChasse.Chasseurs[1].Nom.Should().Be("Bernard");
        savedPartieDeChasse.Chasseurs[1].BallesRestantes.Should().Be(8);
        savedPartieDeChasse.Chasseurs[1].NbGalinettes.Should().Be(0);
        savedPartieDeChasse.Chasseurs[2].Nom.Should().Be("Robert");
        savedPartieDeChasse.Chasseurs[2].BallesRestantes.Should().Be(12);
        savedPartieDeChasse.Chasseurs[2].NbGalinettes.Should().Be(0);

        meilleurChasseur.Should().Be("Brocouille");
    }

    [Fact]
    public void QuandLesChasseursSontALaperoEtTousExAequo()
    {
        var id = Guid.NewGuid();
        var repository = new PartieDeChasseRepositoryForTests();

        repository.Add(new PartieDeChasse
        {
            Id = id,
            Chasseurs = new List<Chasseur>
            {
                new() { Nom = "Dédé", BallesRestantes = 20, NbGalinettes = 3 },
                new() { Nom = "Bernard", BallesRestantes = 8, NbGalinettes = 3 },
                new() { Nom = "Robert", BallesRestantes = 12, NbGalinettes = 3 }
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
        var meilleurChasseur = service.TerminerLaPartie(id);

        var savedPartieDeChasse = repository.SavedPartieDeChasse();
        savedPartieDeChasse.Id.Should().Be(id);
        savedPartieDeChasse.Status.Should().Be(PartieStatus.Terminée);
        savedPartieDeChasse.Terrain.Nom.Should().Be("Pitibon sur Sauldre");
        savedPartieDeChasse.Terrain.NbGalinettes.Should().Be(3);
        savedPartieDeChasse.Chasseurs.Should().HaveCount(3);
        savedPartieDeChasse.Chasseurs[0].Nom.Should().Be("Dédé");
        savedPartieDeChasse.Chasseurs[0].BallesRestantes.Should().Be(20);
        savedPartieDeChasse.Chasseurs[0].NbGalinettes.Should().Be(3);
        savedPartieDeChasse.Chasseurs[1].Nom.Should().Be("Bernard");
        savedPartieDeChasse.Chasseurs[1].BallesRestantes.Should().Be(8);
        savedPartieDeChasse.Chasseurs[1].NbGalinettes.Should().Be(3);
        savedPartieDeChasse.Chasseurs[2].Nom.Should().Be("Robert");
        savedPartieDeChasse.Chasseurs[2].BallesRestantes.Should().Be(12);
        savedPartieDeChasse.Chasseurs[2].NbGalinettes.Should().Be(3);

        meilleurChasseur.Should().Be("Dédé, Bernard, Robert");
    }

    [Fact]
    public void EchoueSiLaPartieDeChasseEstDéjàTerminée()
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
            Status = PartieStatus.Terminée
        });

        var service = new PartieDeChasseService(repository, () => DateTime.Now);
        var prendreLapéroQuandTerminée = () => service.TerminerLaPartie(id);

        prendreLapéroQuandTerminée.Should()
            .Throw<QuandCestFiniCestFini>();

        repository.SavedPartieDeChasse().Should().BeNull();
    }
}