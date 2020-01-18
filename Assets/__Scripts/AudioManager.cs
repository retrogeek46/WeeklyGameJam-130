using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    private static AudioSource themeAudioSource;

    public static AudioClip jump;
    public static AudioClip die;
    public static AudioClip charge;
    public static AudioClip arrowRelease;
    public static AudioClip arrowHit;
    public static AudioClip createDeadBody;
    public static AudioClip theme;
    public static AudioClip themeSlow;

    public AudioClip jumpSetter;
    public AudioClip dieSetter;
    public AudioClip chargeSetter;
    public AudioClip arrowReleaseSetter;
    public AudioClip arrowHitSetter;
    public AudioClip createDeadBodySetter;
    public AudioClip themeSetter;
    public AudioClip themeSlowSetter;

    // singleton
    private static AudioManager _instance = null;
    // Start is called before the first frame update
    void Awake () {
        if (_instance != null) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
            GameObject.DontDestroyOnLoad(gameObject);
        }
        themeAudioSource = this.gameObject.GetComponent<AudioSource>();
    }

    private void Start () {
        jump = jumpSetter;
        die = dieSetter; 
        charge = chargeSetter; 
        arrowRelease = arrowReleaseSetter; 
        arrowHit = arrowHitSetter; 
        createDeadBody = createDeadBodySetter;
        theme = themeSetter;
        themeSlow = themeSlowSetter;
    }

    public static void PlayOnce(string clipName, AudioSource audioSource) {
        AudioClip clip = null;

        switch (clipName) {
            case "jump":
                clip = jump;
                break;
            case "die":
                clip = die; 
                break;
            case "arrowRelease":
                clip = arrowRelease; 
                break;
            case "arrowHit":
                clip = arrowHit; 
                break;
            case "charge":
                clip = charge; 
                break;
            case "createDeadBody":
                clip = createDeadBody; 
                break;
            default:
                clip = createDeadBody;
                break;
        }
        audioSource.clip = clip;
        audioSource.Play();
    }

    public static void ChangeTheme (string themeName) {
        AudioClip clip = null;
        if (themeName == "theme") {
            clip = theme;
        } else if (themeName == "themeSlow") {
            clip = themeSlow;
        }
        themeAudioSource.clip = clip;
        themeAudioSource.Play();

    }
}
