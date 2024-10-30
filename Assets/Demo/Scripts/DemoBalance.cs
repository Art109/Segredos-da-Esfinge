using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoBalance : MonoBehaviour, RockReceiver
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


    void BalanceCompleted(){
        Debug.Log("Terminei a Balança");
        OnBalaceCompleted?.Invoke();
        
    }

    public void TakeRock(DemoPlayer player, DemoRock rock)
    {
        throw new NotImplementedException();
    }

    public void RemoveRock(DemoPlayer player, DemoRock rock)
    {
        throw new NotImplementedException();
    }
}
