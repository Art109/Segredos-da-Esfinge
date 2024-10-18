using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feedback : MonoBehaviour
{
    // Start is called before the first frame update
    SpriteRenderer spriteRenderer;
    string nome;
    public string Nome{set{nome = value;}}
    [SerializeField]DataStorage sprites;


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Destroy(gameObject, 0.5f);    
    }

    // Update is called once per frame
    void Update()
    {
        if(spriteRenderer.sprite == null)
            UpdateSprite();
    }

    void UpdateSprite(){
        if(nome == "Perfeito"){
            //spriteRenderer.sprite = sprites[1];
        }
    }
}
