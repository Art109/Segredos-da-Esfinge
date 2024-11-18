using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TutorialBalance : MonoBehaviour, RockReceiver
{

    [SerializeField]int maxWeight;
    public int MaxWeight{get{return maxWeight;}}
    [SerializeField] TextMeshPro maxWeightDisplayer;
    float currentWeight;

    private Dictionary<DemoPlayer, List<DemoRock>> playerRocks; // Dicionário para rastrear pedras por jogador
    private Dictionary<DemoPlayer, float> playerWeights; // Dicionário para rastrear o peso por jogador



    void OnEnable(){
        System.Random random = new System.Random();
        maxWeight = random.Next(20, 81);

    }

    void OnDisable(){
    }

    // Start is called before the first frame update
    void Start()
    {
        maxWeightDisplayer.text = $"{maxWeight}";
        playerRocks = new Dictionary<DemoPlayer, List<DemoRock>>();
        playerWeights = new Dictionary<DemoPlayer, float>();
    }

    public void TakeRock(DemoPlayer player, DemoRock rock)
    {

        if(TutorialManager.CurrentTask == "Task2")
            TutorialManager.TaskCompleteTrigger("Task2");
        else
        {
            if (!playerRocks.ContainsKey(player))
            {
                playerRocks[player] = new List<DemoRock>();
            }
            playerRocks[player].Add(rock);
            UpdateWeight();

        }
        
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
        if(TutorialManager.CurrentTask == "Task3")
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
        
    }

    void ProcessWeight()
{
    /*
    if (currentWeight > maxWeight)
    {
        if (OnDamagePlayers != null)
            OnDamagePlayers.Invoke();
        else
            Debug.LogWarning("OnDamagePlayers está nulo!");
    }*/

    // Verifica se playerWeights está inicializado
    if (playerWeights == null)
    {
        Debug.LogError("playerWeights não foi inicializado!");
        return;
    }

    foreach (var player in playerWeights.Keys)
    {
        // Verifica se player não é nulo e se possui uma entrada em playerWeights
        if (player == null || !playerWeights.ContainsKey(player))
        {
            Debug.LogWarning("Jogador está nulo ou não existe em playerWeights.");
            continue;
        }

        if (player.ObjectiveWeight < playerWeights[player])
        {
            TrapTrigger();
            // playErrorSound();
        }
        else if (player.ObjectiveWeight == playerWeights[player])
        {
            // Invoca OnPlayerCompletion se não for nulo
            
            TutorialManager.TaskCompleteTrigger("Task3");
        }
    }
}


    void TrapTrigger(){
        
        TutorialManager.TaskCompleteTrigger("Task3Erro");
         
    }


    
    public void setMaxWeight(int weight){
        maxWeight = weight;
        maxWeightDisplayer.text = $"{maxWeight}";
    }

}