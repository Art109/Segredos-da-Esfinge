using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFade : MonoBehaviour
{
    public Animator animator;

    private GameManager gameManager;

    private AudioSource audioSource;
    public AudioClip transitionSound;


    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = transitionSound;
        animator = GetComponent<Animator>();
    }
    public void fadeToNextLevel()
    {
        animator.Play("LevelFade");
    }

    private void playTransitionSound()
    {
        audioSource.PlayOneShot(transitionSound);
    }

    public void chamaMudarSala()
    {
        gameManager.mudarSala();
    }

    public void BackState()
    {
        animator.SetTrigger("backState");
    }

    void Update()
    {
        //if (UnityEngine.Input.GetKeyDown("i"))
        //{
            //LevelFade.Trigger(this);
        //}
    }
}
