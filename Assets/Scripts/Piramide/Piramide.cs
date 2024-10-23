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
    public Sala SalaAtual { get => salaAtual; set => salaAtual = value; }
    public List<Sala> ListSalas { get => listSalas; set => listSalas = value; }

    public void initializeRoom(int roomIndex){

        GameObject room;

        //listSalas.Add(GerarSala());
    }
    public void iniciarSalas()
    {
        


    
        
    }

    private Sala GerarSala(float tempoParaConclusao, int pontuacao, List<Balance> balances)
    {
        Sala sala = new Sala();
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