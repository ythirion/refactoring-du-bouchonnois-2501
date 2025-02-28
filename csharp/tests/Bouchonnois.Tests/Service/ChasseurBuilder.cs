using Bouchonnois.Domain;

namespace Bouchonnois.Tests.Service;

public class ChasseurBuilder
{
    private string _nom = "Mauricienne";
    private int _nbGalinettes = 0;
    private int _ballesRestantes = 8;

    public static ChasseurBuilder UnChasseur()
    {
        return new ChasseurBuilder();
    }

    public ChasseurBuilder Nommé(string nom)
    {
        _nom = nom;
        return this;
    }

    public ChasseurBuilder AvecDesBalles(int balles)
    {
        _ballesRestantes = balles;
        return this;
    }

    public Chasseur Build()
    {
        return new Chasseur
        {
            Nom = _nom,
            NbGalinettes = _nbGalinettes,
            BallesRestantes = _ballesRestantes,
        };
    }
}