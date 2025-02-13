using Bouchonnois.Domain;
using Bouchonnois.Service;
using Bouchonnois.Tests.Doubles;

namespace Bouchonnois.Tests.Service;

public class PartieDeChasseTestContext
{
    private PartieDeChasseRepositoryForTests _repository;
    private PartieDeChasseService _service;
    protected Guid id;
    protected DateTime _time = new(2025, 01, 01, 09, 00, 00);

    public PartieDeChasseTestContext()
    {
        _repository = new PartieDeChasseRepositoryForTests();
        _service = new PartieDeChasseService(_repository, () => _time);
    }

    public PartieDeChasseTestContext EtantDonné(PartieDeChasseBuilder partieDeChasse)
    {
        _repository.Add(partieDeChasse.Build());
        return this;
    }

    public PartieDeChasseTestContext EtantDonnéUnePartieDémarée(PartieDeChasseBuilder partieDeChasse)
    {
        var deChasse = partieDeChasse.Build();
        id = _service.Demarrer(
            (deChasse.Terrain.Nom,deChasse.Terrain.NbGalinettes),
            deChasse.Chasseurs.Select(chasseur => (chasseur.Nom,chasseur.BallesRestantes)).ToList());
        return this;
    }

    public PartieDeChasseTestContext LeChasseurTire(string nomDuChasseur)
    {
        try
        {
            _service.Tirer(id, nomDuChasseur);
        }
        catch
        {
        }
        return this;
    } 
    
    // [Obsolete]
    public PartieDeChasseTestContext QuandLeChasseurTire(Guid id, string nomDuChasseur)
    {
        _service.Tirer(id, nomDuChasseur);

        return this;
    }
    
    public PartieDeChasseTestContext LeChasseurTireSurUneGalinette(string nomDuChasseur)
    {
        _service.TirerSurUneGalinette(id, nomDuChasseur);
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

    public PartieDeChasseTestContext A(DateTime time)
    {
        _time = time;
        return this;
    }

    public PartieDeChasseTestContext Apres(TimeSpan delai)
    {
        _time = _time.Add(delai);
        return this;
    }

    public PartieDeChasseTestContext LesChasseursPrennentLapéro()
    {
        _service.PrendreLapéro(id);
        return this;
    }

    public PartieDeChasseTestContext LesChasseursReprennentLaPartie()
    {
        _service.ReprendreLaPartie(id);
        return this;
    }

    public PartieDeChasseTestContext TerminerLaPartie()
    {
        _service.TerminerLaPartie(id);
        return this;
    }
    
    public string ConsulterStatus()
    {
       return  _service.ConsulterStatus(id);
    }
}