using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Advertisements;


public class GameManager : MonoBehaviour
{
    public Slider[] scoreSliders;
    public Slider speedSlider;
    public SpriteRenderer backscreen;
    public Color[] colors;
    SpawnUnits su;
    public GameObject[] buttons;
    public GameObject pauseMenu;
    public Text timerText;
    public Text[] infoText;
    public Text[] moneyText;
    public Text scoreText;
    public float maxUnits;
    public float rocks, papers, scissors;
    float timer;
    public bool isOver;
    bool isWon;
    public bool started;
    public bool isFastGame;
    public int money;
    int s, m, h;
    int timeBonus;
    int score;
    string currentScene;
    public AudioClip[] ac;
    public GameObject winPanel, losePanel;
    public GameObject[] FastgameWinPanels;
    [SerializeField] bool ifNoAds = false;

    public GameObject wheel;
    Rigidbody2D wrb;
    bool stopped;
    public Text spinText;
    Button spinButton;
    int moneyBonus;
    public int chosenTeam;

    public Vector2 rSpawn, pSpawn, sSpawn, offset;

    [SerializeField] private GameObject easterEgg;
    void Awake()
    {
        if (!PlayerPrefs.HasKey("money")) PlayerPrefs.SetInt("money", 0);

        if (!PlayerPrefs.HasKey("speed")) PlayerPrefs.SetFloat("speed", 1f);

        if (!PlayerPrefs.HasKey("startUnits")) PlayerPrefs.SetInt("startUnits", 0);

        if (!PlayerPrefs.HasKey("battleUnits")) PlayerPrefs.SetInt("battleUnits", 0);

        if (!PlayerPrefs.HasKey("speedPrice")) PlayerPrefs.SetInt("speedPrice", 150);
    }


    void Start()
    {

        if (PlayerPrefs.HasKey("ifNoAds"))
        {

            if (PlayerPrefs.GetInt("ifNoAds") >= 1)
            {
                ifNoAds = true;
                print("1");
            }
            else
            {
                ifNoAds = false;
                print("2");
            }

        }
        else
        {
            ifNoAds = false;
            print("3");
        }

        if (PlayerPrefs.HasKey("ifNoAds"))
        {

            if (PlayerPrefs.GetInt("ifNoAds") >= 1)
            {
                ifNoAds = true;
            }
            else
            {
                ifNoAds = false;
            }
        }

        CheckAdds();
        su = GetComponent<SpawnUnits>();
        RefreshSpawnPos();
        started = false;
        Time.timeScale = 1;
        int rand = Random.Range(0, colors.Length - 1);
        backscreen.color = colors[rand];
        currentScene = SceneManager.GetActiveScene().name;
        

        if (!isFastGame)
        {
            speedSlider.minValue = .1f;
            speedSlider.maxValue = PlayerPrefs.GetFloat("speed");
            speedSlider.value = 1;
            speedSlider.gameObject.SetActive(false);
            spinButton = spinText.GetComponentInParent<Button>();
        }
    }

    void Update()
    {
        if (!started)
        {
            rocks = GameObject.FindGameObjectsWithTag("rock").Length;
            papers = GameObject.FindGameObjectsWithTag("paper").Length;
            scissors = GameObject.FindGameObjectsWithTag("scissors").Length;
        }

        infoText[0].text = rocks.ToString();
        infoText[1].text = papers.ToString();
        infoText[2].text = scissors.ToString();

        scoreSliders[0].value = Mathf.Lerp(scoreSliders[0].value, rocks, 4 * Time.deltaTime);
        scoreSliders[1].value = Mathf.Lerp(scoreSliders[1].value, papers, 4 * Time.deltaTime);
        scoreSliders[2].value = Mathf.Lerp(scoreSliders[2].value, scissors, 4 * Time.deltaTime);

        for (int i = 0; i < scoreSliders.Length; i++)
        {
            scoreSliders[i].maxValue = maxUnits;
        }

        if (started)
        {
            if (isFastGame)
            {
                if (rocks == maxUnits || papers == maxUnits || scissors == maxUnits)
                {
                    if (!isOver) Win();
                }
            }
            else
            {
                if (!isOver)
                {
                    if (rocks == maxUnits)
                    {
                        if (su.chosenTeam == 1) Win();
                        else Lose();
                    }
                    if (papers == maxUnits)
                    {
                        if (su.chosenTeam == 2) Win();
                        else Lose();
                    }
                    if (scissors == maxUnits)
                    {
                        if (su.chosenTeam == 3) Win();
                        else Lose();
                    }
                }

            }

            if (rocks == 0 || papers == 0 || scissors == 0)
            {
                if (!isOver) Time.timeScale = 3;
                else Time.timeScale = 1;
            }

            timer += Time.deltaTime;
            m = (int)timer / 60;
            s = (int)timer - m * 60;
            h = (int)timer / 3600;
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            AutoWin();
        }
        if (Input.GetKey(KeyCode.Alpha1))
        {
            PlayerPrefs.DeleteKey("speed");
        }
        if(!stopped)
        {
            if (wrb != null)
            {
                if (wrb.angularVelocity == 0)
                {
                    CalculateRotation();
                }
            }
        }
    }

