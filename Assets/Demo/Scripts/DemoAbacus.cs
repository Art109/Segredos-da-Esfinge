using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DemoAbacus : MonoBehaviour , RockReceiver
{

    [SerializeField] GameObject weightDisplayer;
    DemoRock playerRock;
    float currentWeight;


    public void TakeRock(DemoPlayer player, DemoRock rock)
    {
        Debug.Log("Recebi a pedra");
        if(rock != null){
            playerRock = rock;
            UpdateWeight();
        }
    }

    public void RemoveRock(DemoPlayer player, DemoRock rock)
    {
        Debug.Log("Devolvi a pedra");
        if (rock == playerRock)
        {
            playerRock = null;
            UpdateWeight();
        }
    }


    void UpdateWeight()
    {
        currentWeight = 0;

        if(playerRock != null)
            currentWeight = playerRock.Weight;

        ShowWeight();
    }

    void ShowWeight(){
        if(currentWeight > 0){
            weightDisplayer.SetActive(true);
            weightDisplayer.GetComponent<TextMeshPro>().text = ((int)currentWeight).ToString();

        }
        else{
           weightDisplayer.SetActive(false);
        }
    }
}
