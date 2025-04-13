using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SManager : MonoBehaviour
{
    public int actualScene;
    public TMP_InputField levelInputField;
    EditorController ec;

    public TriggerGoal[] goals;
    private bool canWin = false;

    // Start is called before the first frame update
    void Start()
    {
        ec = FindAnyObjectByType<EditorController>();
        StartCoroutine(InitActualLevel());
        StartCoroutine(Check());
    }
    private void Update()
    {
        if(canWin) CheckWinCondition();
        RestartLevel();
    }

    public void RestartLevel()
    {
        if (Input.GetKey("r"))
        {
            StartCoroutine(RestartScene());
        }
    }

    private IEnumerator RestartScene()
    {
        canWin = false;
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(actualScene-1);
    }
    private IEnumerator InitActualLevel()
    {
        levelInputField.text = actualScene.ToString();
        yield return new WaitForSeconds(0.2f);
        ec.ConfirmLevel();
        ec.LoadLevelInit();
    }

    public IEnumerator Check()
    {
        yield return new WaitForSeconds(1);
        goals = FindObjectsOfType<TriggerGoal>();
        canWin = true;
    }
    void CheckWinCondition()
    {
        bool levelIsCorrect = true;

        foreach (TriggerGoal goal in goals)
        {
            if (!goal.hasBox)
            {
                levelIsCorrect = false;
                break;  
            }
        }       
        if (levelIsCorrect) LoadNextLevel();
    }
    void LoadNextLevel()
    {
        Debug.Log("Pasamos a la siguiente escena");
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene + 1);
    }
}
