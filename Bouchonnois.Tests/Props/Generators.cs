using FsCheck;
using FsCheck.Fluent;

namespace Bouchonnois.Tests.Props;

public static class Generators
{
    private static Gen<string> RandomString() => ArbMap.Default.ArbFor<string>().Generator;

    public static Arbitrary<(string nom, int nbGalinettes)> TerrainGenerator(int minGalinettes, int maxGalinettes)
        => (from nom in RandomString()
            from nbGalinette in Gen.Choose(minGalinettes, maxGalinettes)
            select (nom, nbGalinette)).ToArbitrary();

    public static Arbitrary<(string nom, int nbGalinettes)> TerrainRicheEnGalinettes()
        => TerrainGenerator(1, int.MaxValue);

    public static Arbitrary<(string nom, int nbGalinettes)> TerrainSansGalinettes()
        => TerrainGenerator(-int.MaxValue, 0);

    private static Arbitrary<(string nom, int nbBalles)> Chasseurs(int minBalles, int maxBalles)
        => (from nom in RandomString()
            from nbBalles in Gen.Choose(minBalles, maxBalles)
            select (nom, nbBalles)).ToArbitrary();

    private static Arbitrary<(string nom, int nbBalles)[]> GroupeDeChasseurs(int minBalles, int maxBalles)
        => (from nbChasseurs in Gen.Choose(1, 1_000)
            select Chasseurs(minBalles, maxBalles).Generator.Sample(nbChasseurs)).ToArbitrary();

    public static Arbitrary<(string nom, int nbBalles)[]> DesChasseursAvecDesBalles()
        => GroupeDeChasseurs(1, int.MaxValue);

    public static Arbitrary<(string nom, int nbBalles)[]> DesChasseursSansBalles()
        => GroupeDeChasseurs(0, 0);
}