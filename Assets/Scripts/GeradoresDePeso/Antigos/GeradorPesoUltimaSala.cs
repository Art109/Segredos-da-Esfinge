using System.Collections.Generic;

public class GeradorPesoUltimaSala
{
    private static List<string> nomesBalancas;

    public static void GerarPesos(Piramide piramide, List<Player> players)
    {
        setarNomeDasBalancas(piramide);

        for (int i = 0; i < players.Count; i++)
        {
            // Pegando o nome do player
            string playerName = players[i].name;

            // Encontrando o �ndice da balan�a correspondente
            int index = nomesBalancas.FindIndex(nome => nome == "Balan�a" + playerName);

            // Criando a balan�a correspondente para aquele player
            if (index >= 0 && index < piramide.SalaAtual.Balances.Count)
            {
                Balance balancaCorrespondente = piramide.SalaAtual.Balances[index];

                // Atribuir o peso objetivo do player � balan�a correspondente
                players[i].PesoObjetivo = balancaCorrespondente.PesoMaximo;
            }
        }
    }

    // Fun��o para pegar o nome de cada balan�a na lista de balan�as presente na sala 3
    private static void setarNomeDasBalancas(Piramide piramide)
    {
        nomesBalancas = new List<string>();
        for (int i = 0; i < piramide.SalaAtual.Balances.Count; i++)
        {
            nomesBalancas.Add(piramide.SalaAtual.Balances[i].name);
        }
    }
}
