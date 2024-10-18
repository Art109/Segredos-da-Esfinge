using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BalanceFinder : MonoBehaviour
{
    // Script para pegar refer�ncia das balan�as.
    // Nesta lista, as posi��es 0 e 1 s�o da primeira e segunda sala, respectivamente.
    // As posi��es 2, 3, 4 e 5 s�o as balan�as da �ltima fase, onde correspondem
    // aos jogadores 1, 2, 3 e 4 respectivamente.
    [SerializeField] private List<Balance> balances;

    public List<Balance> Balances { get => balances; set => balances = value; }
}
