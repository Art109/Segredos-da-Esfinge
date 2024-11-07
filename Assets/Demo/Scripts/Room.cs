using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public static event Action OnRoomEndend;
    

    private List<DemoPlayer> playersInRoom = new List<DemoPlayer>();

    RockGenerator rockGenerator;

    
    int balaceDone = 0;


    void OnEnable(){
        DemoBalance.OnBalaceCompleted += BalanceDone;
        DemoBalance.OnDamagePlayer += DamagePlayer;
        DemoBalance.OnDamagePlayers += DamagePlayers;
    }

    void OnDisable(){
        DemoBalance.OnBalaceCompleted -= BalanceDone;
        DemoBalance.OnDamagePlayer -= DamagePlayer;
        DemoBalance.OnDamagePlayers -= DamagePlayers;
    }

    void Start(){
        Debug.Log($"{this.name}Estou começando");
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

    void BalanceDone(){
        balaceDone++;
        if(balances.Count == balaceDone)
        {
            Debug.Log("Todas as Balanças dessa sala foram completas");
            EndRoom();
        }
    }

    void ObjectiveInitiatilizer(){
        Debug.Log($"{this.name}Defini os objetivos");
        if(balances.Count > 1)
        {
            int balanceIndex = 0;
            foreach(var player in playersInRoom)
            {
                if(balanceIndex < balances.Count)
                {
                    player.ObjectiveFraction = ObjectiveGenerator.GerarFracaoAleatoria(balances[balanceIndex].MaxWeight);
                    player.ObjectiveWeight = balances[balanceIndex].MaxWeight * player.ObjectiveFraction.numerator / player.ObjectiveFraction.denominator;
                    balanceIndex++;
                }
            }
        }
        else
        {
            foreach(var player in playersInRoom)
            {
                player.ObjectiveFraction = ObjectiveGenerator.GerarFracaoAleatoria(balances[0].MaxWeight);
                player.ObjectiveWeight = (balances[0].MaxWeight * player.ObjectiveFraction.numerator) / player.ObjectiveFraction.denominator;  
            }
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
