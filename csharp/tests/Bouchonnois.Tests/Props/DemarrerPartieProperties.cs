using Bouchonnois.Domain;
using Bouchonnois.Service;
using Bouchonnois.Service.Exceptions;
using Bouchonnois.Tests.Doubles;
using FsCheck;
using FsCheck.Fluent;
using FsCheck.Xunit;

namespace Bouchonnois.Tests.Props;

public class DemarrerPartieProperties
{
    private readonly PartieDeChasseRepositoryForTests _repository;
    private readonly PartieDeChasseService _service;

    public DemarrerPartieProperties()
    {
        _repository = new PartieDeChasseRepositoryForTests();
        _service = new PartieDeChasseService(_repository, () => DateTime.Now);
    }

    private static Gen<string> RandomString() => ArbMap.Default.ArbFor<string>().Generator;

    private static Arbitrary<(string nom, int nbGalinettes)> TerrainGenerator(int minGalinettes, int maxGalinettes)
        => (from nom in RandomString()
            from nbGalinette in Gen.Choose(minGalinettes, maxGalinettes)
            select (nom, nbGalinette)).ToArbitrary();

    private static Arbitrary<(string nom, int nbGalinettes)> TerrainRicheEnGalinettes()
        => TerrainGenerator(1, int.MaxValue);

    private static Arbitrary<(string nom, int nbGalinettes)> TerrainSansGalinettes()
        => TerrainGenerator(-int.MaxValue, 0);

    private static Arbitrary<(string nom, int nbBalles)> Chasseurs(int minBalles, int maxBalles)
        => (from nom in RandomString()
            from nbBalles in Gen.Choose(minBalles, maxBalles)
            select (nom, nbBalles)).ToArbitrary();

    private static Arbitrary<(string nom, int nbBalles)[]> GroupeDeChasseurs(int minBalles, int maxBalles)
        => (from nbChasseurs in Gen.Choose(1, 1_000)
            select Chasseurs(minBalles, maxBalles).Generator.Sample(nbChasseurs)).ToArbitrary();

    private static Arbitrary<(string nom, int nbBalles)[]> DesChasseursAvecDesBalles()
        => GroupeDeChasseurs(1, int.MaxValue);

    private static Arbitrary<(string nom, int nbBalles)[]> DesChasseursSansBalles()
        => GroupeDeChasseurs(0, 0);

    [Property]
    public Property Sur1TerrainAvecGalinettesEtChasseursAvecTousDesBalles() =>
        Prop.ForAll(
            TerrainRicheEnGalinettes(),
            DesChasseursAvecDesBalles(),
            (terrain, chasseurs) => DémarreLaPartieAvecSuccès(terrain, chasseurs));

    private bool DémarreLaPartieAvecSuccès((string nom, int nbGalinettes) terrain,
        IEnumerable<(string nom, int nbBalles)> chasseurs)
        => _service.Demarrer(
            terrain,
            chasseurs.ToList()) == _repository.SavedPartieDeChasse().Id;

    public class Echoue : DemarrerPartieProperties
    {
        private static List<(string, int)> PasDeChasseurs => new();

        [Property]
        public Property SansChasseursSurNimporteQuelTerrainRicheEnGalinette()
            => Prop.ForAll(
                TerrainRicheEnGalinettes(),
                terrain =>
                    EchoueAvec<ImpossibleDeDémarrerUnePartieSansChasseur>(
                        terrain,
                        PasDeChasseurs,
                        savedPartieDeChasse => savedPartieDeChasse == null)
            );

        [Property]
        public Property AvecUnTerrainSansGalinettes()
            => Prop.ForAll(
                TerrainSansGalinettes(),
                DesChasseursAvecDesBalles(),
                (terrain, chasseurs) =>
                    EchoueAvec<ImpossibleDeDémarrerUnePartieSansGalinettes>(
                        terrain,
                        chasseurs.ToList(),
                        savedPartieDeChasse => savedPartieDeChasse == null)
            );

        [Property]
        public Property SiAuMoins1ChasseurSansBalle()
            => Prop.ForAll(
                TerrainRicheEnGalinettes(),
                DesChasseursSansBalles(),
                (terrain, chasseurs) =>
                    EchoueAvec<ImpossibleDeDémarrerUnePartieAvecUnChasseurSansBalle>(
                        terrain,
                        chasseurs.ToList(),
                        savedPartieDeChasse => savedPartieDeChasse == null)
            );

        private bool EchoueAvec<TException>(
            (string nom, int nbGalinettes) terrain,
            IEnumerable<(string nom, int nbBalles)> chasseurs,
            Func<PartieDeChasse?, bool>? assert = null) where TException : Exception
            => MustFailWith<TException>(() => _service.Demarrer(terrain, chasseurs.ToList()), assert);

        private bool MustFailWith<TException>(Action action, Func<PartieDeChasse?, bool>? assert = null)
            where TException : Exception
        {
            try
            {
                action();
                return false;
            }
            catch (TException)
            {
                return assert?.Invoke(_repository.SavedPartieDeChasse()) ?? true;
            }
        }
    }
}