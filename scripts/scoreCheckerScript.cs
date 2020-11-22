using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scoreCheckerScript : MonoBehaviour
{
    public Text fallText;
    public Text hiddenAreaText;
    public Text stateText;

    private int fallCounter;
    private int hiddenAreaCounter;
    private int stateCounter;

    // Start is called before the first frame update
    void Start()
    {
        updateScore();
    }

    // Update is called once per frame
    void Update()
    {
        updateScore(); //Should not be running this every frame. It's not efficient.
                       //Score should be updated whenever a certain event occurs. Please fix!
    }

    ///<summary>
    ///Function to ensure that the main game counts are updated
    ///</summary>
    void updateScore()
    {
        fallCounter = gameManager.Instance.fallCount;
        hiddenAreaCounter = gameManager.Instance.hiddenAreaCount;
        stateCounter = gameManager.Instance.State;

        fallText.GetComponent<Text>().text = fallCounter.ToString();
        hiddenAreaText.GetComponent<Text>().text = hiddenAreaCounter.ToString();
        stateText.GetComponent<Text>().text = stateCounter.ToString();
    }
}
