using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Room : MonoBehaviour
{
    
    [SerializeField] private float conclusionTime;
    [SerializeField] private int score;
    [SerializeField] private float bonusScore;
    List<DemoRock> rocksInRoom = new List<DemoRock>();
    [SerializeField]List<DemoBalance> balances;

    public static event Action OnRoomEndend;
    

    private List<DemoPlayer> playersInRoom = new List<DemoPlayer>();

    
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
        playersInRoom = FindObjectsOfType<DemoPlayer>().ToList();
        ObjectiveInitiatilizer();
    }


    void EndRoom(){
        OnRoomEndend?.Invoke();
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
