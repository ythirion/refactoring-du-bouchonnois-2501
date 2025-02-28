using Bouchonnois.Domain;
using Bouchonnois.Service;

namespace Bouchonnois.Tests.Service;

public class PartieDeChasseBuilder()
{
    private Terrain _terrain = new Terrain
    {
        Nom = "Le bois de derrière chez Dédé", NbGalinettes = 3
    };

    private List<Chasseur> _chasseurs = new List<Chasseur>();
    private PartieStatus _partieStatus = PartieStatus.EnCours;
    private Guid _id = Guid.Parse("BFB8EE46-34C2-4152-A9B9-F863F18FBDF5");
    private List<Event> _events = new List<Event>();

    public static PartieDeChasseBuilder UnePartieDeChasse()
    {
        var partieDeChasseBuilder = new PartieDeChasseBuilder();
        return partieDeChasseBuilder;
    }


    public static PartieDeChasseBuilder UnePartieDeChasse(Guid id)
    {
        var partieDeChasseBuilder = new PartieDeChasseBuilder();
        partieDeChasseBuilder._id = id;
        return partieDeChasseBuilder;
    }

    public PartieDeChasse Build()
    {
        return new PartieDeChasse
        {
            Id = _id,
            Chasseurs = _chasseurs,
            Terrain = _terrain,
            Status = _partieStatus,
            Events = _events,
        };
    }

    public PartieDeChasseBuilder EnCours()
    {
        _partieStatus = PartieStatus.EnCours;
        return this;
    }


    public PartieDeChasseBuilder OuParticipent(params ChasseurBuilder[] chasseurs)
    {
        _chasseurs.AddRange(from chasseur in chasseurs select chasseur.Build());
        return this;
    }

    public PartieDeChasseBuilder OuParticipe(ChasseurBuilder chasseur)
    {
        OuParticipent(chasseur);
        return this;
    }

    public PartieDeChasseBuilder LorsDeLApéro()
    {
        _partieStatus = PartieStatus.Apéro;
        return this;
    }

    public PartieDeChasseBuilder Terminée()
    {
        _partieStatus = PartieStatus.Terminée;
        return this;
    }

    public PartieDeChasseBuilder Sur(TerrainBuilder terrain)
    {
        _terrain = terrain.Build();
        return this;
    }
}

public class TerrainBuilder
{
    private string _nom;
    private readonly int _nbGalinettes;

    public TerrainBuilder(string nom, int nbGalinettes)
    {
        _nom = nom;
        _nbGalinettes = nbGalinettes;
    }

    public Terrain Build()
    {
        return new Terrain
        {
            Nom = this._nom,
            NbGalinettes = _nbGalinettes
        };
    }
}