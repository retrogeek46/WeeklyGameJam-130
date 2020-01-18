using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OldDude : MonoBehaviour {

    public static bool startParticle;
    public static bool controlPlayer;
    public ParticleSystem charge;
    public GameObject player;
    public GameObject arrow;
    public GameObject flash;
    private bool moveArrow;
    public Animator arrowAnim;
    public Animator playerAnim;
    public GameObject marker;
    private AudioSource oldDudeAudioSource;
    public GameObject tutorialPanel;
    // Start is called before the first frame update
    void Start () {
        moveArrow = false;
        oldDudeAudioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update () {
        if (startParticle) {
            startParticle = false;
            AudioManager.PlayOnce("charge", oldDudeAudioSource);
            StartCoroutine(Attack());
            charge.Play();
        }
        if (controlPlayer) {
            controlPlayer = false;
            PlayerController.movementActive = false;
            playerAnim.enabled = true;
            // playerAnim.SetTrigger("player");
        }
        if (moveArrow) {
            arrow.SetActive(true);
            AudioManager.PlayOnce("arrowRelease", oldDudeAudioSource);
            arrowAnim.SetTrigger("arrow");
            StartCoroutine(FreePlayer());
            moveArrow = false;
        }
    }

    private IEnumerator Attack () {
        yield return new WaitForSeconds(1f);
        moveArrow = true;
    }

    private IEnumerator FreePlayer () {
        yield return new WaitForSeconds(0.8f);
        StartCoroutine(Shake(0.1f, 0.5f));
        yield return new WaitForSeconds(0.1f);
        AudioManager.PlayOnce("arrowHit", oldDudeAudioSource);
        playerAnim.enabled = false;
        PlayerController.movementActive = true;
        flash.SetActive(false);
        GameManager.awakened = true;
        marker.SetActive(true);
        arrow.SetActive(false);
        AudioManager.ChangeTheme("theme");
        player.GetComponent<PlayerDeath>().InitDeaths();
        StartCoroutine(ShowDeathAbilityTutorial());
    }

    private IEnumerator ShowDeathAbilityTutorial () {
        Image img = tutorialPanel.GetComponent<Image>();
        Text txt = tutorialPanel.GetComponentInChildren<Text>();
        // fade in
        Color col = img.color;
        col.a = 0;
        img.color = col;


        Color colt = txt.color;
        colt.a = 0;
        txt.color = colt;

        tutorialPanel.SetActive(true);
        
        float elapsed = 0f;
        float duration = 0.5f;
        float alphaLimit = 0.25f;
        while (elapsed < duration) {
            Color c = img.color;
            if (c.a < alphaLimit) {
                c.a = img.color.a + (0.2f * Time.deltaTime);
                img.color = c;
            }

            Color t = txt.color;
            if (t.a < 1) {
                t.a = txt.color.a + (2f * Time.deltaTime);
                txt.color = t;
            }
            elapsed += Time.deltaTime;
            yield return new WaitForSeconds(0.1f);
        }
        tutorialPanel.SetActive(false);
    }

    private //Coroutine to shake camera every frame
    IEnumerator Shake (float duration, float magnitude) {
        //Get original position
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0f;
        flash.SetActive(true);
        Image image = flash.GetComponent<Image>();
        //loop to shake camera every frame
        while (elapsed < duration) {
            //flash screen by displaying white image and slowly lowering it's alpha
            Color c = image.color;
            if (c.a > 0) {
                c.a = image.color.a - (2f * Time.deltaTime);
                image.color = c;
            }
            if (c.a <= 0) {
                flash.SetActive(false);
            }
            //shake camera here
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            transform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);
            elapsed += Time.deltaTime;
            magnitude -= 1.5f * Time.deltaTime;
            yield return null;
        }
        //set camera back after shake
        transform.localPosition = originalPos;
    }
}
