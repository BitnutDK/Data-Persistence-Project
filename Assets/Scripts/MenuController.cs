using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuController : MonoBehaviour
{
    public static MenuController Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI inputField;
    public string PlayerName { get; private set; }
    
    void Awake()
    {
        // if instance already set, destroy this new game object and return
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        // set instance
        Instance = this;
        // persist game object
        DontDestroyOnLoad(gameObject);
    }

    public void StartNew()
    {
        PlayerName = inputField.text;
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
