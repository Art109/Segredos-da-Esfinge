using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sala
{
    private int numero;
    private float tempoParaConclusao;
    private int pontuacao;
    private int pontuacaoBonus;
    private List<Balance> balance;
    public int Numero { get => numero; set => numero = value; }
    public float TempoParaConclusao { get => tempoParaConclusao; set => tempoParaConclusao = value; }
    public int Pontuacao { get => pontuacao; set => pontuacao = value; }
    public int PontuacaoBonus { get => pontuacaoBonus; set => pontuacaoBonus = value; }
    public List<Balance> Balances { get => balance; set => balance = value; }

    public void pontuarJogador(Player player)
    {
        player.PontuacaoAtual += pontuacao;
    }

    public void pontuarBonusJogador(Player player)
    {
        player.PontuacaoAtual += pontuacaoBonus;
    }

    // Função usada para teste para conferir se as balanças
    // foram atribuídas corretamente
    public void imprimirNomesBalancas(List<Balance> balancasTop)
    {
        for(int i = 0; i < balancasTop.Count; i++)
        {
            Debug.Log(balancasTop[i].name);
        }
    }
}
