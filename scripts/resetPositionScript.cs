using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class resetPositionScript : MonoBehaviour
{
    public Transform target; //reference to the player's position
    public float yBoundary; //boundary set to trigger position reset
    public Rigidbody rb;
    private Vector3 vectorReset;
    private Quaternion QuaternionReset;
    public GameObject areaUIText;

    // Start is called before the first frame update
    void Start()
    {
        //reset position and rotation is set when the game runs
        vectorReset = new Vector3(0, 0, 0);
        QuaternionReset = new Quaternion(0, 0, 0, 1);
        
    }

    //when called, text will appear, saying that the player has fallen. Disappears after 3 seconds.
    IEnumerator showText()
    {
        areaUIText.SetActive(true);
        areaUIText.GetComponent<Text>().text = "You fell off :(";
        yield return new WaitForSeconds(3);
        areaUIText.SetActive(false);
    }

    // Update is called once per frame
    void LateUpdate()
    {

        //if the player reaches the y axis boundary, their position
        //is reset
        if(target.position.y < yBoundary)
        {
            //This part is a little buggy. Sometimes it shows and fades, sometimes it does not show at all
            //sometimes it will do what it is meant to do according to the code. Need to fix this
            StartCoroutine(showText());

            target.position = vectorReset; //resets position
            target.rotation = QuaternionReset; //resets rotation so when they come back, they're not spinning
            rb.velocity = vectorReset; //resets velocity so their momentum doesn't carry over from where they fell off
            rb.angularVelocity = vectorReset; //resets angular velocity for the same above ^

            gameManager.Instance.fallCount += 1;
        }
    }
}
