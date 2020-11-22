using System.Collections.Generic;
using UnityEngine;
using System;
using Mono.Data.Sqlite;
using System.IO;

public class InventoryManager : MonoBehaviour
{
    //public List<Items> itemList; //(OBSOLETE) the list of items collected by the user

    public List<NewItem> newItemList; //The NEW list of items collected by the user

    private string pathToItemFolder; //Directory path to items folder. Should be .../Assets/Scenes/items

    public int knownItemCount {get; set;} //this is a variable for how many items the game *knows*. Used for logic when adding new items

    private static InventoryManager _instance;

    //using the singleton design pattern so there is only one instance of this object
    //that way we do not create more than one inventory, making it peristant 
    public static InventoryManager Instance{
        get{
            if(_instance == null){
                GameObject iManager = new GameObject("inventoryManager");
                iManager.AddComponent<InventoryManager>();
            }
            return _instance;
        }
    }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        _instance = this;
        newItemList = new List<NewItem>();

        //itemList = new List<Items>(); //We're no longer using this. This is for the old way to assign 
                                        //and display items. Newer one below
        
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        knownItemCount = 0; 
        string[] pathsToItemFolder = {Environment.CurrentDirectory, "Assets", "Scenes", "items"};
        pathToItemFolder = Path.Combine(pathsToItemFolder);
        
    }

    ///<summary>
    ///Given a NewItem object, adds item to inventory to be displayed
    ///</summary>
    public void assignItem(NewItem item){
        newItemList.Add(item);
    }


    //THIS VERSION IS NOW OBSOLETE. NEW VERSION IS ABOVE ^^^^^
    //This version uses the database version, which we're no longer using
    //Buuuuuuuuut just in case we end up using it again, I'm leaving it here.

    // public void assignItem(string itemTitle){

    //     //Connect to the database. To be honest, this SHOULD be a function call so I need to sort that out...
    //     string[] paths = {Environment.CurrentDirectory, "Assets", "Scenes", "databases", "testDatabase.db"};
    //     string fullPath = Path.Combine(paths);
    //     Debug.Log(fullPath); //Debugging purposes
    //     string connection = "URI=file:" + fullPath;
    //     Debug.Log(connection);
    //     SqliteConnection dbcon = new SqliteConnection(connection);
    //     dbcon.Open();
    //     Debug.Log("Connected to the database."); //debugging purposes

    //     SqliteDataReader reader;
    //     SqliteCommand cmdRead;
    //     cmdRead = dbcon.CreateCommand();
    //     cmdRead.CommandText = "SELECT * FROM ITEMS WHERE ItemID = @itemTitle"; //"@itemTitle" being a parameter. That way, we avoid SQL injection attacks

    //     cmdRead.Parameters.Add(new SqliteParameter("@itemTitle", itemTitle)); //adding the parameter to the SQL query so safely do the command
    //     reader = cmdRead.ExecuteReader();

    //     //create a new object and insert the data values from the database into this object
    //     Items item = new Items();
    //     while(reader.Read()){
    //         item.itemID = reader[0].ToString();
    //         item.itemType = reader[1].ToString();
    //         item.itemSummary = reader[2].ToString();
    //         item.itemContent = reader[3].ToString();
    //         item.isObtained = Convert.ToBoolean(reader[4]);
    //     }
    //     itemList.Add(item); //add the item object to the list of objects. I chose a list as this datatype can be changed in terms of size
    //     dbcon.Close();
    //     Debug.Log("Databased has been closed.");
    // }
}
