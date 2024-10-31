using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using UnityEngine;

public class DemoPlayer : MonoBehaviour
{
    [Header("Atributos do jogador")]
    [SerializeField]float baseSpeed;
    [SerializeField]float currentSpeed;
    [SerializeField]float points;
    [SerializeField]int life;
    Vector2 input;
    Rigidbody2D rb;
    Animator animator;

    [Header("Atributos da Interação")]
    [SerializeField]LayerMask interactableLayer;
    [SerializeField]float interactableRange;
    bool carryingRock;
    DemoRock rockCarried;

    public AudioSource somPegadas;


    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentSpeed = baseSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Interaction();
    }

    void playSomPegadas(bool boolean)
    {
        somPegadas.pitch = currentSpeed + 0.5f;
        somPegadas.enabled = boolean;
    }

    void Movement(){
        float moveX = Input.GetAxis("HorizontalP1");
        float moveY = Input.GetAxis("VerticalP1");

        input = new Vector2(moveX, moveY);

        rb.velocity = input * currentSpeed;

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

    void UpdateSpeed(){
        if(rockCarried != null){
            if(rockCarried.Weight > 40){
                currentSpeed = baseSpeed * 0.5f;
                Debug.Log("Reduzir a velocidade");
            }
                
            else if(rockCarried.Weight > 20)
                currentSpeed = baseSpeed * 0.8f;
            
        }
        else{
            currentSpeed = baseSpeed;
        }
    }

    void Interaction(){

        Collider2D[] interactables = Physics2D.OverlapCircleAll(transform.position, interactableRange, interactableLayer);


        RockReceiver_Interaction(interactables);
        
        Rock_Interaction(interactables);

        if(interactables.Length != 0){  
            
            Debug.Log("Tem Algo Proximo");

        }
        
    }

    void Rock_Interaction(Collider2D[] interactables){
        if(carryingRock){
            
            rockCarried.FollowPlayer(this);

            
            if(Input.GetKeyDown(KeyCode.F)){

                    rockCarried.PlayAudioClip("Rock_Ground_Drop");
                    rockCarried = null;
                    carryingRock = false;
                    UpdateSpeed();
                    Debug.Log("Soltei uma pedra");
            }

            
        }
        else{
            DemoRock rockNearby = null;  
            foreach(Collider2D rock in interactables){
                if(rock.GetComponent<DemoRock>() != null){
                    rockNearby = rock.GetComponent<DemoRock>();
                    break;
                }
            }

            if(rockNearby != null){
                if(Input.GetKeyDown(KeyCode.F)){
                    rockCarried = rockNearby;
                    carryingRock = true;
                    rockCarried.PlayAudioClip("Rock_Ground_PickUp");
                    UpdateSpeed();
                    
                    Debug.Log("Peguei uma pedra");
                }
                
            }
        }
    }

    void RockReceiver_Interaction(Collider2D[] interactables){

        RockReceiver rockReceiverNearby = null;
        foreach(Collider2D rockReceiver in interactables){
            if(rockReceiver.GetComponent<RockReceiver>() != null){
                rockReceiverNearby = rockReceiver.GetComponent<DemoBalance>();
                break;
            }
        }

        if(rockReceiverNearby != null){
            if(carryingRock){
                if(Input.GetKeyDown(KeyCode.F)){
                    rockReceiverNearby.TakeRock(this,rockCarried);
                    rockCarried = null;
                    carryingRock = false;
                }
            }
            else{
                if(Input.GetKeyDown(KeyCode.F)){
                    rockReceiverNearby.RemoveRock(this, rockCarried);
                }
            }
        }

    }

    
}
