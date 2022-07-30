using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainLobby : MonoBehaviour
{
    public GameObject wipObject;
    public Text scoreText;
    int score;
    float wipTimer;

    void Start()
    {
        score = PlayerPrefs.GetInt("score");
        scoreText.text += score.ToString();
    }

    void Update()
    {
        if(wipTimer > 0)
        {
            wipTimer -= Time.deltaTime;
        }
        else
        {
            if (wipObject.activeSelf)
            {
                wipObject.SetActive(false);
            }
        }
        if (Input.GetKey(KeyCode.Alpha5))
        {
            PlayerPrefs.DeleteAll();
        }
    }

    public void StartCampaign()
    {
        SceneManager.LoadScene("CampaignLevels");
    }

    public void StartFastGame()
    {
        SceneManager.LoadScene("FastGame");
    }

    public void StartOnline()
    {

    }

    public void Shop()
    {
        SceneManager.LoadScene("Shop");
    }

    public void WorkInProgress()
    {
        wipObject.SetActive(true);
        wipTimer = 3;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
