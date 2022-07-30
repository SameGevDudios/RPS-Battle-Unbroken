using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelCount : MonoBehaviour
{
    public GameObject[] levelButtons;
    public int choosenLevel;
    public bool calculate;

    void Start()
    {
        if (calculate)
        {
            if (!PlayerPrefs.HasKey("wonLevel"))
            {
                PlayerPrefs.SetInt("wonLevel", 1);
            }

            for (int i = 0; i < levelButtons.Length; i++)
            {
                levelButtons[i].GetComponentInChildren<Text>().text = (i + 1).ToString();
                if(i+1 > PlayerPrefs.GetInt("wonLevel"))
                {
                    levelButtons[i].GetComponent<Button>().interactable = false;
                }
            }
        }
    }
    
    void Update()
    {
        if(Input.GetKey(KeyCode.Alpha2))
        {
            PlayerPrefs.SetInt("wonLevel", 1);
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            PlayerPrefs.SetInt("wonLevel", 49);
        }
    }

    public void StartLevel()
    {
        choosenLevel = int.Parse(GetComponentInChildren<Text>().text);
        PlayerPrefs.SetInt("difficulty", choosenLevel);
        SceneManager.LoadScene("Campaign");
    }
}
