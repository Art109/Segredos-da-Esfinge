using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class SorteadorPeso
{
    public static void DividePeso(int total, int numPartes, List<Player> players, Piramide piramide)
    {
        int restante;

        if(piramide.SalaAtual.Numero == 1)
        {
            restante = total - (numPartes * 6);
        }
        else
        {
            restante = total - (numPartes * 8);
        }

        List<int> lista = new List<int>(new int[numPartes]);

        if(piramide.SalaAtual.Numero == 1)
        {
            // Inicializa a lista com o valor mínimo garantido
            for (int i = 0; i < numPartes; i++)
            {
                lista[i] = 6;
            }
        }
        else
        {
            for (int i = 0; i < numPartes; i++)
            {
                lista[i] = 8;
            }
        }

        System.Random random = new System.Random();

        while (restante > 0)
        {
            int index = random.Next(0, numPartes);
            lista[index] += 1;
            restante -= 1;
        }

        // Distribui os valores na lista de jogadores
        if (lista.Count == players.Count)
        {
            for (int i = 0; i < lista.Count; i++)
            {
                players[i].PesoObjetivo = lista[i];
            }
        }
    }

    static int GetMdc(int numerador, int denominador)
    {
        int maior = 1;
        for (int i = 1; i < denominador; i++)
        {
            if (denominador % i == 0 && numerador % i == 0)
            {
                maior = i;
            }
        }
        return maior;
    }

    public static (int, int) SimplificarFracao(int parteTotal, int pesoTotal)
    {
        int maior = GetMdc(parteTotal, pesoTotal);
        return (parteTotal / maior, pesoTotal / maior);
    }
}
