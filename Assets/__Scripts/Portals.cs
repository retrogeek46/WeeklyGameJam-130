using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portals : MonoBehaviour {

    public GameObject fallingObject;
    public Vector2 startPosition;
    private bool reachedEnd;
    private Rigidbody2D fallingObjectRB;
    // Start is called before the first frame update
    void Start () {
        startPosition = fallingObject.transform.position;
        reachedEnd = false;
        fallingObjectRB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update () {
        if (reachedEnd) {
            fallingObject.transform.position = startPosition;
            fallingObjectRB.velocity = Vector2.zero;
            reachedEnd = false;
        }
    }

    private void OnTriggerEnter2D (Collider2D collision) {
        if (collision.gameObject.CompareTag("deathzone")) {
            // teleport object to the top
            reachedEnd = true;
        }
    }
}
