using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text HighScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private string m_Playername;
    private int m_Points;
    
    private bool m_GameOver = false;

    void Start()
    {
        // load highscore
        //LoadHighscore();
        UpdateHighscoreText();

        if (MenuController.Instance != null)
        {
            m_Playername = MenuController.Instance.PlayerName;
        }

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"{m_Playername} score : {m_Points}";
    }

    void UpdateHighscoreText()
    {
        if (MenuController.Instance != null)
        {
            HighScoreText.text = $"Best score : {MenuController.Instance.HighscoreName} : {MenuController.Instance.Highscore}";
        }
    }

    public void GameOver()
    {
        m_GameOver = true;

        if (MenuController.Instance != null)
        {
            MenuController.Instance.NewScore(m_Points);
        }

        UpdateHighscoreText();

        GameOverText.SetActive(true);
    }

    /*
    void LoadHighscore()
    {
        if (MenuController.Instance != null)
        {
            MenuController.Instance.LoadHighscores();
        }
    }
    */
}
