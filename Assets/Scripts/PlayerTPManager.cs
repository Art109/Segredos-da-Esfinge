using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTPManager
{
    public static void TeleportarPlayers(Piramide piramide, List<Player> players)
    {
        for (int i = 0; i < players.Count; i++)
        {
            // Pegando o número da sala atual da piramida
            int numeroDaSala = piramide.SalaAtual.Numero;

            // Pegando o player
            string playerName = players[i].name;

            // Montando o nome do objeto de posição correspondente
            string nomePosicao = "Position " + playerName;

            GameObject parentObject = GameObject.Find("Sala" + numeroDaSala);
            Transform childTransform = parentObject.transform.Find(nomePosicao);

            players[i].transform.position = childTransform.position;
        }
    }
}

