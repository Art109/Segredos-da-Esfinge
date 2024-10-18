using UnityEngine;

public class AcessarFilhos : MonoBehaviour
{
    [SerializeField] GameObject teste;

    void Start()
    {
        // Acessando um filho por nome
        Transform filhoPorNome = teste.transform.Find("Quadrante1");

        if (filhoPorNome != null)
        {
            Debug.Log("Filho por nome: " + filhoPorNome.name);
        }
        else
        {
            Debug.Log("Filho com o nome 'Quadrante1' não encontrado.");
        }

        // Acessando o primeiro filho por índice
        if (teste.transform.childCount > 0)
        {
            Transform primeiroFilho = teste.transform.GetChild(0);
            Debug.Log("Primeiro Filho: " + primeiroFilho.name);
        }
        else
        {
            Debug.Log("O objeto 'teste' não tem filhos.");
        }

        // Iterando sobre todos os filhos
        for (int i = 0; i < teste.transform.childCount; i++)
        {
            Transform filho = teste.transform.GetChild(i);
            Debug.Log("Nome do filho: " + filho.name);
        }
    }
}


