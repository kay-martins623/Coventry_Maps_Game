using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class InventoryScreen : MonoBehaviour
{
    public static bool isLoaded = false; //will be using this to check whether the game is paused

    public GameObject inventoryScreen; //the inventory canvas to show
    public GameObject inGameScreen; //the ingame canvas to show
    public GameObject videoPlayer; //the video player to show on canvas

    public KeyCode inventoryKey; // the key to toggle the inventory screen

    public GameObject titleButtonPrefab; //icon prefab for the inventory
    public GameObject viewPort; //viewport for where the buttons will be

    public Text contentContainer; //the content container for the info on items
    public Button closeInventory; //button object for closing inventory and going back to game

    public Sprite textIcon; //icon category for text-based items
    public Sprite videoIcon; //icon category for video-based items
    public AudioManager audioManager; //audioManager object to manipulate how audio is stopped

    // Start is called before the first frame update
    void Start()
    {
        //Within the first frame, deactivate the inventory and video player UI
        //This way, only the in-game UI is being shown when the game starts
        inventoryScreen.SetActive(false);
        videoPlayer.SetActive(false);

        closeInventory.onClick.AddListener( () => quitInventory());

    }

    // Update is called once per frame
    void Update()
    {
        //If the key for the inventory toggle is pressed, it will load or quit the inventory
        //(this is based on whether or not the inventory is currently loaded)
        if (Input.GetKeyDown(inventoryKey))
        {
            // Debug.Log("You pressed inventory thingy yuh");
            // Debug.Log(isLoaded);
            if (isLoaded)
            {
                quitInventory();
            }
            else
            {
                loadInventory();
            }

        }
    }

    void loadInventory()
    {
        //In-game UI is disabled, as well as the player movement and mouse interation w/objects
        //timescale brought to 0 to stop "time" in the game, giving it a "pause" effect
        //inventory screen is active and the game is told that it is currently loaded
        inGameScreen.SetActive(false);
        GameObject.FindGameObjectWithTag("Player").GetComponent<playerMovementScript>().enabled = false;
        GameObject.FindGameObjectWithTag("Player").GetComponent<RaycastScript>().enabled = false;
        inventoryScreen.SetActive(true);
        Time.timeScale = 0f;
        isLoaded = true;

        /*As the game can't add or adjust to objects that don't actually "exist" at start, you cannot reference to them
         (though I *did* do some research and it is possible? I would have to tinker with it later), I created logic for 
         when the game knows how many items the player has, compared to what has been collected in the database. By "know", 
         I mean knowing how many items they have when they open the inventory. Adding to the itemList in InventoryManager is separate
         
         When the actual size is different to the known size, it creates a new item icon, from the prefab template created. The prefab
         has the custom item class, where the database's item info is given (for easier access)
         
         Once there are many items in the inventory, a scrollbar becomes visible for the user to scroll with*/
        int actualSize = InventoryManager.Instance.newItemList.Count;
        int knownSize = InventoryManager.Instance.knownItemCount;

        if(actualSize < 1){
            contentContainer.text = "No items in your inventory to view. Try to find some!";
        }
        else{
            contentContainer.text = "Please select an item to view.";
        }

        if (knownSize < actualSize)
        {
            //This code is the newer version, using the NewItem object. It's pretty much the same thing
            for(int i = knownSize; i < actualSize; i++){
                var iconButton = Instantiate(titleButtonPrefab, Vector3.zero, Quaternion.identity, viewPort.transform) as GameObject;
                iconButton.GetComponent<Button>().onClick.AddListener(() => newLoadContent(iconButton));
                iconButton.GetComponentInChildren<Text>().text = " - " + InventoryManager.Instance.newItemList[i].itemName;
                NewItem buttonItem = iconButton.GetComponent<NewItem>();

                buttonItem.itemName = InventoryManager.Instance.newItemList[i].itemName;
                buttonItem.itemType = InventoryManager.Instance.newItemList[i].itemType;
                buttonItem.fileName = InventoryManager.Instance.newItemList[i].fileName;

                Image[] images = iconButton.GetComponentsInChildren<Image>();
                for(int j = 0; j < images.Length; j++){
                    if(images[j].CompareTag("ItemTypeIcon")){

                        switch(buttonItem.itemType){
                            case ItemType.Text:
                                images[j].GetComponent<Image>().sprite = textIcon;
                                break;
                            case ItemType.Video:
                                images[j].GetComponent<Image>().sprite = videoIcon;
                                break;
                        }

                    }
                }
                InventoryManager.Instance.knownItemCount = actualSize;
            }
        }
    }

    ///<summary>
    ///Deactivates inventory and restores control back to the player, effectively unpausing the game
    ///</summary>
    void quitInventory()
    {
        inventoryScreen.SetActive(false);
        GameObject.FindGameObjectWithTag("Player").GetComponent<playerMovementScript>().enabled = true;
        GameObject.FindGameObjectWithTag("Player").GetComponent<RaycastScript>().enabled = true;
        inGameScreen.SetActive(true);
        Time.timeScale = 1f;
        isLoaded = false;
    }

    ///<summary>
    ///Newer version of LoadContent(). Loads content by finding it in the items folder.
    ///Different logic is done depending on what kind of content it is 
    ///</summary>
    public void newLoadContent(GameObject btn){
        NewItem selectedItem = btn.GetComponent<NewItem>();

        switch(selectedItem.itemType){
            //If the content is a text, reads the file contents and displays it in
            //inventory box
            case ItemType.Text:
                // Debug.Log(Application.dataPath + "/items/" +  selectedItem.fileName); //DEBUGGING PURPOSES
                StreamReader sr = new StreamReader(Application.dataPath + "/Scenes/items/" +  selectedItem.fileName);
                contentContainer.text = sr.ReadToEnd();
                sr.Close();
                break;
            //If the content is a video, gets the path to the file and uses that path to play the video
            case ItemType.Video:
                string pathToFile = Application.dataPath + "/Scenes/items/" + selectedItem.fileName;
                inventoryScreen.SetActive(false);
                videoPlayer.GetComponentInChildren<VideoPlayer>().url = pathToFile;
                videoPlayer.SetActive(true);
                audioManager.stopAllAudio();
                videoPlayer.GetComponentInChildren<VideoPlayer>().Play();
                break;
        }

    }

    ///<summary>
    ///Loads content based on the item and what kind of item it is. Finds content via database
    ///</summary>
    //THIS CODE IS NOW OBSOLETE. NEW CODE ABOVE ^^
    public void loadContent(GameObject btn)
    {
        //handles any texxt-based items (viewing it in the text container. Becomes scrollable for any big text)
        if(btn.GetComponent<Items>().itemType == "Text"){
            contentContainer.text = btn.GetComponent<Items>().itemContent;
        }
        //Activates video player for any video items
        if(btn.GetComponent<Items>().itemType == "Video"){
            //access the video via url file. This will only accept .mp4 formats
            string[] paths = {Environment.CurrentDirectory, "Assets", "Scenes", "videos", btn.GetComponent<Items>().itemID + ".mp4"};
            string fullPath = Path.Combine(paths);

            //deactivates inventory and activates video player screen. plays video based on url destination
            inventoryScreen.SetActive(false);
            videoPlayer.GetComponentInChildren<VideoPlayer>().url = fullPath;
            videoPlayer.SetActive(true);
            
            // for (int i = 0; i < audioManager.sounds.Length; i++){
            //     audioManager.Pause(audioManager.sounds[i].name);
            // }
            audioManager.stopAllAudio();
            videoPlayer.GetComponentInChildren<VideoPlayer>().Play();
        }
    }
}
