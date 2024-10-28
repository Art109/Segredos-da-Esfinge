using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using UnityEngine;

public class DemoPlayer : MonoBehaviour
{
    [Header("Atributos do jogador")]
    [SerializeField]float moveSpeed;
    [SerializeField]float pontos;
    [SerializeField]int vida;
    Vector2 input;
    Rigidbody2D rb;
    Animator animator;

    [Header("Atributos da Interação")]
    [SerializeField]LayerMask interactableLayer;
    [SerializeField]float interactableRange;
    bool carryingRock;
    Rock rockCarried;

    public AudioSource somPegadas;


    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Interaction();
    }

    void playSomPegadas(bool boolean)
    {
        somPegadas.pitch = moveSpeed + 0.5f;
        somPegadas.enabled = boolean;
    }

    void Movement(){
        float moveX = Input.GetAxis("HorizontalP1");
        float moveY = Input.GetAxis("VerticalP1");

        input = new Vector2(moveX, moveY);

        rb.velocity = input * moveSpeed;

        if(rb.velocity != Vector2.zero){
            animator.SetFloat("x", moveX);
            animator.SetFloat("y", moveY);
            animator.SetBool("Moving", true);
            playSomPegadas(true);
        }
        else{
            playSomPegadas(false);
            animator.SetBool("Moving", false);
        }

        


    }

     void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, interactableRange);
    }

    void Interaction(){

        Collider2D[] interactables = Physics2D.OverlapCircleAll(transform.position, interactableRange, interactableLayer);

        if(interactables.Length != 0){  

            Rock_Interaction(interactables);

        }
        
    }

    void Rock_Interaction(Collider2D[] interactables){
        if(carryingRock){
            // Código para quando o jogador está carregando uma pedra
            Balance_Interaction(interactables);
            Abacus_Interaction(interactables);
        }
        else{
            Rock rockNearby = null;  // Declaração de rockNearby fora do bloco else
            foreach(Collider2D rock in interactables){
                if(rock.GetComponent<Rock>() != null){
                    rockNearby = rock.GetComponent<Rock>();
                    break;
                }
            }

            if(rockNearby != null){
                if(Input.GetKeyDown(KeyCode.F))
                    rockCarried = rockNearby;
            }
        }
    }

    void Balance_Interaction(Collider2D[] interactables){

    }

    void Abacus_Interaction(Collider2D[] interactables){

    }
}
