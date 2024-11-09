using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class DemoFeedBack : MonoBehaviour
{
    [SerializeField]GameObject feedBackGameObject;
    SpriteRenderer spriteRenderer;
    [SerializeField] FeedBack_DataStorage storage;
    Sprite sprite;
    void OnEnable()
    {
        DemoPlayer.OnFeedBackTrigger += FeedBackManager;
    }
    void OnDisable()
    {
        DemoPlayer.OnFeedBackTrigger -= FeedBackManager;
    }

    void Start(){
        spriteRenderer = feedBackGameObject.GetComponent<SpriteRenderer>();

        
    }

    void FeedBackManager(String name, DemoPlayer player){
        
        if(GetComponent<DemoPlayer>() == player){
            sprite = storage.GetFeedBackByName(name);
            StartCoroutine(FeedBackTrigger(sprite));
        }
        
    }

    IEnumerator FeedBackTrigger(Sprite sprite){
    
    feedBackGameObject.SetActive(true);
    spriteRenderer.sprite = sprite;
    yield return new WaitForSeconds(3f);
    feedBackGameObject.SetActive(false);


    }
}
