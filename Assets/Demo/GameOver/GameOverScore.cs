using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverScore : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI player1Score;
    [SerializeField]  TextMeshProUGUI player2Score;  
    void Start()
    {
        player1Score.text = $"{PlayerPrefs.GetInt("player1Score", 0)}";
        player2Score.text = $"{PlayerPrefs.GetInt("player2Score", 0)}";

        PlayerPrefs.DeleteAll();
    }

    

    // Update is called once per frame
   
}
