using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    private Piramide piramide;
    public Piramide Piramide { get => piramide; }

    //Player Management
    private List<Player> players;
    public List<Player> Players { get => players; }
    private PlayerInitializer playerInitializer;

    private ContaTempo contaTempo;

    private CameraFollow cameraFollow;

    [SerializeField] private GameObject paredeNoMeio;

    [SerializeField] private List<Transform> posicoesSalas;

    [SerializeField] private GameObject spawnPedra;
    private List<Transform> posicoesPedra;

    private GeradorPedras geradorPedras;

    private int indiceSalaAtual = 0;

    private int quantidadeDeJogadoresQueAcabaram = 0;

    [Tooltip("Lista para manter somente uma sala ativa por vez")]
    [SerializeField] private List<GameObject> objetosDasSalas;
    private GameObject canvasFade;
    private Animator canvasAnimator;

    [HideInInspector] public LevelFade levelfade;

    // Vida
    public UnityEngine.UI.Image[] P1Hearts;
    public UnityEngine.UI.Image[] P2Hearts;
    //public UnityEngine.UI.Image[] P3Hearts;
    //public UnityEngine.UI.Image[] P4Hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    // Pesos
    public TextMeshProUGUI[] playerPesoTexts;

    //Audio
    private AudioSource audioSource;
    public AudioClip hurtSound;

    void Awake()
    {
        piramide = FindObjectOfType<Piramide>();
        piramide.iniciarSalas();
        geradorPedras = GetComponent<GeradorPedras>();
        levelfade = FindObjectOfType<LevelFade>();
    }
    void Start()
    {
        contaTempo = GetComponent<ContaTempo>();
        cameraFollow = FindObjectOfType<CameraFollow>();
        players = new List<Player>();
        playerInitializer = gameObject.GetComponent<PlayerInitializer>();
        playerInitializer.IniatlizaPlayers(players);
        Sortear_Peso_Jogadores(piramide);
        desativarSalasRestantesEAtivarSalaAtual();

        canvasFade = GameObject.Find("Canvas_fade");
        if(canvasFade != null)
        {
            canvasAnimator = canvasFade.GetComponent<Animator>();
        }

        UpdatePlayerTexts();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = hurtSound;
    }

    public void UpdatePlayerTexts()
    {
        for (int i = 0; i < playerPesoTexts.Length; i++)
        {
            
            playerPesoTexts[i].text = players[i].FracaoPesoObjetivo.numerador + " / " + players[i].FracaoPesoObjetivo.denominador;
        }
    }

    void Update()
    {
        if (UnityEngine.Input.GetKeyDown("p"))
        {
            causarDanoNoJogador(players[0]);
        }
    }

    private UnityEngine.UI.Image[] GetHeartsForPlayer(Player player)
    {
        // Get player number or identifier
        string playerNum = player.getPlayerNum();
        
        switch (playerNum) 
        {
            case "P1":
                return P1Hearts;
            case "P2":
                return P2Hearts;
           /* case "P3":
                return P3Hearts;
            case "P4":
                return P4Hearts;
            */
            default:
                Debug.LogError("Unknown player number: " + playerNum);
                return null;
        }
    }

    public void UpdateHealth(Player player)
    {
        UnityEngine.UI.Image[] hearts = GetHeartsForPlayer(player);

        if (hearts != null)
        {
            for (int i = 0; i < hearts.Length; i++)
            {
                hearts[i].sprite = (i < player.Vida) ? fullHeart : emptyHeart;
            }
        }
    }

    // Pegando os players para usar na vida
    public List<Player> ReturnPlayersList()
    {
        return players;
    }

    void Sortear_Peso_Jogadores(Piramide piramide)
    {
        // Fazendo mudança para pegar o peso certo da balança atual
        if (piramide.SalaAtual.Numero < 3)
        {
            // A sala 1 e 2 só tem uma balança na lista, então o índice é 0
            ObjectiveGenerator.DividirPeso(piramide.SalaAtual.Balances[0].PesoMaximo, players);
        }
        else
        {
            // Última fase
            // O peso objetivo de cada player será o peso máximo de cada balança
            GeradorPesoUltimaSala.GerarPesos(piramide, players);
        }
        GerarPedras();
    }

    void GerarPedras()
    {
        Transform salaSpawn = spawnPedra.transform.GetChild(indiceSalaAtual);
        Transform quadranteSpawn;
        List<Transform> pedrasPositions = new List<Transform>();

        for (int i = 0; i < players.Count; i++)  // Percorre os jogadores
        {
            // Montando o nome do objeto de posição correspondente ao jogador
            string nomePosicao = "Quadrante" + players[i].name;
            quadranteSpawn = salaSpawn.Find(nomePosicao);

            if (quadranteSpawn != null)
            {
                for (int j = 0; j < quadranteSpawn.childCount; j++)
                {
                    pedrasPositions.Add(quadranteSpawn.GetChild(j));
                }

                geradorPedras.InstanciarPedras(players[i].PesoObjetivo, pedrasPositions);
                pedrasPositions.Clear();  // Limpa a lista para o próximo jogador
            }
            else
            {
                Debug.LogWarning("Quadrante não encontrado para o jogador: " + players[i].name);
            }
        }
    }


    public void PontuarPlayers(int pontos)
    {
        //Debug.Log("Entrei na fun��o de pontuar");
        for (int i = 0; i < players.Count; i++)
        {
            piramide.SalaAtual.pontuarJogador(players[i]);
            PontosParaUI.colocarPontoNaUi(players[i]);
        }
        levelfade.fadeToNextLevel();
    }

    public void PontuarPlayer(Player player, int pontos)
    {
        piramide.SalaAtual.pontuarJogador(player);
        PontosParaUI.colocarPontoNaUi(player);
        verificaSeTodosAcabaramAUltimaFase();
    }

    private void DestroyHearts(Player player)
    {
        UnityEngine.UI.Image[] hearts = GetHeartsForPlayer(player);
        
        foreach (var heart in hearts)
        {
            if (heart != null)
            {
                heart.enabled = false; // Disable the Image component
            }
        }
    }

    public void causarDanoNosJogadores()
    {
        // Causa dano em todos os jogadores
        for (int i = players.Count - 1; i >= 0; i--)
        {
            audioSource.PlayOneShot(hurtSound);
            StartCoroutine(players[i].Blink());

            players[i].Vida -= 1;
            if (players[i].Vida == 0)
            {
                DestroyHearts(players[i]);
                Destroy(players[i].gameObject);
                players.RemoveAt(i);  // Removendo o jogador da lista
            }
        }

        if (players.Count != 0)
        {
            resetarTempoSala();
        }
    }

    public void causarDanoNoJogador(Player player)
    {
        audioSource.PlayOneShot(hurtSound);
        StartCoroutine(player.Blink());
        
        player.Vida -= 1;
        if (player.Vida == 0)
        {
            DestroyHearts(player);
            Destroy(player.gameObject);
            players.Remove(player);
        }
    }

    public void resetarTempoSala()
    {
        contaTempo.Tempo = piramide.SalaAtual.TempoParaConclusao;
    }

    public void desativarParedeNoMeio()
    {
        paredeNoMeio.SetActive(false);
    }

    public void mudarSala()
    {
        indiceSalaAtual += 1;
        piramide.trocarSala(indiceSalaAtual);
        cameraFollow.follow(piramide);
        resetarTempoSala();
        limparPedras();
        PlayerTPManager.TeleportarPlayers(piramide, players);
        Sortear_Peso_Jogadores(piramide);
        desativarSalasRestantesEAtivarSalaAtual();
    }

    // Nome autoexplicativo
    public void verificaSeTodosAcabaramAUltimaFase()
    {
        quantidadeDeJogadoresQueAcabaram = 0;
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].CanTake == false)
            {
                quantidadeDeJogadoresQueAcabaram++;
                Debug.Log("Quantidade = " + quantidadeDeJogadoresQueAcabaram);
            }
        }

        if (quantidadeDeJogadoresQueAcabaram == players.Count)
        {
            SceneManager.LoadScene("FimMinigame");
        }
    }

    private void limparPedras()
    {
        List<DemoRock> pedras = new List<DemoRock>(FindObjectsOfType<DemoRock>());

        foreach (DemoRock rock in pedras) { 
            Destroy(rock.gameObject);
        }
    }

    private void desativarSalasRestantesEAtivarSalaAtual()
    {
        for (int i = 0; i < objetosDasSalas.Count; i++)
        {
            objetosDasSalas[i].SetActive(false);
        }

        int salaAtualIndice = piramide.SalaAtual.Numero - 1;  
        if (salaAtualIndice >= 0 && salaAtualIndice < objetosDasSalas.Count)
        {
            objetosDasSalas[salaAtualIndice].SetActive(true);
        }
    }

}