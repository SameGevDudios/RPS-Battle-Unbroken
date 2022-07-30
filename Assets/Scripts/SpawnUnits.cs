using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnUnits : MonoBehaviour
{
    GameManager gm;
    public Slider slider;
    public Text sliderText, unitsShopText;
    public GameObject r, p, s;
    public GameObject chooseTeamPanel;
    public GameObject[] unitType;
    public GameObject startButton;
    public GameObject[] obstacles;
    public GameObject[] obstacleSheets;
    public GameObject pauseMenu;
    public GameObject crate;
    public GameObject crateBulb;
    public int chosenTeam; //1.rocks 2.papers 3.scissors
    public int difficulty, myUnits;
    public bool timeToSpawn;
    public bool spawnCrate;
    public int seedOffset;
    Camera cam;


    void Awake()
    {
        difficulty = PlayerPrefs.GetInt("difficulty");
        Debug.Log(difficulty);
    }

    void Start()
    {
        gm = GetComponent<GameManager>();
        myUnits = 5 + PlayerPrefs.GetInt("startUnits");
        cam = GetComponent<Camera>();
        if (!gm.isFastGame)
        {
            unitsShopText.text = myUnits.ToString();
            startButton.SetActive(false);
            chooseTeamPanel.SetActive(true);
            Random.InitState(difficulty + seedOffset); //seed insertion
            SpawnObstacles();
        }

        if (!gm.isFastGame)
        {
            spawnCrate = true;
            CrateButton();
        }

    }
    
    void Update()
    {
        if (timeToSpawn && !gm.started)
        {
            SpawnUnit();
        }

        if (!gm.started)
        {
            if (gm.isFastGame)
            {
                sliderText.text = slider.value.ToString();
                if (gm.rocks < slider.value)
                {
                    Instantiate(r, gm.rSpawn, transform.rotation);
                }
                if (gm.rocks > slider.value)
                {
                    GameObject[] rock = GameObject.FindGameObjectsWithTag("rock");
                    Destroy(rock[rock.Length - 1]);
                }

                if (gm.papers < slider.value)
                {
                    Instantiate(p, gm.pSpawn, transform.rotation);
                }
                if (gm.papers > slider.value)
                {
                    GameObject[] papers = GameObject.FindGameObjectsWithTag("paper");
                    Destroy(papers[papers.Length - 1]);
                }

                if (gm.scissors < slider.value)
                {
                    Instantiate(s, gm.sSpawn, transform.rotation);
                }
                if (gm.scissors > slider.value)
                {
                    GameObject[] scissors = GameObject.FindGameObjectsWithTag("scissors");
                    Destroy(scissors[scissors.Length - 1]);
                }
            }
            else
            {

            }
        }
        else
        {
            if (spawnCrate && !gm.isOver) SpawnCrate();
        }
        
    }

    public void ChooseTeam(int team)
    {
        chosenTeam = team;
        gm.chosenTeam = team;
        chooseTeamPanel.SetActive(false);

        int unitsToSpawn = difficulty + 4;
        if(difficulty > 40)
        {
            unitsToSpawn -= 40;
        }

        gm.maxUnits = 5 + (unitsToSpawn) * 2;

        if (chosenTeam == 1)
        {
            for (int i = 0; i < unitsToSpawn; i++)
            {
                Instantiate(p, gm.pSpawn, transform.rotation);
                Instantiate(s, gm.sSpawn, transform.rotation);
            }
        }
        if (chosenTeam == 2)
        {
            for (int i = 0; i < unitsToSpawn; i++)
            {
                Instantiate(r, gm.rSpawn, transform.rotation);
                Instantiate(s, gm.sSpawn, transform.rotation);
            }
        }
        if (chosenTeam == 3)
        {
            for (int i = 0; i < unitsToSpawn; i++)
            {
                Instantiate(r, gm.rSpawn, transform.rotation);
                Instantiate(p, gm.pSpawn, transform.rotation);
            }
        }
        timeToSpawn = true;
    }

    void SpawnUnit()
    {
        if(myUnits > 0)
        {
            if (Input.GetMouseButtonDown(0) && pauseMenu.activeSelf == false)
            {
                Vector2 touchPos = cam.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));

                RaycastHit2D rayHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if(rayHit.collider == null)
                {
                    Instantiate(unitType[chosenTeam - 1], touchPos, Quaternion.identity);
                    myUnits--;
                    if (myUnits > 4)
                    {
                        startButton.SetActive(true);
                    }

                    if (chosenTeam == 1) gm.rocks++;
                    if (chosenTeam == 2) gm.papers++;
                    if (chosenTeam == 3) gm.scissors++;
                }
            }
        }
        else
        {
            startButton.SetActive(true);
            timeToSpawn = false;
        }
    }

    void SpawnObstacles()
    {
        if(difficulty < 41)
        {
            for (int i = 0; i < difficulty / 3; i++)
            {
                int obstacleNum = Random.Range(0, obstacles.Length);
                Vector2 pos = new Vector2(Random.Range(-3f, 3f), Random.Range(-4.3f, 4.3f));

                GameObject spawnedObstacles = Instantiate(obstacles[obstacleNum], pos, transform.rotation);
                spawnedObstacles.transform.Rotate(Vector3.forward * Random.Range(0, 360f));
            }
        }
        if(difficulty > 40)
        {
            int obsSheetNum = Random.Range(0, obstacleSheets.Length);
            Vector2 sheetPos = new Vector2(0, 0);
            Instantiate(obstacleSheets[obsSheetNum], sheetPos, transform.rotation);

            for (int i = 0; i < (difficulty - 41) / 10; i++)
            {
                int obstacleNum = Random.Range(0, obstacles.Length);
                Vector2 pos = new Vector2(Random.Range(-3f, 3f), Random.Range(-4.3f, 4.3f));

                GameObject spawnedObstacles = Instantiate(obstacles[obstacleNum], pos, transform.rotation);
                spawnedObstacles.transform.Rotate(Vector3.forward * Random.Range(0, 360f));
            }
        }
       
    }

    public void PlusUnits()
    {
        myUnits++;
        unitsShopText.text = myUnits.ToString();
    }

    public void SpawnCrate()
    {
        if(Input.GetMouseButtonDown(0) && pauseMenu.activeSelf == false)
        {
            if (PlayerPrefs.GetInt("battleUnits") > 0)
            {
                
                Vector2 touchPos = cam.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));

                RaycastHit2D rayHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (rayHit.collider == null)
                {
                    Instantiate(crate, touchPos + new Vector2(0, 10), Quaternion.identity);
                    PlayerPrefs.SetInt("battleUnits", PlayerPrefs.GetInt("battleUnits") - 1);
                }
            }
        }
    }

    public void CrateButton()
    {
        spawnCrate = !spawnCrate;
        if (spawnCrate) crateBulb.GetComponent<RawImage>().color = Color.green;
        else crateBulb.GetComponent<RawImage>().color = Color.red;
    }
}
