using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    [SerializeField] private GameObject cameraPlayer;
    [SerializeField] private GameObject cameraCenario;

    public void SwitchToCameraCenario()
    {
        cameraCenario.SetActive(true);
        cameraPlayer.SetActive(false);
    }

    public void SwitchToCameraPlayer()
    {
        cameraCenario.SetActive(false);
        cameraPlayer.SetActive(true);
    }
}
