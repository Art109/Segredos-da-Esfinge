using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Balance : MonoBehaviour
{
    private Dictionary<Player, List<Rock>> playerRocks; // Dicionário para rastrear pedras por jogador
    private Dictionary<Player, float> playerWeights; // Dicionário para rastrear o peso por jogador

    [SerializeField] private int pesoMaximo;
    public int PesoMaximo { get { return pesoMaximo; } }

    private GameManager gameManager;

    // Efeitos sonoros da balança
    //public AudioClip errorSound;

    // Variável de peso atual (peso total na balança)
    [SerializeField] private float pesoAtual;

    [SerializeField] GameObject feedback;
    [SerializeField] LayerMask rockLayer;

    SpriteRenderer spriteRenderer;

    //public AudioSource balanceAudioSource;

    [SerializeField] private GameObject textoPesoMaximoPrefab;
    private GameObject textoPesoMaximoObj;

    private LevelFade levelfade;

    private int quantidade; // Variável para contar quantos jogadores atingiram seus pesos objetivos

    void Start()
    {
        playerRocks = new Dictionary<Player, List<Rock>>();
        playerWeights = new Dictionary<Player, float>();
        gameManager = FindObjectOfType<GameManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        levelfade = FindObjectOfType<LevelFade>();
        mostrarPesoBalanca();
    }

    void mostrarPesoBalanca()
    {
        textoPesoMaximoObj = Instantiate(textoPesoMaximoPrefab, transform.position + Vector3.forward * 10.0f, Quaternion.identity);
        TextMeshPro pesoBalanca = textoPesoMaximoObj.GetComponent<TextMeshPro>();
        pesoBalanca.text = pesoMaximo.ToString("F2") + "kg";
    }

    public void TakeRock(Rock rock, Player player)
    {
        if (!playerRocks.ContainsKey(player))
        {
            playerRocks[player] = new List<Rock>();
        }
        playerRocks[player].Add(rock);
        AtualizaPesoTotal();
    }

    public void RemoveRock(Rock rock, Player player)
    {
        if (playerRocks.ContainsKey(player) && playerRocks[player].Contains(rock))
        {
            playerRocks[player].Remove(rock);
            AtualizaPesoTotal();
        }
    }

    void AtualizaPesoTotal()
    {
        pesoAtual = 0; // Reinicia o peso total antes de recalcular
        quantidade = 0; // Reinicia a quantidade de jogadores que atingiram o peso objetivo

        foreach (var player in playerRocks.Keys)
        {
            float pesoJogador = 0;

            foreach (Rock rock in playerRocks[player])
            {
                pesoJogador += rock.Peso;
            }

            playerWeights[player] = pesoJogador; // Atualiza o peso individual do jogador
            pesoAtual += pesoJogador; // Adiciona o peso do jogador ao peso total

            // Verifica se o jogador atingiu seu peso objetivo
            if (Mathf.Approximately(playerWeights[player], player.PesoObjetivo))
            {
                quantidade++; // Incrementa a quantidade se o jogador atingiu o peso objetivo
            }
        }

        Debug.Log("Peso total atualizado -> " + pesoAtual);
        Debug.Log("Quantidade de jogadores que atingiram o peso objetivo -> " + quantidade);

        // Realiza a verificação de armadilha e feedback apenas uma vez
        ArmadilhaTrigger();
        Feedback();
    }

    void ArmadilhaTrigger()
    {
        // Verifica se o peso total na balança excede o peso máximo
        if (pesoAtual > pesoMaximo)
        {
            // Para as salas 1 e 2, o dano é causado em todos os jogadores
            if (gameManager.Piramide.SalaAtual.Numero < 3)
            {
                gameManager.causarDanoNosJogadores();
            }
            // Na terceira sala, o dano é causado apenas ao jogador específico
            else
            {
                // Aqui, verificamos qual jogador tem mais peso e causamos dano somente nele
                foreach (var player in playerWeights.Keys)
                {
                    gameManager.causarDanoNoJogador(player);
                }
            }
            //playErrorSound();
        }
    }


    void Feedback()
    {
        // Fase 3
        if (gameManager.Piramide.SalaAtual.Numero == 3)
        {
            if (Mathf.Approximately(pesoAtual, pesoMaximo))
            {
                spriteRenderer.color = Color.green;

                foreach (var player in playerRocks.Keys)
                {
                    player.CanTake = false;
                    gameManager.PontuarPlayer(player, gameManager.Piramide.SalaAtual.Pontuacao);
                }
            }
        }
        // Outras fases
        else
        {
            if (quantidade == 4)
            {
                // Pontuar todos os jogadores apenas se todos atingiram os pesos objetivos
                gameManager.PontuarPlayers(gameManager.Piramide.SalaAtual.Pontuacao);
            }
        }
    }

    /*void playErrorSound()
    {
        balanceAudioSource.clip = errorSound;
        balanceAudioSource.Play();
    }*/
}
