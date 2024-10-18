using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GeradorPedras : MonoBehaviour
{
    private System.Random random = new System.Random();
    [SerializeField] GameObject pedraPrefab;

    private List<int> GerarCombinacaoPeso(int pesoObjetivo, int numPedras)
    {
        List<int> pedras = null;
        List<List<int>> combinacoes = new List<List<int>>();
        int maxCombinacoes = 0; // Variável para armazenar o máximo de combinações encontradas

        for (int tentativa = 0; tentativa < 1000; tentativa++) // Limite de tentativas para evitar loop infinito
        {
            List<int> pedrasTemp = new List<int>();
            int pesoRestante = pesoObjetivo + random.Next(1, pesoObjetivo);

            for (int i = 0; i < numPedras; i++)
            {
                int maxValorPedra = Mathf.Min(pesoRestante / (numPedras - i), pesoObjetivo - 1);
                int valorPedra = random.Next(1, maxValorPedra + 1);

                pedrasTemp.Add(valorPedra);
                pesoRestante -= valorPedra;
            }

            var combinacoesTemp = EncontrarCombinacoes(pedrasTemp, pesoObjetivo);

            // Verifica se essa tentativa gerou mais combinações válidas
            if (combinacoesTemp.Count > maxCombinacoes)
            {
                maxCombinacoes = combinacoesTemp.Count;
                pedras = new List<int>(pedrasTemp); // Salva essa combinação de pedras
                combinacoes = combinacoesTemp; // Atualiza as combinações válidas
            }

            // Se encontrar o máximo de combinações possíveis para esse peso
            if (maxCombinacoes == (int)Mathf.Pow(2, numPedras) - 1)
            {
                break; // Interrompe se atingir o máximo teórico de combinações
            }
        }

        return pedras;
    }



    private List<List<int>> EncontrarCombinacoes(List<int> pedras, int pesoObjetivo)
    {
        List<List<int>> combinacoes = new List<List<int>>();

        int totalCombinacoes = (int)Mathf.Pow(2, pedras.Count);

        for (int i = 1; i < totalCombinacoes; i++)
        {
            List<int> combinacaoAtual = new List<int>();
            int somaAtual = 0;

            for (int j = 0; j < pedras.Count; j++)
            {
                if ((i & (1 << j)) != 0)
                {
                    combinacaoAtual.Add(pedras[j]);
                    somaAtual += pedras[j];
                }
            }

            if (somaAtual == pesoObjetivo)
            {
                combinacoes.Add(combinacaoAtual);
            }
        }

        return combinacoes.Distinct(new ListComparer()).ToList();
    }

    public void InstanciarPedras(int pesoObjetivo, List<Transform> posicoes)
    {
        int numPedras = posicoes.Count;
        List<int> pedras = GerarCombinacaoPeso(pesoObjetivo, numPedras);

        pedras = pedras.OrderBy(x => random.Next()).ToList();

        int somaPedras = pedras.Sum();
        //Debug.Log($"Instanciando Pedras - Peso Objetivo: {pesoObjetivo}, Soma das Pedras: {somaPedras}");

        for (int i = 0; i < numPedras; i++)
        {
            GameObject pedra = Instantiate(pedraPrefab, posicoes[i].position, Quaternion.identity);
            Rock rockScript = pedra.GetComponent<Rock>();
            if (rockScript != null)
            {
                rockScript.setPeso(pedras[i]);
            }

            //Debug.Log($"Pedra {i + 1}: Peso {pedras[i]}");
        }

        List<List<int>> combinacoes = EncontrarCombinacoes(pedras, pesoObjetivo);

        Debug.Log("Peso objetivo :" + pesoObjetivo);

        Debug.Log($"Número de Combinações Possíveis: {combinacoes.Count}");

        foreach (var combinacao in combinacoes)
        {
            Debug.Log($"Combinação: {string.Join(", ", combinacao)}");
        }
    }
}

public class ListComparer : IEqualityComparer<List<int>>
{
    public bool Equals(List<int> x, List<int> y)
    {
        if (x == null || y == null)
            return false;

        return x.OrderBy(a => a).SequenceEqual(y.OrderBy(b => b));
    }

    public int GetHashCode(List<int> obj)
    {
        int hash = 17;
        foreach (int val in obj.OrderBy(x => x))
        {
            hash = hash * 23 + val.GetHashCode();
        }
        return hash;
    }
}
