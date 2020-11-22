using System.Collections;
using UnityEngine;

public class RaycastScript : MonoBehaviour
{
    //raycast behaviour for the player

    public float distance;
    protected Vector3 minMousePosition; //used to check if the mouse is within the screenbox
    protected Vector3 maxMousePosition; //used to check if the mouse is within the screenbox
    public Material selectedMaterial; //the selectedMaterial highlight
    public Material defaultMaterial; //the default material on the obstacles
    private Transform deselectCheck; //variable storing any select Transform properties. Is null if mouse is hovering over nothing
    public GameObject canvas;

    public GameObject noLookText; //Text to appear when player looks at an obstacle
    public GameObject noTouchText; //Text to appear when player clicks on an obstacle

    // Update is called once per frame

    //Given the transform of an obstacle that has been seen, generates a text above the object
    //that also follows the camera's rotation
    void createHeyText(Transform obstacleHit)
    {
        //Instantiate TextMeshPro prefab and set it to be above the obstacle
        var heyText = Instantiate(noLookText, new Vector3(obstacleHit.transform.position.x, obstacleHit.transform.position.y + 2, obstacleHit.transform.position.z), Quaternion.identity, obstacleHit.transform) as GameObject;

        //make sure it looks at the player/main camera the entire time
        heyText.transform.rotation = Quaternion.LookRotation(heyText.transform.position - Camera.main.transform.position);

    }

    ///<summary>
    ///Creates the text "No Touching!!" that will point towards the player for a second before disappearing
    ///</summary>
    IEnumerator createDontTouchText(Transform obstacleHit)
    {

        if(GameObject.FindGameObjectWithTag("noTouching") != null){
            Destroy(GameObject.FindGameObjectWithTag("noTouching"));
        }

        float time = 1f; //The amount of time the text will be out for (1 second)
        float incrementTime = 0f; //Initial timer that will increment to time variable

        var dontTouch = Instantiate(noTouchText, new Vector3(obstacleHit.transform.position.x + 5, obstacleHit.transform.position.y, obstacleHit.transform.position.z), Quaternion.identity, obstacleHit.transform) as GameObject;

        //while the increment time is less than the time set for text to be out for,
        //The position if the text will be positioned right next to the obstacle, while
        //the rotation will be altered every frame so the text is looking at the camera.
        //The increment time will increased based on the completion time of a frame (Time.deltaTime).
        //Afterwards, the object will be destroyed
        while(incrementTime < time){
            dontTouch.transform.rotation = Quaternion.LookRotation(dontTouch.transform.position - Camera.main.transform.position);
            incrementTime += Time.deltaTime;
            yield return null;

        }
        Destroy(GameObject.FindGameObjectWithTag("noTouching"));
    }

    void Update()
    {
        //This piece of code will remove the highlight material from an obstacle if the mouse is no longer
        //hovering on the object
        if (deselectCheck != null) {
            var selectionRenderer = deselectCheck.GetComponent<Renderer>();
            selectionRenderer.material = defaultMaterial;
            deselectCheck = null;
        }

        //This checks if the text already exists. If it does, it destroys it.
        //This makes it so multiple text game objects are not stacking up on each other
        if (GameObject.FindGameObjectWithTag("noLooking") != null)
        {
            Destroy(GameObject.FindGameObjectWithTag("noLooking"));
        }

        RaycastHit hit; //the variable stored for the hit object
        Ray directionRay = new Ray(transform.position, transform.TransformDirection(Vector3.forward)); //A ray variable showing the origin position and the direction of the raycast
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition); //ray variable storing the cursor position

        //Ray logic for when the player is looking at an obstacle within a given distance
        //If that collider has the tag "Obstacle", it will show the ray (for debugging purposes)
        if(Physics.Raycast(directionRay, out hit, distance))
        {
            if(hit.collider.tag == "Obstacle")
            {
                createHeyText(hit.collider.transform); //creates text above the obstacle
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * distance, Color.red); //debugging purposes
                Debug.Log("looking at an obstacle via player! ");
            }
            
        }

        if (Physics.Raycast(mouseRay, out hit))
        {
            if(hit.collider.CompareTag("Obstacle"))
            {
                //collects transform and render properties of the obstacle the mouse is hovering on, and giving it the selected material
                var selection = hit.transform;
                var selectionRenderer = selection.GetComponent<Renderer>();
                if(selectionRenderer != null)
                {
                    selectionRenderer.material = selectedMaterial;
                }
                deselectCheck = selection; //this is used to check later on if the mouse is hovering on the obstacle
                Debug.Log("looking at an obstacle via mouse!");
            }

            if (Input.GetMouseButtonDown(0))
            {
                if(hit.collider.CompareTag("Obstacle"))
                {
                    StartCoroutine(createDontTouchText(hit.collider.transform));
                }
                
            }
            
        }

        
    }
}
