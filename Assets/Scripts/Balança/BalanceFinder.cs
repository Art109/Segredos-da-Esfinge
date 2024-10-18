using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BalanceFinder : MonoBehaviour
{
    // Script para pegar referência das balanças.
    // Nesta lista, as posições 0 e 1 são da primeira e segunda sala, respectivamente.
    // As posições 2, 3, 4 e 5 são as balanças da última fase, onde correspondem
    // aos jogadores 1, 2, 3 e 4 respectivamente.
    [SerializeField] private List<Balance> balances;

    public List<Balance> Balances { get => balances; set => balances = value; }
}
