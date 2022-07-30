using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToLobby : MonoBehaviour
{
    public GameObject donatePanel;
    public GameObject moneyText;
    public GameObject[] otherObjects;

    public void Back()
    {
        if (donatePanel.activeSelf == false) SceneManager.LoadScene("MainLobby");
        else
        {
            donatePanel.SetActive(false);
            moneyText.SetActive(true);
            for (int i = 0; i < otherObjects.Length; i++)
            {
                otherObjects[i].SetActive(true);
            }
        }   
    }

    public void BackFromLevels()
    {
        SceneManager.LoadScene("MainLobby");
    }
    public void OpenDonateMenu()
    {
        donatePanel.SetActive(true);
        moneyText.SetActive(false);
        for (int i = 0; i < otherObjects.Length; i++)
        {
            otherObjects[i].SetActive(false);
        }
    }
}
