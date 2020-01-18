using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static int maximumRespawns;
    public static bool showingText;
    private static Text textbox;
    private static string message;
    private static bool showText;
    public static bool awakened;
    public static string oldDudePrompt = "You are not worthy, yet.|You haven't tasted true rebirth.|Begin by breaking your shackles, then go and explore the world.  [Press SPACE]";

    private Scene oldScene;

    // singleton
    private static GameManager _instance = null;

    // Start is called before the first frame update
    void Awake () {
        oldScene = SceneManager.GetActiveScene();
        
        if (_instance != null) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
            GameObject.DontDestroyOnLoad(gameObject);
        }
        
        SetCurrentDeath();
    }

    // Update is called once per frame
    void Update () {
        if (showText) {
            showText = false;
            StartCoroutine(TextCoroutine(textbox, message));
        }
        if (Input.GetKeyDown(KeyCode.R)) {
            ReloadScene();
        }
        if (Input.GetKey(KeyCode.Q) && Input.GetKey(KeyCode.T)) {
            awakened = true;
            GameObject.FindGameObjectWithTag("player").GetComponent<PlayerDeath>().InitDeaths();
        }
        if (CurrentScene() == "Level_0" && awakened && GameObject.Find("Underground Marker") != null) {
            GameObject.Find("Underground Marker").SetActive(false);
        }
        if (oldScene != SceneManager.GetActiveScene()) {
            oldScene = SceneManager.GetActiveScene();
            SetCurrentDeath();
        }
    }

    private string CurrentScene () {
        return SceneManager.GetActiveScene().name;
    }

    private void ReloadScene () {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Function to set the number of deaths player has for given level based on level name
    /// </summary>
    public static void SetCurrentDeath () {
        switch (SceneManager.GetActiveScene().name) {
            case "Level_0":
                maximumRespawns = 1;
                break;
            case "Level_1":
                maximumRespawns = 2;
                break;
            case "Level_2":
                maximumRespawns = 1;
                break;
            case "Level_3":
                maximumRespawns = 2;
                break;
            case "Level_Underground":
                maximumRespawns = 1;
                break;
            default:
                maximumRespawns = 1;
                break;
        }
    }

    /// <summary>
    /// Function to change level based on scene name
    /// </summary>
    /// <param name="markerID">the name of scene to change to</param>
    public static void GoToNextLevel (string markerID) {
        SceneManager.LoadScene(markerID);
    }

    /// <summary>
    /// A function to handle text messages based on event triggers
    /// </summary>
    /// <param name="textbox">referece to textbox object on which text is shown</param>
    /// <param name="message">the message to display</param>
    public static void ShowTextMessage (Text textbox_arg, string message_arg) {
        // pass the textbox and message to local objects
        textbox = textbox_arg;
        message = message_arg;
        textbox.gameObject.transform.parent.gameObject.SetActive(true);
        showText = true;
        if (message == oldDudePrompt) {
            OldDude.controlPlayer = true;
        }
    }

    /// <summary>
    /// Coroutine to introduce delay between every character when showing text
    /// </summary>
    /// <param name="textbox">referece to textbox object on which text is shown</param>
    /// <param name="txt">the message to display</param>
    /// <returns></returns>
    private IEnumerator TextCoroutine (Text textbox, string txt) {
        // show text with delay of 0.03 seconds between characters
        showingText = true;
        string[] text = txt.Split('|');
        for (int j = 0; j < text.Length; j++) {
            txt = text[j];
            txt = txt.Trim();
            textbox.text = "";
            int i = 0;
            while (i < txt.Length) {
                textbox.text += txt[i];
                i++;
                yield return new WaitForSeconds(.03f);
            }
            if (j != text.Length - 1) {
                yield return new WaitForSeconds(1f);
            }
        }
        showingText = false;
    }
}
