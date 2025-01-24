using Bouchonnois.Domain;
using Bouchonnois.Service;
using Bouchonnois.Tests.Doubles;

namespace Bouchonnois.Tests.Service;

public class PartieDeChasseTestContext
{
    private PartieDeChasseRepositoryForTests _repository;
    private PartieDeChasseService _service;

    public PartieDeChasseTestContext()
    {
        _repository = new PartieDeChasseRepositoryForTests();
        _service = new PartieDeChasseService(_repository, () => DateTime.Now);
    }

    public PartieDeChasseTestContext EtantDonné(PartieDeChasseBuilder partieDeChasse)
    {
        _repository.Add(partieDeChasse.Build());
        return this;
    }


    public PartieDeChasseTestContext QuandLeChasseurTire(Guid id, string nomDuChasseur)
    {
        _service.Tirer(id, nomDuChasseur);

        return this;
    }
    private protected PartieDeChasseTestContext QuandOnPrendLapéro(Guid id)
    {
     _service.PrendreLapéro(id);
     return this;
    }
    private protected PartieDeChasseTestContext CEstLapéro(Guid id)
    {
     _repository.SavedPartieDeChasse().Status.Should().Be(PartieStatus.Apéro);
     return this;
    }

    public PartieDeChasseTestContext IlReste(int nombreDeBalles, string à)
    {
        _repository.SavedPartieDeChasse().Chasseurs
            .Find(chasseur => chasseur.Nom.Equals(à))
            ?.BallesRestantes.Should().Be(nombreDeBalles);
        return this;
    }

    public PartieDeChasseTestContext AucunePartieDeChasseNEstSauvée()
    {
        _repository.SavedPartieDeChasse().Should().BeNull();
        return this;
    }
}