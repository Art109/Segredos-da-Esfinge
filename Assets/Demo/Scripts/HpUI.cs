using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpUI : MonoBehaviour
{
    [SerializeField] Sprite heartEmpty;
    int lifeIndex = 3;

    void OnEnable(){
        DemoPlayer.OnPlayerDamaged += UpdateUI;
    }

    void OnDisable(){
        DemoPlayer.OnPlayerDamaged += UpdateUI;
    }


    void UpdateUI(String playerId)
    {
        if(playerId == gameObject.name)
        {
            Image childImage = null;

            // Itera pelos filhos diretos do objeto que contém esse script
            foreach (Transform child in transform)
            {
                if (child.name == $"H{lifeIndex}")
                {
                    // Encontra o primeiro filho com o nome correspondente e obtém o TextMeshProUGUI
                    childImage = child.GetComponent<Image>();
                    break;
                }
            }

            if (childImage != null)
            {
                // Atualizar o sprite
                childImage.sprite = heartEmpty; 
                lifeIndex--;
            }
            else
            {
                Debug.LogWarning("Filho com o nome H" + playerId + " não encontrado diretamente sob " + gameObject.name);
            }
        }
    }
}
    

