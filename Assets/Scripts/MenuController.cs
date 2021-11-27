using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;

public class MenuController : MonoBehaviour
{
    public static MenuController Instance { get; private set; }
    private const string HIGHSCORE_FILE = "highscore.json";

    private int m_Highscore1;
    private int m_Highscore2;
    private int m_Highscore3;
    private string m_HighscoreName1;
    private string m_HighscoreName2;
    private string m_HighscoreName3;

    [SerializeField] private TextMeshProUGUI inputField;
    [SerializeField] private TextMeshProUGUI highscoreText1;
    [SerializeField] private TextMeshProUGUI highscoreText2;
    [SerializeField] private TextMeshProUGUI highscoreText3;
    public string PlayerName { get; private set; }
    public int Highscore
    {
        get
        {
            return m_Highscore1;
        }
    }
    public string HighscoreName
    {
        get
        {
            return m_HighscoreName1;
        }
    }
    
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

        LoadHighscores();
    }

    public void StartNew()
    {
        PlayerName = inputField.text;
        SceneManager.LoadScene(1);
    }

    public void NewScore(int score)
    {
        if (score > m_Highscore1)
        {
            m_Highscore3 = m_Highscore2;
            m_Highscore2 = m_Highscore1;
            m_Highscore1 = score;

            m_HighscoreName3 = m_HighscoreName2;
            m_HighscoreName2 = m_HighscoreName1;
            m_HighscoreName1 = PlayerName;
            SaveHighscores();
        }
        else if (score > m_Highscore2)
        {
            m_Highscore3 = m_Highscore2;
            m_Highscore2 = score;

            m_HighscoreName3 = m_HighscoreName2;
            m_HighscoreName2 = PlayerName;
            SaveHighscores();
        }
        else if (score > m_Highscore3)
        {
            m_Highscore3 = score;
            m_HighscoreName3 = PlayerName;
            SaveHighscores();
        }

        UpdateHighscoreTexts();
    }

    void UpdateHighscoreTexts()
    {
        highscoreText1.text = $"{m_HighscoreName1}\t{m_Highscore1}";
        highscoreText2.text = $"{m_HighscoreName2}\t{m_Highscore2}";
        highscoreText3.text = $"{m_HighscoreName3}\t{m_Highscore3}";
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void LoadHighscores()
    {
        string path = Path.Combine(Application.persistentDataPath, HIGHSCORE_FILE);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            HighscoreData data = JsonUtility.FromJson<HighscoreData>(json);

            m_Highscore1 = data.Highscore1;
            m_HighscoreName1 = data.Playername1;
            m_Highscore2 = data.Highscore2;
            m_HighscoreName2 = data.Playername2;
            m_Highscore3 = data.Highscore3;
            m_HighscoreName3 = data.Playername3;
        }

        UpdateHighscoreTexts();
    }

    void SaveHighscores()
    {
        HighscoreData data = new HighscoreData();
        data.Highscore1 = m_Highscore1;
        data.Playername1 = m_HighscoreName1;
        data.Highscore2 = m_Highscore2;
        data.Playername2 = m_HighscoreName2;
        data.Highscore3 = m_Highscore3;
        data.Playername3 = m_HighscoreName3;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Path.Combine(Application.persistentDataPath, HIGHSCORE_FILE), json);
    }

    private class HighscoreData
    {
        public int Highscore1;
        public string Playername1;
        public int Highscore2;
        public string Playername2;
        public int Highscore3;
        public string Playername3;
    }
}
