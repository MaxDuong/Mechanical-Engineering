using System.Collections;
using UnityEngine;

/* SUMMARY
Controls a robotic arm gripper's grab and release mechanics using animation and physics
*/
public class ColliderTest3 : MonoBehaviour {
    public int grab = 2; // Grab state: 0 = release, 1 = grab, 2 = idle
    public GameObject anim; // Reference to the animated gripper object
    Collider CollidedWith; // Stores reference to object currently in contact with gripper
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        // Initializes gripper in idle state
        anim.GetComponent<Animator>().speed = 0f;
    }

    // Update is called once per frame
    void Update() {
        // Monitors grab state and triggers appropriate actions
        if (grab == 1) {
            GrabObj();
        }
        if (grab == 0) {
            ReleaseObj();   
        }

    }

    // Initiates grab sequence animation and physics
    public void GrabObj() {
        StartCoroutine(TestCoroutineStart());
    }

    // Initiates grab sequence animation and physics
    public void ReleaseObj() {
        StartCoroutine(TestCoroutineStop());
    }

    // Detects when objects enter gripper trigger zone
    private void OnTriggerEnter(Collider other) {
        CollidedWith = other;
        print(other);
    }

    // Coroutine that handles grab animation and attaches object to gripper
    IEnumerator TestCoroutineStart() {
        grab = 2;
        anim.GetComponent<Animator>().speed = 10f;
        yield return new WaitForSeconds(0.35f);
        anim.GetComponent<Animator>().speed = 0f;
        if (CollidedWith != null) {
            CollidedWith.GetComponent<Rigidbody>().isKinematic = true;
            CollidedWith.transform.parent = this.transform;
        }
    }

    // Coroutine that handles release animation and detaches object from gripper
    IEnumerator TestCoroutineStop() {
        grab = 2;
        anim.GetComponent<Animator>().StartPlayback();
        anim.GetComponent<Animator>().speed = -10f;
        if (CollidedWith != null) {
            CollidedWith.transform.parent = null;
            CollidedWith.GetComponent<Rigidbody>().isKinematic = false;
        }
        yield return new WaitForSeconds(0.35f);
        anim.GetComponent<Animator>().speed = 0f;
    }
}
