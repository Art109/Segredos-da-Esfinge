using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{

    [SerializeField] float peso;
    SpriteRenderer spriteRenderer;
    [SerializeField] DataStorage sprites;

    public AudioSource rockAudioSource;
    public AudioClip groundPickupSound;
    public AudioClip groundDropSound;
    public AudioClip balancePickupSound;
    public AudioClip balanceDropSound;

    public float Peso { get { return peso; } }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        AtualizaSprite();
    }

    public void playGroundPickupSound()
    {
        rockAudioSource.clip = groundPickupSound;
        rockAudioSource.Play();
    }
    public void playGroundDropSound()
    {
        if (rockAudioSource != null && rockAudioSource.isActiveAndEnabled)
        {
            rockAudioSource.clip = groundDropSound;
            rockAudioSource.Play();
        }
    }

    public void playBalancePickupSound()
    {
        rockAudioSource.clip = balancePickupSound;
        rockAudioSource.Play();
    }
    public void playBalanceDropSound()
    {
        rockAudioSource.clip = balanceDropSound;
        rockAudioSource.Play();
    }

    public void FollowPlayer(Player player)
    {
        if (player == null || this == null || gameObject == null)
        {
            return;
        }

        gameObject.transform.position = player.transform.position;
        //Debug.Log("Seguindo o player");
    }


    void AtualizaSprite(){
        if (peso <= 5)
        {
            spriteRenderer.sprite = sprites.Sprites[0];
        }
        else if (peso > 5 && peso <= 10)
        {
            spriteRenderer.sprite = sprites.Sprites[1];
        }
        else if (peso > 10 && peso <= 20)
        {
            spriteRenderer.sprite = sprites.Sprites[2];
        }else if(peso > 20 && peso <= 30)
        {
            spriteRenderer.sprite = sprites.Sprites[3];
        }else if(peso > 30)
        {
            spriteRenderer.sprite = sprites.Sprites[4];
        }
    }
    public void setPeso(int peso)
    {
        this.peso = peso;
        AtualizaSprite();
    }
}