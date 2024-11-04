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

    public static event Action OnRoomEndend;
    

    private List<DemoPlayer> playersInRoom = new List<DemoPlayer>();


    [SerializeField]int numberBalances;
    public int NumberBalances{get{return numberBalances;}}
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
    }


    void EndRoom(){
        OnRoomEndend?.Invoke();
        Destroy(gameObject);
    }

    void BalanceDone(){
        balaceDone++;
        if(numberBalances == balaceDone)
        {
            Debug.Log("Todas as Balanças dessa sala foram completas");
            EndRoom();
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
