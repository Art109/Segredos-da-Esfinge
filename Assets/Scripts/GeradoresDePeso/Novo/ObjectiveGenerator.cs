using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveGenerator 
{
    public static void DividirPeso(int pesoTotal, List<Player> players)
    {
        int numPartes = players.Count;
        List<(int, int)> fracoes = new List<(int, int)>();
        int somaNumerador = 0;

        // Gera numeradores de forma aleatória para as frações
        System.Random random = new System.Random();

        for (int i = 0; i < numPartes - 1; i++)
        {
            int numerador = random.Next(1, pesoTotal); // Gera numerador aleatório
            int denominador = pesoTotal;               // Denominador é o peso total
            fracoes.Add((numerador, denominador));
            somaNumerador += numerador;
        }

        // Última fração é ajustada para que a soma dos numeradores seja igual ao total
        fracoes.Add((pesoTotal - somaNumerador, pesoTotal));

        // Distribui o peso entre os jogadores garantindo que seja inteiro
        for (int i = 0; i < players.Count; i++)
        {
            var (numerador, denominador) = SimplificarFracao(fracoes[i].Item1, fracoes[i].Item2);
            players[i].PesoObjetivo = (pesoTotal * numerador) / denominador;
            players[i].FracaoPesoObjetivo = (numerador, denominador);
        }
    }

    public static (int, int) SimplificarFracao(int numerador, int denominador)
    {
        int mdc = GetMdc(numerador, denominador);
        return (numerador / mdc, denominador / mdc);
    }

    static int GetMdc(int numerador, int denominador)
    {
        while (denominador != 0)
        {
            int temp = denominador;
            denominador = numerador % denominador;
            numerador = temp;
        }
        return numerador;
    }

}
