using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform cameraPositionS2;
    [SerializeField] private Transform cameraPositionS3;

    public void follow(Piramide piramide)
    {
        if(piramide.SalaAtual == piramide.ListSalas[1])
        {
            // Sala 2
            gameObject.transform.position = cameraPositionS2.position;
        }
        else
        {
            // Sala 3
            gameObject.transform.position = cameraPositionS3.position;
        }
    }
}
