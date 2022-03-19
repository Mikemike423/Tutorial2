using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;

    public AudioClip victoryTheme;
    public AudioClip music;
    public AudioSource sounds;

    public float speed;

    private bool inLevel2 = false;
    private bool won = false;

    public GameObject level2Spawn;

    public TextMeshProUGUI score;
    public TextMeshProUGUI winText;
    public TextMeshProUGUI loseText;
    public TextMeshProUGUI livesText;

    public Animator anim;

    public int jumpForce;
    public int lives = 3;
    private int scoreValue = 0;

    // Start is called before the first frame update
    void Start()
    {
        sounds.clip = music;
        sounds.Play();
        rd2d = GetComponent<Rigidbody2D>();
        score.text = "Coins: " + scoreValue.ToString();
        livesText.text = "Lives: " + lives;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        rd2d.AddForce(new Vector2(hozMovement * speed, 0.0f));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = "Coins: " + scoreValue.ToString();
            Destroy(collision.collider.gameObject);        
        }
        if (collision.collider.tag == "Enemy" && !won){
            Destroy(collision.collider.gameObject);
            lives--;
            livesText.text = "Lives: " + lives;
        }
        if (lives <= 0) {
            Destroy(this.gameObject);
            loseText.gameObject.SetActive(true);
        }
        if (scoreValue >= 4 && !inLevel2) {
            this.gameObject.transform.position = level2Spawn.transform.position;
            lives = 3;
            livesText.text = "Lives: " + lives;
            inLevel2 = true;
        }
        if (scoreValue >= 8) {
            sounds.clip = victoryTheme;
            sounds.loop = false;
            sounds.Play();
            won = true;
            winText.gameObject.SetActive(true);
            Destroy(this.gameObject);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(Input.GetKey(KeyCode.A)||Input.GetKey(KeyCode.D)){
            anim.SetInteger("state", 1);
        }
        else {
            anim.SetInteger("state", 0);
        }
        if (collision.collider.tag == "Ground")
        {
            if (Input.GetKey(KeyCode.W))
            {
                anim.SetInteger("state", 2);
                rd2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            }
        }
    }
}