    void Win()
    {
        GameObject.FindGameObjectWithTag("music").GetComponent<AudioSource>().volume = 0;

        isOver = true;
        isWon = true;
        GetComponent<AudioSource>().PlayOneShot(ac[0]);
        if (!isFastGame)
        {
            PlayerPrefs.SetInt("difficulty", PlayerPrefs.GetInt("difficulty") + 1);
            if (PlayerPrefs.GetInt("difficulty") > PlayerPrefs.GetInt("wonLevel"))
            {
                PlayerPrefs.SetInt("wonLevel", PlayerPrefs.GetInt("wonLevel") + 1);

                if (m < 2) timeBonus = (180 - m * 2 - s) / 10;
                money = (int)maxUnits + timeBonus;
                moneyText[0].text = "+" + money;

                score = money * 10;

                if (PlayerPrefs.HasKey("money"))
                    PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") + money);
                else
                    PlayerPrefs.SetInt("money", money);

                if (PlayerPrefs.HasKey("score"))
                    PlayerPrefs.SetInt("score", PlayerPrefs.GetInt("score") + score);
                else
                    PlayerPrefs.SetInt("score", score);

                scoreText.text = "You win! Your score: " + score;
            }
            else
            {
                money = 5;
                PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") + money);
            }

        }

        Unit[] unit = FindObjectsOfType<Unit>();
        for (int i = 0; i < unit.Length; i++)
        {
            unit[i].speed = 0;
            unit[i].isGameStarted = false;
        }

        if (!isFastGame)
        {
            winPanel.SetActive(true);
        }
        else
        {
            if (rocks == maxUnits)
            {
                FastgameWinPanels[0].SetActive(true);
            }
            if (papers == maxUnits)
            {
                FastgameWinPanels[1].SetActive(true);
            }
            if (scissors == maxUnits)
            {
                FastgameWinPanels[2].SetActive(true);
            }
        }

        
    }
    void Lose()
    {
        GameObject.FindGameObjectWithTag("music").GetComponent<AudioSource>().volume = 0;

        isOver = true;
        GetComponent<AudioSource>().PlayOneShot(ac[1]);
        losePanel.SetActive(true);

        if (!isFastGame)
        {
            money = 5;
            moneyText[1].text = "+" + money;
            PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") + money);
        }
    }
    public void ToLevelList()
    {
        if (isFastGame) SceneManager.LoadScene("MainLobby");
        else SceneManager.LoadScene("CampaignLevels");
    }
    public void Restart()
    {
        if (isFastGame) SceneManager.LoadScene("FastGame");
        else SceneManager.LoadScene("Campaign");
    }
    public void LoadNextLevel()
    {
        if (!ifNoAds)
        {
            //AddWithSkip();
        }
        else if (ifNoAds)
        {
            if (!isFastGame)
            {
                SceneManager.LoadScene("Campaign");
            }
        }
    }

