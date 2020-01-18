using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDeath : MonoBehaviour {

    public GameObject player;           // reference to the player gameobject
    public GameObject checkpoint;       // reference to the checkpoint where the player gets reborn
    public GameObject deadBody;         // reference to the prefab to instantiate

    private GameObject[] skullsUI;
    private bool doCreateBody;          // bool to check if player pressed the button to create body
    private Vector2 deathPosition;
    public static bool playerDead;
    public int deaths;
    public PhysicsMaterial2D slipperyMaterial;
    public GameObject skull;
    public GameObject canvas;

    private AudioSource playerAudioSource;

    // Start is called before the first frame update
    void Start () {
        InitDeaths();
        playerAudioSource = gameObject.GetComponent<AudioSource>();
    }

    public void InitDeaths () {
        if (GameManager.awakened) {
            GameManager.SetCurrentDeath();
            deaths = GameManager.maximumRespawns;
        } else {
            deaths = 0;
        }
        skullsUI = new GameObject[deaths];
        canvas = GameObject.Find("Canvas");

        for (int i = 0; i < deaths; i++) {
            GameObject skullInstance = Instantiate(skull);
            skullInstance.transform.SetParent(canvas.transform, false);
            skullInstance.GetComponent<Image>().rectTransform.anchoredPosition = new Vector2(20 + (30 * i), -20);
            skullsUI[i] = skullInstance;
        }
    }


    // Update is called once per frame
    void Update () {
        if (playerDead) {
            Die();
        }
        if (Input.GetKeyDown(KeyCode.A) && deaths > 0 && GameManager.awakened) {
            doCreateBody = true;
            Die();
        } 
        else if (doCreateBody) {
            doCreateBody = false;
        }
    }

    /// <summary>
    /// Function to respawn player at checkpoint or call <see cref="CreateBodyPlatform"/> function
    /// </summary>
    private void Die () {
        playerDead = false;
        PlayerController.movementActive = false;
        deathPosition = player.transform.position;
        // call create body function if 
        if (doCreateBody && deaths > 0) {
            CreateBodyPlatform();
            Destroy(skullsUI[deaths-1].gameObject);
            deaths--;
        }
        // die and get reborn at the checkpoint
        // TODO: insert animation and a bit of delay
        else {
            player.transform.position = checkpoint.transform.position;
        }
        PlayerController.movementActive = true;
    }

    /// <summary>
    /// Function to instantiate dead bodies when appropriate key is pressed
    /// </summary>
    private void CreateBodyPlatform () {
        PlayerController.movementActive = false;
        // create the deadbody
        var body = Instantiate(deadBody);
        body.transform.position = deathPosition;
        // move the player up a bit 
        player.transform.position = new Vector2(deathPosition.x, deathPosition.y + 0.25f);
        // add components to the instantiated deadbody
        body.AddComponent<BoxCollider2D>();
        body.tag = "ground";
        body.GetComponent<BoxCollider2D>().sharedMaterial = slipperyMaterial;
        AudioManager.PlayOnce("createDeadBody", playerAudioSource);
        PlayerController.movementActive = true;
    }
}
