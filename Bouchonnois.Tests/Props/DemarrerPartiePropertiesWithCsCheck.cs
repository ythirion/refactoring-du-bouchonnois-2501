using Bouchonnois.Service;
using Bouchonnois.Tests.Doubles;
using CsCheck;

namespace Bouchonnois.Tests.Props;

public class DemarrerPartiePropertiesWithCsCheck
{
    private readonly PartieDeChasseRepositoryForTests _repository;
    private readonly PartieDeChasseService _service;

    public DemarrerPartiePropertiesWithCsCheck()
    {
        _repository = new PartieDeChasseRepositoryForTests();
        _service = new PartieDeChasseService(_repository, () => DateTime.Now);
    }

    [Fact(Skip = "todo next Seed : 723_W3e884p4")]
    public void Sur1TerrainAvecGalinettesEtChasseursAvecTousDesBalles() =>
        (from terrain in TerrainRicheEnGalinettes()
            from chasseursAvecBalles in DesChasseursAvecDesBalles()
            select (terrain, chasseursAvecBalles))
        .Sample((terrain, chasseurs) => DémarreLaPartieAvecSuccès(terrain, chasseurs));

    private bool DémarreLaPartieAvecSuccès((string nom, int nbGalinettes) terrain,
        IEnumerable<(string nom, int nbBalles)> chasseurs)
        => _service.Demarrer(
            terrain,
            chasseurs.ToList()) == _repository.SavedPartieDeChasse().Id;

    private static readonly Gen<string> RandomString = Gen.String[Gen.Char.AlphaNumeric, 1, 5000];

    private static Gen<(string nom, int nbGalinette)> TerrainGenerator(int minGalinettes, int maxGalinettes)
        => from nom in RandomString
            from nbGalinette in Gen.Int[minGalinettes, maxGalinettes]
            select (nom, nbGalinette);

    public static Gen<(string nom, int nbGalinettes)> TerrainRicheEnGalinettes()
        => TerrainGenerator(1, int.MaxValue);

    private static Gen<(string nom, int nbBalles)> Chasseurs(int minBalles, int maxBalles)
        => from nom in RandomString
            from nbBalles in Gen.Int[minBalles, maxBalles]
            select (nom, nbBalles);

    private static Gen<(string nom, int nbBalles)[]> GroupeDeChasseurs(int minBalles, int maxBalles)
        => from nbChasseurs in Gen.Int[1, 1_000]
            select Chasseurs(minBalles, maxBalles).Array[nbChasseurs].Single();

    private static Gen<(string nom, int nbBalles)[]> DesChasseursAvecDesBalles()
        => GroupeDeChasseurs(1, int.MaxValue);
}