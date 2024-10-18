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

            // Encontrando o índice da balança correspondente
            int index = nomesBalancas.FindIndex(nome => nome == "Balança" + playerName);

            // Criando a balança correspondente para aquele player
            if (index >= 0 && index < piramide.SalaAtual.Balances.Count)
            {
                Balance balancaCorrespondente = piramide.SalaAtual.Balances[index];

                // Atribuir o peso objetivo do player à balança correspondente
                players[i].PesoObjetivo = balancaCorrespondente.PesoMaximo;
            }
        }
    }

    // Função para pegar o nome de cada balança na lista de balanças presente na sala 3
    private static void setarNomeDasBalancas(Piramide piramide)
    {
        nomesBalancas = new List<string>();
        for (int i = 0; i < piramide.SalaAtual.Balances.Count; i++)
        {
            nomesBalancas.Add(piramide.SalaAtual.Balances[i].name);
        }
    }
}
