using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class RockGenerator : MonoBehaviour
{
    private System.Random random = new System.Random();
    [SerializeField] GameObject rockPrefab;
    List<GameObject> rocks = new List<GameObject>();

     void OnEnable(){
        Room.OnRoomEndend += DestroyRocks;
       
    }

    void OnDisable(){
        Room.OnRoomEndend -= DestroyRocks;
    }

    // Gera uma lista de pesos que somam o peso objetivo com combinações distintas
    private List<int> GenerateRocks(int objectiveWeight, int numRocks)
    {
        List<int> weights = new List<int>();
        int remaningWeight = objectiveWeight;
        int extraRocksNumber = Random.Range(0,4);

        // Gera os pesos principais que somam o peso objetivo
        for (int i = 0; i < numRocks - extraRocksNumber - 1; i++)
        {
            int maxWeight = remaningWeight - (numRocks - i - 1); // Garante que o peso final será alcançado
            int rockValor = random.Next(1, maxWeight);
            weights.Add(rockValor);
            remaningWeight -= rockValor;
        }
        
        weights.Add(remaningWeight); // Adiciona o peso restante para completar o peso objetivo

        // Gera as pedras extras que não fazem parte da combinação
        for (int i = 0; i < extraRocksNumber; i++)
        {
            int extraWeight = random.Next(1, objectiveWeight); // Peso aleatório menor que o objetivo
            weights.Add(extraWeight);
        }

        // Embaralha a lista para misturar as pedras extras com as pedras válidas
        return weights.OrderBy(x => random.Next()).ToList(); 
    }

    // Instancia as pedras com pesos gerados e configura cada uma
    public void InstatiateRocks(int objectiveWeight, List<Transform> positions)
    {
        int numRocks = positions.Count;
        List<int> pesos = GenerateRocks(objectiveWeight, numRocks );

        for (int i = 0; i < numRocks; i++)
        {
            GameObject rock = Instantiate(rockPrefab, positions[i].position, Quaternion.identity);
            DemoRock rockScript = rock.GetComponent<DemoRock>();
            if (rockScript != null)
            {
                rockScript.setWeight(pesos[i]);
                rocks.Add(rock);
            }

            //Debug.Log($"Pedra {i + 1}: Peso {pesos[i]}");
        }

        
    }

    void DestroyRocks(){
        foreach (var rock in rocks)
        {
            Destroy(rock);
        }
    }

    
}
