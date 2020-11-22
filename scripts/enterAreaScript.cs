using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class enterAreaScript : MonoBehaviour
{
    //variable needed for coroutine function
    private IEnumerator coroutine;

    public string areaName; //string storing the arena name
    private GameObject arenaUIText; //the game object of the text field
    private Text textColour; //attribute I'll be using for adjusting opacity
    private float fadeSpeed; //speed multiplier for the fade text effect
    private GameObject inventoryText; //What I'll be using to edit text regarding player's inventory

    private bool areaVisited; //Variable used to check if an area has been visited or not
    
    private NewItem itemForArea; //Item that can be picked up if player enters area

    void Awake(){
        //Referencing the appropriate variables. These are private 
        //(no one else really need to see this in the inspector)
        arenaUIText = GameObject.FindGameObjectWithTag("AreaTriggerText");
        textColour = arenaUIText.GetComponent<Text>();
        inventoryText = GameObject.FindGameObjectWithTag("NewItemText");
        itemForArea = this.gameObject.GetComponent<NewItem>();

    }

    // Start is called before the first frame update
    void Start()
    {
        //when the game starts, deactivate the text so it does not show
        inventoryText.SetActive(false);
        arenaUIText.SetActive(false);
        areaVisited = false;
        fadeSpeed = 0.4f;
    }

    //Given a text object and a float to adjust the speed to which the text
    //object fades away, text is displayed for 3 seconds and opacity changes from
    //1f -> 0f
    IEnumerator fadeAway(Text text, float speedMultiplier)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1f);
        arenaUIText.SetActive(true); //This is to activate text again in case player comes back
        yield return new WaitForSeconds(3); //text displays for 3 seconds before fade effect
        //on each frame, opacity of text decreases based on completion time of frame until opacity = 0
        while(0 < text.color.a)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - (speedMultiplier * Time.deltaTime));
            //Debug.Log(text.color.a);
            yield return null;
        }
        arenaUIText.SetActive(false); //disables text
    }

    ///<summary>
    ///notifies the user that a new item has been stored into their iventory. disappears after 3 seconds.
    ///</summary>
    IEnumerator newInventoryItem(GameObject invGameObject){
        invGameObject.SetActive(true);
        invGameObject.GetComponent<Text>().text = "New item in Inventory!!";
        yield return new WaitForSeconds(3);
        invGameObject.SetActive(false);
    }

    ///<summary>
    /// For this specific use of OnTriggerEnter, assigns an item to the inventory
    /// When a player enters an area prefab
    ///</summary>
    void OnTriggerEnter(Collider player){
        //Logic only applies if the area hasn't been visited already and the
        //player object has entered the area   
        if(player.CompareTag("Player") && this.gameObject.CompareTag("Area") && !areaVisited){         
            InventoryManager.Instance.assignItem(itemForArea);
            StartCoroutine(newInventoryItem(inventoryText));
            gameManager.Instance.hiddenAreaCount += 1;

            arenaUIText.SetActive(true);
            arenaUIText.GetComponent<Text>().text = "You have entered " + areaName + "!";
            StartCoroutine(fadeAway(textColour, fadeSpeed = 1.5f));
            areaVisited = true; //To make sure the coroutine does not happen again
        }       
    }

    //given a collider object, displays that they have left a particular area
    //and fades away after 5 seconds
    void OnTriggerExit(Collider player)
    {
        if(player.gameObject.tag == "Player")
        {
            arenaUIText.GetComponent<Text>().text = "You have left " + areaName + ".";
            arenaUIText.SetActive(false);
        }
    }
}
