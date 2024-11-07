using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DemoBalance : MonoBehaviour, RockReceiver
{

    public static event Action OnBalaceCompleted;
    [SerializeField]int maxWeight;
    public int MaxWeight{get{return maxWeight;}}
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

        foreach (var player in playerRocks.Keys)
        {
            float playerWeight = 0;

            foreach (DemoRock rock in playerRocks[player])
            {
                playerWeight += rock.Weight;
            }

            playerWeights[player] = playerWeight; // Atualiza o peso individual do jogador
            currentWeight += playerWeight; // Adiciona o peso do jogador ao peso total

            
        }

        Debug.Log("Peso total atualizado -> " + currentWeight);
        
        ProcessWeight();
    }

    void ProcessWeight(){

        int numberOfConclusions = 0;

        if(currentWeight > maxWeight)
            OnDamagePlayers?.Invoke();

        foreach(var player in playerWeights.Keys){
            if(player.ObjectiveWeight < playerWeights[player]){
                TrapTrigger(player);
                //playErrorSound();
            }
            if(player.ObjectiveWeight == playerWeights[player] ){
                player.CompletedObjective = true;
                numberOfConclusions++;
            }
        }

        if(numberOfConclusions == playerWeights.Count){
            BalanceCompleted();
        }
    }

    void TrapTrigger(DemoPlayer player){
        
        OnDamagePlayer?.Invoke(player);
        //playErrorSound();
         
    }

    public static void TriggerDamagePlayersEvent()
    {
        OnDamagePlayers?.Invoke();
    }


}
