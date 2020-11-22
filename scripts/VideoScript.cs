using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class VideoScript : MonoBehaviour
{
    public VideoPlayer videoPlayer; //the video player game object for all video-related actions
    public Button pausePlay; //the button for the pause/play function
    public Sprite pauseIcon; //pause icon sprite
    public Sprite playIcon; //play icon sprite
    public Button backToInventory; //button object to take user back to inventory
    public GameObject inventoryScreen;
    public KeyCode exitKey; //the key pressed that will take the user back to inventory

    //These are for the numbers displaying the time of the video. Current and total time in mm:ss/mm:ss
    public Text currentMinutes;
    public Text currentSeconds;
    public Text totalMinutes;
    public Text totalSeconds;

    //These are for the visualisation bars of the video to give the user a rough idea
    //How far they are into the video
    public RectTransform totalTimeFrame;
    public RectTransform timeElapsedFrame;

    // Start is called before the first frame update
    void Start()
    {
        //The elapsed rectangle (that is red) will have 0 width at the beginning.
        //play/pause function is added to the playpause button and the video is prepared and played through a coroutine
        timeElapsedFrame.sizeDelta = new Vector2(0f, timeElapsedFrame.sizeDelta.y);
        pausePlay.onClick.AddListener(() => playPause());
        pausePlay.GetComponent<Button>().image.sprite = pauseIcon;

        if(videoPlayer.isPrepared){
            videoPlayer.frame = 0;
            videoPlayer.Play();
        }
        else{
            StartCoroutine(loadVideo());
        }
        backToInventory.onClick.AddListener( () => fromVideoToInventory(inventoryScreen)) ;
    }
    

    // Update is called once per frame
    void Update()
    {
        //The current time and elapsed rectangle width will update while the video is playing
        if(videoPlayer.isPlaying){
            setCurrentTime();
            moveTimeElapsed(calculatePlayedFraction());
        }

        if(Input.GetKeyDown(exitKey)){
            fromVideoToInventory(inventoryScreen);
        }

        //DEBUGGING PURPOSES
        // Debug.Log("Time elapsed width: " + timeElapsedFrame.sizeDelta.x);
        // Debug.Log("size of time elapsed: " + timeElapsedFrame.sizeDelta);
        // Debug.Log("frame " + (double)videoPlayer.frame + "/" + (double)videoPlayer.frameCount);
        
    }

    //The video needs to be prepared before the "time elapsed" function can work
    //This will run in the background and will set the total time AND play the video once everything is prepared
    IEnumerator loadVideo(){
        videoPlayer.Prepare();
        while(!videoPlayer.isPrepared){
            if(videoPlayer.isPlaying){
                videoPlayer.Pause();
            }
            yield return null;
        }
        videoPlayer.Play();
        setTotalTime();
    }

    ///<summary>
    ///Simple pause/play fucntion on video while switching play and pause icons based on
    ///what the video state is
    ///</summary>
    public void playPause(){
        if(videoPlayer.isPlaying){
            pausePlay.GetComponent<Button>().image.sprite = playIcon;
            videoPlayer.Pause();
        }
        else{
            pausePlay.GetComponent<Button>().image.sprite = pauseIcon;
            videoPlayer.Play();
        }
    }

    ///<summary>
    ///Sets the current frame the video is on into minutes:seconds (mm:ss)
    ///</summary>
    void setCurrentTime(){
        string minutes = Mathf.Floor((int)videoPlayer.time / 60).ToString("00");
        string seconds = Mathf.Floor((int)videoPlayer.time % 60).ToString("00");

        currentMinutes.text = minutes;
        currentSeconds.text = seconds;
    }

    ///<summary>
    ///Sets the sum of the frames in the video into minutes:seconds (mm:ss)
    ///</summary>
    void setTotalTime(){
        string minutes = Mathf.Floor((int)videoPlayer.length / 60).ToString("00");
        string seconds = Mathf.Floor((int)videoPlayer.length % 60).ToString("00");
        totalMinutes.text = minutes;
        totalSeconds.text = seconds;
    }

    ///<summary>
    ///returns a fraction of the completion of the video (current frame/frame count)
    ///</summary>
    double calculatePlayedFraction(){
        double fraction = (double)videoPlayer.frame / (double)videoPlayer.frameCount;
        return fraction;
    }

    ///<summary>
    ///Adjusts elapsed rectangle based on the completion fraction of the video
    ///</sumamry>
    void moveTimeElapsed(double playedFraction){
        timeElapsedFrame.sizeDelta = new Vector2((totalTimeFrame.sizeDelta.x * (float)playedFraction), totalTimeFrame.sizeDelta.y);
    }

    ///<summary>
    ///Given the inventory GameObject, takes the user back to inventory screen
    ///</summary>
    void fromVideoToInventory(GameObject inventory){
        this.gameObject.SetActive(false);
        inventory.SetActive(true);        
        
    }
}
