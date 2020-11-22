using UnityEngine;
//the libraries needed for the SQLite database
using System;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;

// We're not really using this script since we're no longer using databases for this project (deemed not practical)
// BUT, in case we go back to using it, this will be useful, or you can even improve it
public class DBScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //This is test code to see whether the game could successfully open, read from and close the database

        string[] paths = {Environment.CurrentDirectory, "Assets", "Scenes", "databases", "testDatabase.db"};
        string fullPath = Path.Combine(paths);
        Debug.Log(fullPath); //Debugging purposes
        string connection = "URI=file:" + fullPath;
        Debug.Log(connection);
        SqliteConnection dbcon = new SqliteConnection(connection); //opens database from environment path
        dbcon.Open();
        Debug.Log("Connected to the database."); //debugging purposes

        // IDbCommand cmdRead = dbcon.CreateCommand();
        // IDataReader reader;

        string query = "SELECT * FROM ITEMS WHERE IsObtained = 0";
        // cmdRead.CommandText = query;
        // reader = cmdRead.ExecuteReader();

        SqliteDataReader reader;
        SqliteCommand cmdRead;
        cmdRead = dbcon.CreateCommand();
        cmdRead.CommandText = query;
        
        reader = cmdRead.ExecuteReader();
        Debug.Log(reader.GetType());

        while(reader.Read()){
            Debug.Log("type: " + reader[1].ToString() + " summary: " + reader[2].ToString() + " content: " + reader[3].ToString());
        }

        dbcon.Close();


        
    }

    public void openDatabase(){
        string[] paths = {Environment.CurrentDirectory, "Assets", "Scenes", "databases", "testDatabase.db"};
        string fullPath = Path.Combine(paths);
        Debug.Log(fullPath); //Debugging purposes
        string connection = "URI=file:" + fullPath;
        Debug.Log(connection);
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();
        Debug.Log("Connected to the database."); //debugging purposes
    }
}