    public void StartMatch()
    {
        if (isFastGame)
        {
            maxUnits = su.slider.value * 3;
            for (int i = 0; i < scoreSliders.Length; i++)
            {
                scoreSliders[i].maxValue = su.slider.value * 3;
            }
        }
        else
        {
            speedSlider.gameObject.SetActive(true);
            maxUnits = rocks + papers + scissors;

            if(PlayerPrefs.GetInt("difficulty") == 5)
            {
                GameObject canvas = FindObjectOfType<Canvas>().gameObject;
                GameObject egg = Instantiate(easterEgg, easterEgg.transform.position, easterEgg.transform.rotation, canvas.transform);
                egg.transform.position = new Vector2(-265.1f, 75.4f);
            }
        }

        Unit[] unit = FindObjectsOfType<Unit>();
        for (int i = 0; i < unit.Length; i++)
        {
            unit[i].isGameStarted = true;
        }
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].SetActive(false);
        }

        started = true;

    }
    public void RefRSpawn()
    {
        rSpawn = new Vector2(Random.Range(-3.1f, 3.1f), Random.Range(-4.3f, 4.3f));
    }
    public void RefPSpawn()
    {
        pSpawn = new Vector2(Random.Range(-3.1f, 3.1f), Random.Range(-4.3f, 4.3f));
    }
    public void RefSSpawn()
    {
        sSpawn = new Vector2(Random.Range(-3.1f, 3.1f), Random.Range(-4.3f, 4.3f));
    }

    public void RefreshSpawnPos()
    {
        if (!isFastGame)
        {
            rSpawn = new Vector2(Random.Range(-3.1f, 3.1f), Random.Range(-4.3f, 4.3f));
            pSpawn = new Vector2(Random.Range(-3.1f, 3.1f), Random.Range(-4.3f, 4.3f));
            sSpawn = new Vector2(Random.Range(-3.1f, 3.1f), Random.Range(-4.3f, 4.3f));
        }
        else
        {
            rSpawn = new Vector2(Random.Range(-3.1f, 3.1f), Random.Range(-4.3f, 4.3f));
            pSpawn = new Vector2(Random.Range(-3.1f, 3.1f), Random.Range(-4.3f, 4.3f));
            sSpawn = new Vector2(Random.Range(-3.1f, 3.1f), Random.Range(-4.3f, 4.3f));
        }

    }
    public void ChangeSpeed()
    {
        Unit[] unit = FindObjectsOfType<Unit>();
        for (int i = 0; i < unit.Length; i++)
        {
            unit[i].speed = 1;
        }

        if (chosenTeam == 1)
        {
            GameObject[] unitObj = GameObject.FindGameObjectsWithTag("rock");
            for (int i = 0; i < unitObj.Length; i++)
            {
                unitObj[i].GetComponent<Unit>().speed = speedSlider.value;
            }
        }
        if (chosenTeam == 2)
        {
            GameObject[] unitObj = GameObject.FindGameObjectsWithTag("paper");
            for (int i = 0; i < unitObj.Length; i++)
            {
                unitObj[i].GetComponent<Unit>().speed = speedSlider.value;
            }
        }
        if (chosenTeam == 3)
        {
            GameObject[] unitObj = GameObject.FindGameObjectsWithTag("scissors");
            for (int i = 0; i < unitObj.Length; i++)
            {
                unitObj[i].GetComponent<Unit>().speed = speedSlider.value;
            }
        }
    }
    public void PauseMenu()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
    }

    public void BackToLobby()
    {
        SceneManager.LoadScene("MainLobby");
    }

   

    public void CheckAdds()
    {
        if (Advertisement.isSupported)
        {
            Advertisement.Initialize("4153929", false);
        }
    }

    [System.Obsolete]
    public void AddWithSkip()
    {

        if (Advertisement.IsReady())
        {
            Advertisement.Show("video", new ShowOptions
            {
                resultCallback = result =>
                {
                    if (result == ShowResult.Finished || result == ShowResult.Skipped)
                    {
                        if (!isFastGame)
                        {
                            SceneManager.LoadScene("Campaign");
                        }
                    }
                }
            });
        }
        else
        {
            if (!isFastGame)
            {
                SceneManager.LoadScene("Campaign");
            }
        }
    }

    public void AddWithBonus()
    {
        if (!stopped)
        {
            if (Advertisement.IsReady())
            {
                Advertisement.Show("rewardedVideo", new ShowOptions
                {
                    resultCallback = result =>
                    {
                        if (result == ShowResult.Finished)
                        {
                            if (!isFastGame)
                            {
                                Spin();

                            }
                        }
                    }
                });
            }
        }
    }

    void AutoWin()
    {
        Win();
        LoadNextLevel();
    }

    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Spin()
    {
        wrb = wheel.AddComponent<Rigidbody2D>();
        wrb.angularDrag = 1.5f;
        wrb.gravityScale = 0;
        wrb.angularVelocity = Random.Range(7000f, 13000f);
        spinText.gameObject.GetComponentInParent<Button>().interactable = false;
    }

    void CalculateRotation()
    {
        stopped = true;
        float rot = wheel.transform.rotation.eulerAngles.z;
        if (rot >= -31 && rot < 29)
        {
            moneyBonus += money * 2;
        }
        if (rot >= 29 && rot < 89)
        {
            moneyBonus += money * 5;
        }
        if (rot >= 89 && rot < 149)
        {
            moneyBonus += money;
        }
        if (rot >= 149 && rot < 209)
        {
            moneyBonus += money * 3;
        }
        if (rot >= 209 && rot < 269)
        {
            moneyBonus += money * 8;
        }
        if (rot >= 269 && rot < 329)
        {
            moneyBonus += money / 2;
        }
        if (rot >= 329 && rot < 389)
        {
            moneyBonus += money * 2;
        }
        moneyText[0].text = "+" + (money + moneyBonus);
        PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") + moneyBonus);
    }

}
