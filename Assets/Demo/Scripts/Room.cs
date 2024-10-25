using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    
    [SerializeField] private float conclusionTime;
    [SerializeField] private int score;
    [SerializeField] private float bonusScore;

    public static event Action OnRoomEndend;


    [SerializeField]int numberBalances;
    int balaceDone = 0;


    void OnEnable(){
        DemoBalance.OnBalaceCompleted += BalanceDone;
    }

    void OnDisable(){
        DemoBalance.OnBalaceCompleted -= BalanceDone;
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


    
}
