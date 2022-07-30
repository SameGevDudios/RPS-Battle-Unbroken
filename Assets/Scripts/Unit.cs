using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public bool isGameStarted, rotated;
    public float speed, timer;
    public Sprite rock, paper, scissors;
    SpriteRenderer sr;
    public GameObject confetti;
    public Gradient rockTrail, paperTrail, scissorsTrail;
    TrailRenderer trail;
    GameManager gm;
    public AudioClip rSound, pSound, sSound;
    Collider2D col;
    Collider2D wall;
    SpawnUnits su;
    Vector3 _direction;
    AudioSource aS;

    void Start()
    {
        Rotate();
        sr = GetComponent<SpriteRenderer>();
        trail = GetComponent<TrailRenderer>();
        gm = FindObjectOfType<GameManager>();
        su = FindObjectOfType<SpawnUnits>();
        _direction = transform.right;

        speed = 2.5f;

        aS = GameObject.FindGameObjectWithTag("soundManager").GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isGameStarted)
        {
            this.transform.rotation = Quaternion.Euler(0, 0, 0);//это заглушка, чтобы правильно отрисовывался рейкаст
            transform.Translate(_direction * -speed * Time.deltaTime, Space.Self);

            LayerMask _mask = LayerMask.GetMask("wall");
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -_direction, .1f, _mask);
            Debug.DrawRay(transform.position, -_direction, Color.red);

            if (hit)
            {
                _direction = Vector2.Reflect(_direction, hit.normal).normalized;
            }
        }
        else
        {
            if (gameObject.tag == "rock" && su.chosenTeam != 1) transform.position = Vector2.MoveTowards(transform.position, gm.rSpawn, 15 * Time.deltaTime);
            if (gameObject.tag == "paper" && su.chosenTeam != 2) transform.position = Vector2.MoveTowards(transform.position, gm.pSpawn, 15 * Time.deltaTime);
            if (gameObject.tag == "scissors" && su.chosenTeam != 3) transform.position = Vector2.MoveTowards(transform.position, gm.sSpawn, 15 * Time.deltaTime);
        }
        
    }
    void OnTriggerStay2D(Collider2D colObj)
    {
        if (!gm.started)
        {
            if (colObj.tag == "wall")
            {
                if (gameObject.tag == "rock") gm.RefRSpawn();
                if (gameObject.tag == "paper") gm.RefPSpawn();
                if (gameObject.tag == "scissors") gm.RefSSpawn();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D colObj)
    {
        if (isGameStarted)
        {
            if (gameObject.tag == "rock" && colObj.tag == "paper")
            {
                gm.papers++;
                gm.rocks--;
                gameObject.tag = "paper";
                sr.sprite = paper;
                trail.colorGradient = paperTrail;
                aS.PlayOneShot(pSound);
                col = colObj;
                ChangeType();
            }
            if (gameObject.tag == "paper" && colObj.tag == "scissors")
            {
                gm.scissors++;
                gm.papers--;
                gameObject.tag = "scissors";
                sr.sprite = scissors;
                trail.colorGradient = scissorsTrail;
                aS.PlayOneShot(sSound);
                col = colObj;
                ChangeType();
            }
            if (gameObject.tag == "scissors" && colObj.tag == "rock")
            {
                gm.rocks++;
                gm.scissors--;
                gameObject.tag = "rock";
                sr.sprite = rock;
                trail.colorGradient = rockTrail;
                col = colObj;
                aS.PlayOneShot(rSound);
                ChangeType();
            }
            gm.ChangeSpeed();
        }
    }

    void ChangeType()
    {
        transform.localScale = col.transform.localScale;
        Instantiate(confetti, transform.position, transform.rotation);
        gameObject.GetComponent<CircleCollider2D>().radius = col.GetComponent<CircleCollider2D>().radius;
    }
    void Rotate()
    {
        Quaternion rot = transform.rotation;
        float toRotate = Random.Range(0f, 360f);
        transform.Rotate(Vector3.forward * toRotate, Space.Self);
    }
}
