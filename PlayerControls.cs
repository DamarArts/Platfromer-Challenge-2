using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerControls : MonoBehaviour
{
    public CameraController Camscript;
    private Rigidbody2D rb;
    public Animator anim;
    public Animator anim2;
    public Animator anim3;

    public AudioClip musicClipOne;

    public AudioClip musicClipTwo;

    public AudioClip musicClipThree;
    public AudioClip musicClipFour;

    public AudioClip musicClipFive;

    public AudioSource musicSource;

    private Vector3 thetVelocity = Vector3.zero;

    public float speed;
    public float JumpSpeed;

    private readonly float MovementSmoothing = .05f;

    public GameObject Enemy;
    public GameObject[] Enemies;


    public bool jumping;
    private bool facingRight;

    public int Jewels;
    private int Score;
    private int Lives;
    private string sceneName;

    public Text JewelCount;
    public Text WinText;
    public Text LoseText;
    public Text ScoreText;
    public Text LivesText;


    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        facingRight = true;
        jumping = false;
        Lives = 4;
        Score = 0;
        Jewels = 0;
        WinText.text = "";
        LoseText.text = "";
        ScoreText.text = "";
        JewelCount.text = "";
        LivesText.text = "";
        SetAllText();
        

        Scene currentScene = SceneManager.GetActiveScene();

        sceneName = currentScene.name;


    }

    private void Update()
    {
        anim = GetComponent<Animator>();
        HandJumpAndFall();


        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }

        if (Jewels == 22)
        {
            if ((Input.GetKey(KeyCode.KeypadEnter)) || (Input.GetKey(KeyCode.Return)))
            {
                SceneManager.LoadScene(1);
            }
        }

    }
    private void FixedUpdate()
    {

        float moveHorizontal = Input.GetAxis("Horizontal");

        Vector2 targetVelocity = new Vector2(moveHorizontal * speed, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref thetVelocity, MovementSmoothing);
        Flip(moveHorizontal);

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if ((collision.collider.tag == "Ground"))
        {
            anim.SetInteger("State", 0);
            jumping = false;

            if (Input.GetKey(KeyCode.Space))
            {
                Vector2 jumpUp = Vector2.up * JumpSpeed;
                rb.velocity = jumpUp;
                jumping = true;
                musicSource.clip = musicClipFour;
                musicSource.Play();

            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                speed = 7;
            }
            else
            {
                speed = 5;
            }

            if ((Input.GetKey(KeyCode.RightArrow) && (speed == 5)) || (Input.GetKey(KeyCode.LeftArrow) && (speed == 5)))
            {
                anim.SetInteger("State", 1);


            }
            else if ((Input.GetKey(KeyCode.RightArrow) && (speed == 7)) || (Input.GetKey(KeyCode.LeftArrow) && (speed == 7)))
            {
                anim.SetInteger("State", 4);

            }
            else
            {
                anim.SetInteger("State", 0);

            }

        }
        if ((collision.collider.tag == "elevator"))
        {
            anim2.SetInteger("State", 1);

            anim.SetInteger("State", 0);


            if ((Input.GetKey(KeyCode.RightArrow) && (speed == 5)) || (Input.GetKey(KeyCode.LeftArrow) && (speed == 5)))
            {
                anim.SetInteger("State", 1);
            }
            rb.velocity = Vector3.zero;
        }
        else
        {
            anim2.SetInteger("State", 0);

        }

        if ((collision.collider.tag == "elevator2"))
        {
            anim3.SetInteger("State", 1);

            anim.SetInteger("State", 0);


            if ((Input.GetKey(KeyCode.RightArrow) && (speed == 5)) || (Input.GetKey(KeyCode.LeftArrow) && (speed == 5)))
            {
                anim.SetInteger("State", 1);
            }
            rb.velocity = Vector3.zero;
        }
        else
        {
            anim3.SetInteger("State", 0);

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.gameObject.CompareTag("jewel")))
        {

            other.gameObject.SetActive(false);
            Jewels = Jewels + 1;
            Camscript.Juls = Camscript.Juls + 1;
            Score = Score + 1;
            SetAllText();
            musicSource.clip = musicClipTwo;
            musicSource.Play();

        }
        if ((other.gameObject.CompareTag("Enemy")))
        {
            other.gameObject.SetActive(false);
            Score = Score - 1;
            Lives = Lives - 1;
            SetAllText();
            musicSource.clip = musicClipThree;
            musicSource.Play();

        }
    }



    void  SetAllText()
     {
        JewelCount.text = "" + Jewels.ToString();
        ScoreText.text = "Score : " + Score.ToString();
        LivesText.text = "X " + Lives.ToString();
        if (sceneName == "firstlevel")
            
        {
            if (Jewels == 22)
            {
                WinText.text = "MISSION 1 COMPLETE";

                Enemies = GameObject.FindGameObjectsWithTag("Enemy");

                for (var i = 0; i < Enemies.Length; i++)
                {
                    Destroy(Enemies[i]);
                }
            }



        }
        else if (sceneName == "secondlevel")
        {
            if (Jewels == 22)
            {
                WinText.text = "MISSION 2 COMPLETE \n YOU WIN";

                Enemies = GameObject.FindGameObjectsWithTag("Enemy");

                for (var i = 0; i < Enemies.Length; i++)
                {
                    Destroy(Enemies[i]);
                }
            }


   
        }

        if (Lives == 0)
        {
            LoseText.text = "GAME OVER";
            gameObject.SetActive(false);


        }

     }

    void HandJumpAndFall()
    {

        if (jumping)

            if (rb.velocity.y > 0)
            {
                anim.SetInteger("State", 2);
            }
            else if (rb.velocity.y < 0)
            {
               anim.SetInteger("State", 3);
            }


    }

    private void Flip(float moveHorizontal)
    {
        if(moveHorizontal > 0 && !facingRight || moveHorizontal < 0 && facingRight)
        {
            facingRight = !facingRight;
            Vector3 theScale = transform.localScale;

            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }

}
