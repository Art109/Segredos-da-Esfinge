using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectiveUI : MonoBehaviour
{
    void OnEnable(){
        Room.OnObjectiveUpdate += UpdateUI;
    }

    void OnDisable(){
        Room.OnObjectiveUpdate -= UpdateUI;
    }


    void UpdateUI(DemoPlayer player)
    {
        TextMeshProUGUI childText = null;

        // Itera pelos filhos diretos do objeto que contém esse script
        foreach (Transform child in transform)
        {
            if (child.name == player.Id)
            {
                // Encontra o primeiro filho com o nome correspondente e obtém o TextMeshProUGUI
                childText = child.GetComponent<TextMeshProUGUI>();
                break;
            }
        }

        if (childText != null)
        {
            // Atualizar o texto
            childText.text = $"Objective : {player.ObjectiveFraction.numerator}/{player.ObjectiveFraction.denominator}"; 
        }
        else
        {
            Debug.LogWarning("Filho com o nome " + player.Id + " não encontrado diretamente sob " + gameObject.name);
        }
    }

}
