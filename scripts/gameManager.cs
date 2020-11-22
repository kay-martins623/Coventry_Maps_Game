using UnityEngine;

public class gameManager : MonoBehaviour
{
    //using the singleton design pattern, make one instance of the game properties
    //this is so there is not more than one instance of something that does not need
    //multiple things. We only need one instance of the game manager and its properties
    private static gameManager _instance;

    public static gameManager Instance
    {
        //If an instance of the singleton class does not already exist, create one and return it
        //otherwise, just return the existing one, rather to create another instance of it
        get
        {
            if(_instance == null)
            {
                GameObject go = new GameObject("gameManager");
                go.AddComponent<gameManager>();
            }
            return _instance;
        }
    }

    //created a fall count and a hidden area count, to which you can access and change the 
    //values of when needed. These will be used to keep the score going
    public int fallCount { get; set; }
    public int hiddenAreaCount { get; set; }

    public int State {get; set;}

    //created visted booleans for the areas visited. Code has been created that uses these 
    //values so it knows not to increment the "places visited" counter
    public bool area1Visited { get; set; }
    public bool area2Visited { get; set; }

    public bool area3Visited { get; set; }

    void Awake()
    {
        _instance = this;

    }

    void Start()
    {
        State = 0;
        fallCount = 0;
        hiddenAreaCount = 0;
        area1Visited = false;
        area2Visited = false;
        area3Visited = false;
    }
}
