using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    Vector2 destination;
    public float speed;
    public GameObject smoke;
    public GameObject[] units;
    SpawnUnits su;
    GameManager gm;
    void Start()
    {
        su = FindObjectOfType<SpawnUnits>();
        gm = FindObjectOfType<GameManager>();
        destination = new Vector2(transform.position.x, transform.position.y - 10f);
    }
    
    void Update()
    {
        Vector2.MoveTowards(transform.position, destination, speed * Time.deltaTime);

        if(transform.position.y /10 <= destination.y / 10)
        {
            Instantiate(smoke, transform.position, Quaternion.identity);
            if (su.chosenTeam == 1)
            {
                GameObject unit = Instantiate(units[0], transform.position, Quaternion.identity);
                gm.rocks++;
                gm.maxUnits++;
                unit.GetComponent<Unit>().isGameStarted = true;
            }
            if (su.chosenTeam == 2)
            {
                GameObject unit = Instantiate(units[1], transform.position, Quaternion.identity);
                gm.papers++;
                gm.maxUnits++;
                unit.GetComponent<Unit>().isGameStarted = true;
            }
            if (su.chosenTeam == 3)
            {
                GameObject unit = Instantiate(units[2], transform.position, Quaternion.identity);
                gm.scissors++;
                gm.maxUnits++;
                unit.GetComponent<Unit>().isGameStarted = true;
            }
            Destroy(gameObject);
        }
    }
}
