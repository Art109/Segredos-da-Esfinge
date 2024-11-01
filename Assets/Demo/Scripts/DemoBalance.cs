using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoBalance : MonoBehaviour, RockReceiver
{

    public static event Action OnBalaceCompleted;
    [SerializeField]float maxWeight;
    float currentWeight;

    private Dictionary<DemoPlayer, List<DemoRock>> playerRocks; // Dicionário para rastrear pedras por jogador
    private Dictionary<DemoPlayer, float> playerWeights; // Dicionário para rastrear o peso por jogador
    int numberOfConclusions = 0;

    public static event Action OnDamagePlayers;
    public static event Action<DemoPlayer> OnDamagePlayer;




    // Start is called before the first frame update
    void Start()
    {
        playerRocks = new Dictionary<DemoPlayer, List<DemoRock>>();
        playerWeights = new Dictionary<DemoPlayer, float>();
    }


    void BalanceCompleted(){
        Debug.Log("Terminei a Balança");
        OnBalaceCompleted?.Invoke();
        
    }

    public void TakeRock(DemoPlayer player, DemoRock rock)
    {
        if (!playerRocks.ContainsKey(player))
        {
            playerRocks[player] = new List<DemoRock>();
        }
        playerRocks[player].Add(rock);
        UpdateWeight();
    }

    public void RemoveRock(DemoPlayer player, DemoRock rock)
    {
        if (playerRocks.ContainsKey(player) && playerRocks[player].Contains(rock))
        {
            playerRocks[player].Remove(rock);
            UpdateWeight();
        }
    }


    void UpdateWeight()
    {
        currentWeight = 0; // Reinicia o peso total antes de recalcular
        //quantidade = 0; // Reinicia a quantidade de jogadores que atingiram o peso objetivo

        foreach (var player in playerRocks.Keys)
        {
            float playerWeight = 0;

            foreach (DemoRock rock in playerRocks[player])
            {
                playerWeight += rock.Weight;
            }

            playerWeights[player] = playerWeight; // Atualiza o peso individual do jogador
            currentWeight += playerWeight; // Adiciona o peso do jogador ao peso total

            // Verifica se o jogador atingiu seu peso objetivo
            if (Mathf.Approximately(playerWeights[player], player.ObjectiveWeight))
            {
                player.CompletedObjective = true;
                numberOfConclusions++; // Incrementa a quantidade se o jogador atingiu o peso objetivo
            }
        }

        //if(currentWeight == maxWeight && playerWeights.)

        Debug.Log("Peso total atualizado -> " + currentWeight);
        //Debug.Log("Quantidade de jogadores que atingiram o peso objetivo -> " + quantidade);

        // Realiza a verificação de armadilha e feedback apenas uma vez
        TrapTrigger();
        //Feedback();
    }

    void TrapTrigger(){
        // Verifica se o peso total na balança excede o peso máximo
        if (currentWeight > maxWeight)
        {

            Room currentRoom = FindObjectOfType<Room>();
            // Para as salas 1 e 2, o dano é causado em todos os jogadores
            if (currentRoom.NumberBalances == 1)
            {
                OnDamagePlayers?.Invoke();
            }
            // Na terceira sala, o dano é causado apenas ao jogador específico
            else
            {
                // Aqui, verificamos qual jogador tem mais peso e causamos dano somente nele
                foreach (var player in playerWeights.Keys)
                {
                    OnDamagePlayer?.Invoke(player);
                }
            }
            //playErrorSound();
        }
    }


}
