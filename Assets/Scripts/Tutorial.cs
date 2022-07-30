using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    int language;
    int step;
    public Text text;
    public string[] words;
    public GameObject[] langButtons;


    void Awake()
    {
        if (PlayerPrefs.HasKey("firstTime"))
        {
            Destroy(gameObject);
        } 

        if(SceneManager.GetActiveScene().name != "MainLobby")
        {
            if (PlayerPrefs.HasKey("step"))
            {
                step = PlayerPrefs.GetInt("step");
                Destroy(langButtons[0]);
                Destroy(langButtons[1]);
            }
            if (step != 0)
            {
                text.text = words[step];
            }
        }
    }

    public void ChooseRus()
    {
        language = 0;
        step = -1;

        for (int i = 0; i < langButtons.Length; i++)
        {
            langButtons[i].SetActive(!langButtons[i].activeSelf);
        }

        NextStep();
    }
    public void ChooseEng()
    {
        language = 1;
        step = words.Length / 2;
        text.text = words[step];

        for (int i = 0; i < langButtons.Length; i++)
        {
            langButtons[i].SetActive(!langButtons[i].activeSelf);
        }
    }

    public void NextStep()
    {
        step++;
        if (step == words.Length / 2 || step == words.Length)
        {
            Destroy(gameObject);
            PlayerPrefs.SetInt("firstTime", 1);
        }
        else if (step != words.Length / 2 || step != words.Length)
        {
            text.text = words[step];
            PlayerPrefs.SetInt("step", step);
            if (step == 1 || step == 7)
            {
                SceneManager.LoadScene("CampaignLevels");
            }
            if (step == 3 || step == 9)
            {
                SceneManager.LoadScene("Campaign");
            }
        }
        

    }

    public void SkipTut()
    {
        Destroy(gameObject);
        PlayerPrefs.SetInt("firstTime", 1);
    }

}
