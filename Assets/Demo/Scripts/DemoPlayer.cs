using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class DemoPlayer : MonoBehaviour
{
    [Header("Atributos do jogador")]
    [SerializeField]float baseSpeed;
    [SerializeField]float currentSpeed;
    [SerializeField]float points;
    [SerializeField]int life = 3;
    bool isAlive = true;
    public bool IsAlive{get{return isAlive;}}
    [SerializeField]int objectiveWeight;
    public int ObjectiveWeight{ get{ return objectiveWeight;} set{ objectiveWeight = value; }}
    (int numerator, int denominator) objectiveFraction;
    public (int numerator, int denominator) ObjectiveFraction { get{return objectiveFraction;} set{objectiveFraction = value;} }

    Vector2 input;
    Rigidbody2D rb;
    Animator animator;

    [Header("Atributos da Interação")]
    [SerializeField]LayerMask balanceLayer,abacusLayer;
    [SerializeField]LayerMask rockLayer;
    [SerializeField]float interactableRange;
    bool carryingRock;
    DemoRock rockCarried;
    bool completedObjective = false;
    public bool CompletedObjective{  get{ return completedObjective; }}

    public AudioSource audioSource;

    public static event PlayerDeathAlert OnPlayerDeath;
    public delegate void PlayerDeathAlert(DemoPlayer player);

    void OnEnable(){
        DemoBalance.OnPlayerCompletion += PlayerCompletion;
        Room.OnRoomStarted += StartGame;
    }

    void OnDisable(){
        DemoBalance.OnPlayerCompletion -= PlayerCompletion;
        Room.OnRoomStarted -= StartGame;
    }
    

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
        if(isAlive)
        {
            Movement();
            if(!completedObjective)
                Interaction();
        }
        
    }

    void playSomPegadas(bool boolean)
    {
        audioSource.pitch = currentSpeed + 0.5f;
        audioSource.enabled = boolean;
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

        Collider2D balance = Physics2D.OverlapCircle(transform.position, interactableRange, balanceLayer);
        Collider2D abacus = Physics2D.OverlapCircle(transform.position, interactableRange, abacusLayer);
        
        if (carryingRock)
        {
           
            Debug.Log("Estou na interação com pedra");
            rockCarried.FollowPlayer(this);

            if (Input.GetKeyDown(KeyCode.F))
            {
                

                if (balance != null)
                {
                    Debug.Log("Tem uma balança proxima");
                    rockCarried.PlayAudioClip("Rock_Balance_Drop");
                    RockReceiver rockReceiverComponent = balance.GetComponent<RockReceiver>();
                    if (rockReceiverComponent != null)
                    {
                        rockReceiverComponent.TakeRock(this, rockCarried);
                    }
                }
                else if(abacus != null)
                {
                    Debug.Log("Tem uma abaco proxima");
                    rockCarried.PlayAudioClip("Rock_Balance_Drop");
                    RockReceiver rockReceiverComponent = abacus.GetComponent<RockReceiver>();
                    if (rockReceiverComponent != null)
                    {
                        rockReceiverComponent.TakeRock(this, rockCarried);
                    }
                    
                }
                else{
                    rockCarried.PlayAudioClip("Rock_Ground_Drop");
                }
                
                
                carryingRock = false;
                rockCarried = null;
                UpdateSpeed();
            }
        }
        else
        {
            Debug.Log("Estou na interação sem pedra");
            Collider2D[] rocks = Physics2D.OverlapCircleAll(transform.position, interactableRange, rockLayer);
            if (rocks.Length > 0)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {


                    Collider2D closestRock = null;
                    float shortestDistance = Mathf.Infinity;

                    // Encontrar a pedra mais próxima
                    foreach (Collider2D rockCollider in rocks)
                    {
                        float distanceToRock = Vector2.Distance(transform.position, rockCollider.transform.position);
                        if (distanceToRock < shortestDistance)
                        {
                            shortestDistance = distanceToRock;
                            closestRock = rockCollider;
                        }
                    }

                    if (closestRock != null)
                    {
                        rockCarried = closestRock.GetComponent<DemoRock>();
                        rockCarried.PlayAudioClip("Rock_Ground_PickUp");
                        carryingRock = true;

                        if(abacus != null )
                            abacus.GetComponent<RockReceiver>().RemoveRock(this,rockCarried);
                        if(balance != null )
                            balance.GetComponent<RockReceiver>().RemoveRock(this,rockCarried);

                        
                    }
                }
            }
        }
        
    }

    void StartGame(){
        completedObjective = false;
    }
    void PlayerCompletion(DemoPlayer player){
        if(player == this)
            completedObjective = true;
    }
        
    public void ApplyDamage(int damage){
        life -= damage;
        if(life <= 0)
            Die();
    }

    void Die(){
        isAlive = false;
        OnPlayerDeath.Invoke(this);
    }
}



    

