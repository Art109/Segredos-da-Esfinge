using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class Piramide: MonoBehaviour
{
    private List<Sala> listSalas;
    private Sala salaAtual;

    // Salas
    Sala sala1;
    Sala sala2;
    Sala sala3;

    public Sala SalaAtual { get => salaAtual; set => salaAtual = value; }
    public List<Sala> ListSalas { get => listSalas; set => listSalas = value; }

    private BalanceFinder balanceFinder;

    private List<Balance> balanceSala1;
    private List<Balance> balanceSala2;
    private List<Balance> balancesSala3;

    public void iniciarSalas()
    {
        balanceFinder = FindObjectOfType<BalanceFinder>();

        // Setando balanças
        balanceSala1 = new List<Balance>
        {
            balanceFinder.Balances[0]
        };

        balanceSala2 = new List<Balance>
        {
            balanceFinder.Balances[1]
        };

        balancesSala3 = new List<Balance>
        {
            balanceFinder.Balances[2],
            balanceFinder.Balances[3],
            balanceFinder.Balances[4],
            balanceFinder.Balances[5]
        };

        sala1 = GerarSala(1, 250, 20, balanceSala1);
        sala2 = GerarSala(2, 200, 30, balanceSala2);
        sala3 = GerarSala(3, 300, 40, balancesSala3);

        ListSalas = new List<Sala>
        {
            sala1,
            sala2,
            sala3
        };
        SalaAtual = ListSalas[0];
    }

    private Sala GerarSala(int numero, float tempoParaConclusao, int pontuacao, List<Balance> balances)
    {
        Sala sala = new Sala();
        sala.Numero = numero;
        sala.TempoParaConclusao = tempoParaConclusao;
        sala.Pontuacao = pontuacao;
        sala.Balances = balances;
        //sala.imprimirNomesBalancas(balances);
        //sala.Balance = balance;
        // Setando a pontuacao bonus
        int pontuacaoBonus = (int)Math.Round(pontuacao * 0.2f);
        sala.PontuacaoBonus = pontuacaoBonus;
        return sala;
    }

    public void trocarSala(int numeroDaSala)
    {
        salaAtual = listSalas[numeroDaSala];
    }
}