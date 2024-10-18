using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ContaTempo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textoTempo;
    private float tempo;

    private GameManager gameManager;

    public float Tempo { get => tempo; set => tempo = value; }

    void Start()
    {
        gameManager = GetComponent<GameManager>();
        tempo = gameManager.Piramide.SalaAtual.TempoParaConclusao;
    }

    // Update is called once per frame
    void Update()
    {
        if (tempo > 0)
        {
            tempo -= Time.deltaTime;
            if (tempo <= 0)
            {
                gameManager.causarDanoNosJogadores();

            }
            AtualizarTextoTempo();
        }
    }

    void AtualizarTextoTempo()
    {
        // Formatar o tempo para mostrar apenas segundos inteiros
        textoTempo.text = Mathf.Ceil(Tempo).ToString();
    }
}
