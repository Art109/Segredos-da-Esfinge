using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Room : MonoBehaviour
{
    
    [SerializeField] private int conclusionTime;
    public int ConclusionTime{get{return conclusionTime;}}
    [SerializeField] private int score;
    [SerializeField] private float bonusScore;
    [SerializeField]List<DemoBalance> balances;
    [SerializeField]int numberOfRocks;
    [SerializeField]RectTransform ObjectiveUI;


    public static event Action OnRoomStarted;
    public static event Action OnRoomEndend;
    

    private List<DemoPlayer> playersInRoom = new List<DemoPlayer>();

    RockGenerator rockGenerator;

    


    void OnEnable(){
        DemoBalance.OnPlayerCompletion += PlayerDone;
        DemoBalance.OnDamagePlayer += DamagePlayer;
        DemoBalance.OnDamagePlayers += DamagePlayers;
        
    }

    void OnDisable(){
        DemoBalance.OnPlayerCompletion -= PlayerDone;
        DemoBalance.OnDamagePlayer -= DamagePlayer;
        DemoBalance.OnDamagePlayers -= DamagePlayers;

    }

    void Start(){
        OnRoomStarted.Invoke();
        playersInRoom = FindObjectsOfType<DemoPlayer>().ToList();
        ObjectiveInitiatilizer();
        rockGenerator = GetComponent<RockGenerator>();
        SpawnRocks();
    }


    void EndRoom(){
        OnRoomEndend?.Invoke();
        Debug.Log($"{this.name}Adeus");
        Destroy(gameObject);
    }

    void PlayerDone(DemoPlayer player){
        int aux = 0 ;
        VerifyPlayersInRoom();
        foreach(var playerAlive in playersInRoom){
            if(playerAlive.CompletedObjective)
            {
                aux++;
                player.Points = score;
            }
                
        }
        if(aux == playersInRoom.Count)
        {
            Debug.Log("Todoss os players completaram o objetivo");
            EndRoom();
        }
    }

    void VerifyPlayersInRoom(){
        // Itera de trás para frente para evitar problemas ao remover elementos
        for (int i = playersInRoom.Count - 1; i >= 0; i--)
        {
            if (!playersInRoom[i].IsAlive)
            {
                playersInRoom.RemoveAt(i);
            }
        }
        
    }

    void ObjectiveInitiatilizer()
    {
        Debug.Log($"{this.name} Defini os objetivos");

        TextMeshPro[] playerTexts = ObjectiveUI.GetComponentsInChildren<TextMeshPro>();
        int playerIndex = 0;

        if (playerTexts.Length < playersInRoom.Count)
        {
            Debug.LogWarning("O número de playerTexts é menor que o número de playersInRoom.");
            return;
        }

        if (balances.Count > 1)
        {
            int balanceIndex = 0;
            foreach (var player in playersInRoom)
            {
                if (balanceIndex < balances.Count)
                {
                    var fraction = ObjectiveGenerator.GerarFracaoAleatoria(balances[balanceIndex].MaxWeight);
                    player.ObjectiveFraction = fraction;
                    player.ObjectiveWeight = (balances[balanceIndex].MaxWeight * player.ObjectiveFraction.numerator) / player.ObjectiveFraction.denominator;

                    // Define o texto no TextMeshPro correspondente
                    playerTexts[playerIndex].text = $"Objective: {player.ObjectiveFraction.numerator}/{player.ObjectiveFraction.denominator}";
                    
                    balanceIndex++;
                    playerIndex++;
                }
            }
        }
        else if (balances.Count == 1)
        {
            foreach (var player in playersInRoom)
            {
                var fraction = ObjectiveGenerator.GerarFracaoAleatoria(balances[0].MaxWeight);
                player.ObjectiveFraction = fraction;
                player.ObjectiveWeight = (balances[0].MaxWeight * player.ObjectiveFraction.numerator) / player.ObjectiveFraction.denominator;

                // Define o texto no TextMeshPro correspondente
                playerTexts[playerIndex].text = $"Objective: {player.ObjectiveFraction.numerator}/{player.ObjectiveFraction.denominator}";

                playerIndex++;
            }
        }
        else
        {
            Debug.LogWarning("Nenhum balance encontrado.");
        }
    }

        void SpawnRocks(){
            Debug.Log($"{this.name}Estou spawnando as pedras");
            int positionIndex = 0;
            Transform rockSpawnPosition = this.transform.Find("SpawnRockPositions");
            foreach(var player in playersInRoom)
            {
                List<Transform> positions = new List<Transform>();
                Transform childPosition = rockSpawnPosition.GetChild(positionIndex);
                

                for(int i = 0; i < numberOfRocks ; i++)
                {
                    Transform child = childPosition.GetChild(i);
                    positions.Add(child);
                }

                // Gera as pedras com o peso objetivo do jogador, incluindo pedras extras
                rockGenerator.InstatiateRocks(player.ObjectiveWeight, positions);
                Debug.Log($"{this.name}spawnei {positions.Count} pedras");
                positionIndex++;
            }
        }

    void DamagePlayer(DemoPlayer player){
            player.ApplyDamage(1); // Suponha que o método ApplyDamage exista no DemoPlayer
            Debug.Log($"Dano causado ao jogador: {player.name}");
        
    }

    void DamagePlayers(){
        foreach (var player in playersInRoom)
        {
            player.ApplyDamage(1); // Suponha que o método ApplyDamage exista no DemoPlayer
            Debug.Log($"Dano causado ao jogador: {player.name}");
        }
        Debug.Log("Todos os jogadores sofreram dano");
    }
    
}
