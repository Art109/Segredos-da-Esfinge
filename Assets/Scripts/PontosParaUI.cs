using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PontosParaUI
{
    public static int pontosP1 = 0;
    public static int pontosP2 = 0;
    public static int pontosP3 = 0;
    public static int pontosP4 = 0;

    public static void colocarPontoNaUi(Player player)
    {
        string playerName = player.name;
        GameObject objetoDoTexto = GameObject.Find("ValorPontos" + playerName);
        TextMeshProUGUI textoValorPontos = objetoDoTexto.GetComponent<TextMeshProUGUI>();
        textoValorPontos.text = player.PontuacaoAtual.ToString();
        atribuiPonto(playerName, player.PontuacaoAtual);
    }

    private static void atribuiPonto(string playerName, int pontos)
    {
        if(playerName == "P1")
        {
            pontosP1 = pontos;
        }else if(playerName == "P2")
        {
            pontosP2 = pontos;
        }else if(playerName == "P3")
        {
            pontosP3 = pontos;
        }
        else
        {
            pontosP4 = pontos;
        }
    }
}
