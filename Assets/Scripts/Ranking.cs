using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Ranking : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI primeiroLugar;
    [SerializeField] private TextMeshProUGUI segundoLugar;
    [SerializeField] private TextMeshProUGUI terceiroLugar;
    [SerializeField] private TextMeshProUGUI quartoLugar;

    void Start()
    {
        Dictionary<string, int> pontuacoes = new Dictionary<string, int>()
        {
            { "P1", PontosParaUI.pontosP1 },
            { "P2", PontosParaUI.pontosP2 },
            { "P3", PontosParaUI.pontosP3 },
            { "P4", PontosParaUI.pontosP4 }
        };

        // Ordena as pontuações em ordem decrescente
        var pontuacoesOrdenadas = new List<KeyValuePair<string, int>>(pontuacoes);
        pontuacoesOrdenadas.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));

        // Atualiza os textos de acordo com a pontuação ordenada
        pontosParaTexto(pontuacoesOrdenadas);
    }

    private void pontosParaTexto(List<KeyValuePair<string, int>> pontuacoesOrdenadas)
    {
        // Define os textos dos lugares de acordo com as pontuações ordenadas
        primeiroLugar.text = pontuacoesOrdenadas[0].Key + ": " + pontuacoesOrdenadas[0].Value.ToString();
        segundoLugar.text = pontuacoesOrdenadas[1].Key + ": " + pontuacoesOrdenadas[1].Value.ToString();
        terceiroLugar.text = pontuacoesOrdenadas[2].Key + ": " + pontuacoesOrdenadas[2].Value.ToString();
        quartoLugar.text = pontuacoesOrdenadas[3].Key + ": " + pontuacoesOrdenadas[3].Value.ToString();
    }
}
