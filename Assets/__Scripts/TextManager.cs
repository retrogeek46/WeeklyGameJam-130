using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour {

    public Text textBox;
    [TextArea(3, 5)]
    public string message;
    private bool hasPlayed;

    private void Start () {
        hasPlayed = false;
    }

    private void Update () {
        if (hasPlayed && 
            textBox.gameObject.transform.parent.gameObject.activeSelf && 
            !GameManager.showingText) {
            if (Input.GetKey(KeyCode.Space)) {
                textBox.gameObject.transform.parent.gameObject.SetActive(false);
                if (message == GameManager.oldDudePrompt) {
                    OldDude.startParticle = true;
                }
            }
        }
    }

    private void OnTriggerEnter2D (Collider2D collision) {
        // if text has not been shown yet and player collides with the text collider trigger
        if (collision.gameObject.CompareTag("player") && !hasPlayed) {
            hasPlayed = true;
            AudioManager.ChangeTheme("themeSlow");
            GameManager.ShowTextMessage(textBox, message);
        }
    }
}
