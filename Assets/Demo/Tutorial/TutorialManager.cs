using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks.Sources;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public static event TaskComplete OnTaskComplete;
    public delegate void TaskComplete(String task);

    static String  currentTask = "Task1";
    public static String CurrentTask{get{return currentTask;}}

    [SerializeField] GameObject balance;
    [SerializeField] GameObject abacus;
    [SerializeField] GameObject[] task2Rocks;
    [SerializeField] GameObject rockPrefab;
    [SerializeField] Transform[] task3RockPositions;

    [SerializeField] TextMeshProUGUI[] uiText;
    [TextArea]
    [SerializeField]String task2Explanation;
    [TextArea]
    [SerializeField]String task2Objective;
    [TextArea]
    [SerializeField]String task3Explanation;
    [TextArea]
    [SerializeField]String task3ExplanationErro;
    [TextArea]
    [SerializeField]String task3Objective;
    [TextArea]
    [SerializeField]String taskFinalExplanation;
    [TextArea]
    [SerializeField]String taskFinalObjective;


    void OnEnable()
    {
        OnTaskComplete += TaskCompletion;
    }

    void OnDisable()
    {
        OnTaskComplete -= TaskCompletion;
    }
    
    void Update()
    {
        if(currentTask == "TaskFinal")
        {
            if(Input.GetKeyDown(KeyCode.Escape)){
                SceneManager.LoadScene("Menu");
            }
        }
    }

    void TaskCompletion(String task)
    {
        if(task == "Task1")
            Task2RockInteraction();
        if(task == "Task2")
            Task3WeightObjective();
        if(task == "Task3Erro")
            Task3Erro();
        if(task == "Task3")
            TaskFinal();
    }

    void Task2RockInteraction(){
        currentTask = "Task2";
        balance.SetActive(true);
        foreach(var rock in task2Rocks)
            rock.SetActive(true);
        StartCoroutine(TypeText(uiText[0],task2Explanation));
        StartCoroutine(TypeText(uiText[1],task2Objective));
    }

    void Task3WeightObjective()
    {
        currentTask = "Task3";
        abacus.SetActive(true);
        balance.GetComponent<TutorialBalance>().setMaxWeight(20);
        foreach(var rock in task2Rocks)
            rock.SetActive(false);

        int[] weights = {20, 10 , 15, 10 , 10};
        foreach(var position in task3RockPositions)
        {
            GameObject rock = Instantiate(rockPrefab,position.position,Quaternion.identity);
            DemoRock rockComponent = rock.GetComponent<DemoRock>();
            rockComponent.setWeight(weights[UnityEngine.Random.Range(0,weights.Length)]);
        }

        StartCoroutine(TypeText(uiText[0],task3Explanation));
        StartCoroutine(TypeText(uiText[1],task3Objective));
    }

    void Task3Erro(){
        StartCoroutine(TypeText(uiText[0],task3ExplanationErro));
    }

    void TaskFinal(){
        currentTask = "TaskFinal";
        StartCoroutine(TypeText(uiText[0],taskFinalExplanation));
        StartCoroutine(TypeText(uiText[1],taskFinalObjective));
    }



    public float typingSpeed = 1f;
    private IEnumerator TypeText(TextMeshProUGUI ui, String text)
    {
        String currentText;
        for (int i = 0; i <= text.Length; i++)
        {
            currentText = text.Substring(0, i); // Atualiza o texto exibido
            ui.text = currentText; // Define o texto no componente
            yield return new WaitForSeconds(typingSpeed); // Aguarda o tempo de digitação
        }
    }


    

    public static void TaskCompleteTrigger(String task)
    {
        OnTaskComplete.Invoke(task);
    }
   
}
