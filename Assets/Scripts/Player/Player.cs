using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Atributos do jogador")]
    [SerializeField] private float baseMovementSpeed = 0f;
    private float currentMovementSpeed;

    // Fisica
    private Rigidbody2D rb;

    // Vetores
    private Vector2 input;
    private Vector2 lastMoveDirection;

    // Identificador jogador
    protected string playerNum;

    // Propriedades
    public Vector2 Input { get => input; set => input = value; }
    public Vector2 LastMoveDirection { get => lastMoveDirection; set => lastMoveDirection = value; }
    public int PontuacaoAtual { get => pontuacaoAtual; set => pontuacaoAtual = value; }
    [SerializeField] public int Vida { get => vida; set => vida = value; }
    public bool CanTake { get => canTake; set => canTake = value; }

    [Header("Atributos da Interação")]
    [SerializeField] LayerMask rockLayer;
    [SerializeField] LayerMask balanceLayer;
    [SerializeField] LayerMask abacoLayer;
    bool carryingRock = false;
    Rock rockCarried;
    bool canDeposit = false;

    // booleano para ver se pode colocar no abaco
    bool podeColocarNoAbaco = false;

    private bool canTake = true;
    [SerializeField] int pesoObjetivo = 0;
    public int PesoObjetivo { get => pesoObjetivo; set => pesoObjetivo = value; }
    (int numerador, int denominador) fracaoPesoObjetivo;
    public (int numerador, int denominador) FracaoPesoObjetivo { get; set; }

    // Audio
    public AudioSource somPegadas;

    // Pontuação
    [SerializeField] private int pontuacaoAtual;

    // Vida
    [SerializeField] private int vida = 3;

    [SerializeField] private float range = 0f;

    // Game manager
    private GameManager gameManager;


    public float blinkDuration = 1.0f; // tempo de blinks
    public float blinkInterval = 0.1f; // tempo entre blinks
    private Renderer characterRenderer;

    void playSomPegadas(bool boolean)
    {
        somPegadas.pitch = currentMovementSpeed + 0.5f;
        somPegadas.enabled = boolean;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerNum = gameObject.name;
        currentMovementSpeed = baseMovementSpeed;
        gameManager = FindObjectOfType<GameManager>();
        //Debug.Log(playerNum);

        characterRenderer = GetComponent<Renderer>();
        //Debug.Log(pesoObjetivo);
    }

    public IEnumerator Blink()
    {
        float endTime = Time.time + blinkDuration;

        while (Time.time < endTime)
        {
            if(characterRenderer != null)
                characterRenderer.enabled = !characterRenderer.enabled;
            yield return new WaitForSeconds(blinkInterval);
        }

        // Ensure the renderer is enabled at the end of the blinking
        if(characterRenderer != null)
            characterRenderer.enabled = true;
    }

    public String getPlayerNum()
    {
        return playerNum;
    }

    private void Update()
    {
        ProcessInputs();
        Move();
        Rock_Interaction();
        gameManager.UpdateHealth(this);
    }

    private void Move()
    {
        rb.velocity = input * currentMovementSpeed;
        if (rb.velocity.x != 0 || rb.velocity.y != 0)
            playSomPegadas(true);
        else
            playSomPegadas(false);
    }

    public void ProcessInputs()
    {
        // Função Process Inputs
        float moveX = UnityEngine.Input.GetAxisRaw("Horizontal" + playerNum);
        float moveY = UnityEngine.Input.GetAxisRaw("Vertical" + playerNum);

        // Cria um vetor com os valores de input
        input = new Vector2(moveX, moveY);

        // Normaliza o vetor de input se houver movimento
        if (input.magnitude > 0.1f)
        {
            input.Normalize();
            lastMoveDirection = input;
        }
        else
        {
            input = Vector2.zero;
        }
    }

    void Rock_Interaction()
    {
        Collider2D balance = Physics2D.OverlapCircle(transform.position, range, balanceLayer);
        Collider2D abaco = Physics2D.OverlapCircle(transform.position, range, abacoLayer);

        if (carryingRock)
        {
            rockCarried.FollowPlayer(this);

            if (currentMovementSpeed == baseMovementSpeed)
            {
                if (rockCarried.Peso <= 5)
                {
                    currentMovementSpeed = baseMovementSpeed;
                }
                else if (rockCarried.Peso > 5 && rockCarried.Peso <= 10)
                {
                    currentMovementSpeed -= 1;
                }
                else if (rockCarried.Peso > 10 && rockCarried.Peso <= 20)
                {
                    currentMovementSpeed -= 2;
                } else if (rockCarried.Peso > 20 && rockCarried.Peso < 30)
                {
                    currentMovementSpeed -= 3;
                } else if (rockCarried.Peso > 30)
                {
                    currentMovementSpeed -= 4;
                }
            }

            if (balance != null)
            {
                canDeposit = true;
            }
            else
            {
                canDeposit = false;
            }

            if (abaco != null)
            {
                podeColocarNoAbaco = true;
            }
            else
            {
                podeColocarNoAbaco = false;
            }

            if (UnityEngine.Input.GetButtonDown("PegarPedra" + playerNum))
            {
                if (canDeposit)
                {
                    rockCarried.playBalanceDropSound();
                    Balance balanceComponent = balance.GetComponent<Balance>();
                    if (balanceComponent != null)
                    {
                        balanceComponent.TakeRock(rockCarried, this);
                    }
                }

                if (podeColocarNoAbaco)
                {
                    rockCarried.playGroundDropSound();
                    Abaco abacoComponent = abaco.GetComponent<Abaco>();
                    if (abacoComponent != null)
                    {
                        abacoComponent.TakeRock(rockCarried, this);
                    }
                }
                else
                {
                    //rockCarried.playGroundDropSound();
                }
                
                currentMovementSpeed = baseMovementSpeed;
                if (canDeposit == false && podeColocarNoAbaco == false)
                    rockCarried.playGroundDropSound();
                carryingRock = false;
                rockCarried = null;
            }
        }
        else
        {
            Collider2D[] rocks = Physics2D.OverlapCircleAll(transform.position, range, rockLayer);
            if (rocks.Length > 0)
            {
                if (UnityEngine.Input.GetButtonDown("PegarPedra" + playerNum) && canTake)
                {
                    Debug.Log("PlayerNum: " + playerNum);

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
                        rockCarried = closestRock.GetComponent<Rock>();
                        rockCarried.playGroundPickupSound();

                        // Remover referência da pedra da balança, caso esteja lá
                        foreach (var balanceComponent in FindObjectsOfType<Balance>())
                        {
                            balanceComponent.RemoveRock(rockCarried, this);
                        }

                        // Remover referência da pedra do ábaco, caso esteja lá
                        foreach (var abacoComponent in FindObjectsOfType<Abaco>())
                        {
                            abacoComponent.RemoveRock(rockCarried, this);
                        }

                        carryingRock = true;
                    }
                }
            }
        }
    }
}
