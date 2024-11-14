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
        

        // Gera os pesos principais que somam o peso objetivo
        for (int i = 0; i < numRocks; i++)
        {
            if(i == numRocks - 1 || remaningWeight == 1){
                weights.Add(remaningWeight);
                break;
            }

            int maxWeight = remaningWeight; // Garante que o peso final será alcançado
            int rockValor = random.Next(1, maxWeight + 1);
            weights.Add(rockValor);
            remaningWeight -= rockValor;
            if(remaningWeight == 0)
                break;
            
        }
        
       
        while (weights.Count < numRocks)
        {
            int extraWeight = random.Next(1, objectiveWeight); // Gera pesos aleatórios menores que o peso objetivo
            weights.Add(extraWeight);
        }

        // Gera as pedras extras que não fazem parte da combinação
        

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
