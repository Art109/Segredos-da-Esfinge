using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoBalance : MonoBehaviour
{

    public static event Action OnBalaceCompleted;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeRock(DemoRock rock, DemoPlayer player){
        Debug.Log("O Player" + player + " me entregou a pedra" + rock);
    }

    void BalanceCompleted(){
        Debug.Log("Terminei a Balança");
        OnBalaceCompleted?.Invoke();
        
    }
}
