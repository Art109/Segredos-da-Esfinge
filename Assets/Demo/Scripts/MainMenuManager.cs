using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void PlayDemo()
    {
        SceneManager.LoadScene("Demo");
    }

    public void PlayTutorial(){
        SceneManager.LoadScene("Tutorial");
    }
}
