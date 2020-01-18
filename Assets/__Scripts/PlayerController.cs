using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {

    public Rigidbody2D playerBody2D;
    public float moveSpeed = 20f;
    public float jumpSpeed = 20f;
    public static bool movementActive;
    public bool inAir;
    private AudioSource playerAudioSource;

    // Start is called before the first frame update
    void Start () {
        movementActive = true;
        playerAudioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update () {
        if (movementActive) {
            if (Input.GetKey(KeyCode.LeftArrow)) {
                Move(-1);
            }
            if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.A)) {
                Move(1);
            }
            if (Input.GetKeyDown(KeyCode.Space)) {
                Jump();
            }
        }
        if (!Input.anyKey || !movementActive) {
            playerBody2D.velocity = new Vector2(0, playerBody2D.velocity.y);
        }
    }

    private void Move (int direction) {
        playerBody2D.velocity = new Vector2(direction * moveSpeed, playerBody2D.velocity.y);
    }

    private void Jump () {
        if (!inAir) {
            playerBody2D.velocity = new Vector2(playerBody2D.velocity.x, jumpSpeed);
            AudioManager.PlayOnce("jump", playerAudioSource);
            inAir = true;
        }
        // already jumped
        return;
    }

    private void OnCollisionEnter2D (Collision2D collision) {
        // if player touches floor or platforms then grounded
        if (collision.gameObject.CompareTag("ground")) {
            inAir = false;
        }
        if (collision.gameObject.CompareTag("nextlevelmarker")) {
            GameManager.GoToNextLevel(collision.gameObject.GetComponent<Marker>().markerID);
        }
        if (collision.gameObject.CompareTag("animtrigger")) {
            GameObject obstacle = GameObject.Find(collision.gameObject.name+"Obstacle");
            obstacle.GetComponent<Animator>().SetTrigger("unlock");
        }
    }

    private void OnTriggerEnter2D (Collider2D collision) {
        if (collision.gameObject.CompareTag("deathzone")) {
            PlayerDeath.playerDead = true;
        }
    }
}
