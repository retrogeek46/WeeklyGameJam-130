using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainScreen : MonoBehaviour {

    public Text startText;
    private float limit = 5f;
    private float current = 0f;
    private bool start = false;
    public string levelName;

    private void Start () {
        start = false;
        startText.enabled = false;
        StartCoroutine(ShowStartMessage());
    }


    // Update is called once per frame
    void Update () {

        if (start) {
            if (current < (limit / 2)) {
                startText.enabled = true;
            } else if (current > (limit / 2) && current < limit) {
                startText.enabled = false;
            } else {
                current = 0;
            }
        }

        if (Input.anyKeyDown) {
            SceneManager.LoadScene(levelName);
        }
        current+= 0.1f;
    }
    
    private IEnumerator ShowStartMessage () {
        yield return new WaitForSeconds(1f);
        start = true;
    }
}
