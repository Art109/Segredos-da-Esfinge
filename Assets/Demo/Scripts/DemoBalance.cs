using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DemoBalance : MonoBehaviour, RockReceiver
{

    public static event Action OnBalanceCompletion;
    public static event PlayerCompletion OnPlayerCompletion;
    public delegate void PlayerCompletion(DemoPlayer player);
    [SerializeField]int maxWeight;
    public int MaxWeight{get{return maxWeight;}}
    float currentWeight;

    private Dictionary<DemoPlayer, List<DemoRock>> playerRocks; // Dicionário para rastrear pedras por jogador
    private Dictionary<DemoPlayer, float> playerWeights; // Dicionário para rastrear o peso por jogador
    int numberOfConclusions = 0;

    public static event Action OnDamagePlayers;
    public static event Action<DemoPlayer> OnDamagePlayer;


    void OnEnable(){
        DemoPlayer.OnPlayerDeath += RemoveDeadPlayer;
    }

    void OnDisable(){
        DemoPlayer.OnPlayerDeath -= RemoveDeadPlayer;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerRocks = new Dictionary<DemoPlayer, List<DemoRock>>();
        playerWeights = new Dictionary<DemoPlayer, float>();
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


        if(currentWeight > maxWeight)
            OnDamagePlayers?.Invoke();

        foreach(var player in playerWeights.Keys){
            if(player.ObjectiveWeight < playerWeights[player]){
                TrapTrigger(player);
                //playErrorSound();
            }
            if(player.ObjectiveWeight == playerWeights[player] ){
                OnPlayerCompletion.Invoke(player);
                OnBalanceCompletion.Invoke();
            }
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

    void RemoveDeadPlayer(DemoPlayer player){
        if(playerRocks.Keys.Contains(player)){
            playerRocks.Remove(player);
        }
        if(playerWeights.Keys.Contains(player)){
            playerWeights.Remove(player);
        }
        UpdateWeight();
    }


}
