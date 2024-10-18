using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;
    private Player player;

    private void Start()
    {
        anim = GetComponent<Animator>();
        player = GetComponent<Player>();

        if (player == null)
        {
            Debug.LogError("Player não encontrado na cena!");
        }
    }

    private void Update()
    {
        Animate();
    }

    private void Animate()
    {
        anim.SetFloat("MoveX", player.Input.x);
        anim.SetFloat("MoveY", player.Input.y);
        anim.SetFloat("MoveMagnitude", player.Input.magnitude);
        anim.SetFloat("LastMoveX", player.LastMoveDirection.x);
        anim.SetFloat("LastMoveY", player.LastMoveDirection.y);
    }
}
