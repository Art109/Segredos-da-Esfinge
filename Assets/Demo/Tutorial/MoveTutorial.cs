using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveTutorial : MonoBehaviour
{
    
    
    
    void OnTriggerEnter2D(Collider2D collider)
    {
        TutorialManager.TaskCompleteTrigger("Task1");
        Debug.Log("Completei o tutorial 1");
        Destroy(gameObject);
    }
}
