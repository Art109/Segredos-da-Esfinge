using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoRock : MonoBehaviour
{
    [SerializeField]int weight;
    public int Weight{get {return weight;}}
    [SerializeField] DataStorage sprites;
    SpriteRenderer spriteRenderer;

    [SerializeField] AudioSource rockAudioSource;
    [SerializeField] Audio_Storage audioStorage;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    public void FollowPlayer(DemoPlayer player){

        if(this == null)
            return;

        if(player != null)
            transform.position = player.transform.position;
        else
            return;
    }

    void UpdateSprite(){
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                Debug.LogError("SpriteRenderer não encontrado no GameObject.");
                return;
            }
        }

        if (sprites == null || sprites ==  null)
        {
            Debug.LogError("DataStorage 'sprites' não está atribuído ou está vazio.");
            return;
        }

        if (weight <= 5)
        {
            spriteRenderer.sprite = sprites.Sprites[0];
        }
        else if (weight > 5 && weight <= 10)
        {
            spriteRenderer.sprite = sprites.Sprites[1];
        }
        else if (weight > 10 && weight <= 20)
        {
            spriteRenderer.sprite = sprites.Sprites[2];
        }else if(weight > 20 && weight <= 30)
        {
            spriteRenderer.sprite = sprites.Sprites[3];
        }else if(weight > 30)
        {
            spriteRenderer.sprite = sprites.Sprites[4];
        }
    }

    public void setWeight(int weight){
        this.weight = weight;
        UpdateSprite();
    }

    public void PlayAudioClip(string clipName)
    {
        AudioClip clip = audioStorage.GetAudioClipByName(clipName);
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, Vector3.zero);
        }
    }


}
