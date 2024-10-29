using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Abaco : MonoBehaviour
{
    private Rock pedraAtual; // Refer�ncia � pedra atualmente no �baco
    private Player jogadorAtual; // Refer�ncia ao jogador atual que colocou a pedra

    // Vari�vel de peso atual
    [SerializeField] private float pesoAtual;

    [SerializeField] LayerMask rockLayer;

    SpriteRenderer spriteRenderer;

    // Prefab do texto para mostrar o peso
    [SerializeField] private GameObject pesoTextoPrefab;

    private GameObject pesoTextoObj; // Refer�ncia ao objeto de texto atualmente exibido

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        pedraAtual = null; // Nenhuma pedra no in�cio
        jogadorAtual = null; // Nenhum jogador associado no in�cio
    }

    public void TakeRock(Rock rock, Player player)
    {
        if (pedraAtual == null)
        {
            pedraAtual = rock;
            jogadorAtual = player;
            pesoAtual = rock.Peso;
            MostrarPesoAtual();
            spriteRenderer.color = Color.blue;
        }
        else
        {
            Debug.Log("J� existe uma pedra no �baco. Remova-a antes de adicionar outra.");
        }
    }

    public void RemoveRock(Rock rock, Player player)
    {
        if (pedraAtual == rock && jogadorAtual == player)
        {
            // Remove a pedra e o jogador associado
            pedraAtual = null;
            jogadorAtual = null;
            pesoAtual = 0f;
            spriteRenderer.color = Color.red;

            // Destr�i o objeto de texto quando a pedra � removida
            if (pesoTextoObj != null)
            {
                Destroy(pesoTextoObj);
            }
        }
    }

    void MostrarPesoAtual()
    {
        // Destroi o objeto de texto anterior, se existir
        if (pesoTextoObj != null)
        {
            Destroy(pesoTextoObj);
        }

        // Instancia o novo texto na posi��o do �baco
        pesoTextoObj = Instantiate(pesoTextoPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);

        TextMeshPro pesoTexto = pesoTextoObj.GetComponent<TextMeshPro>();

        // Define o texto com o peso atual
        pesoTexto.text = pesoAtual.ToString("F2") + " kg";

        // Inicia a corrotina para animar o texto
        StartCoroutine(AnimarTexto(pesoTextoObj));
    }

    IEnumerator AnimarTexto(GameObject pesoTextoObj)
    {
        float duration = 0.3f; // Dura��o da anima��o
        Vector3 startPosition = pesoTextoObj.transform.position;
        Vector3 endPosition = startPosition + new Vector3(0, 0.4f, 0); 

        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            if(pesoTextoObj != null)
                pesoTextoObj.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
