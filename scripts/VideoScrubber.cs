using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Video;

public class VideoScrubber : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    public RectTransform totalTimeFrame;
    public VideoPlayer videoPlayer;
    
    //If the mouse is dragged (while being clicked) on the timeline bar, the trySkip function will fire
    public void OnDrag(PointerEventData eventData){
        trySkip(eventData);
    }

    //if the mouse is clicked on the timeline bar, the trySkip function will fire
    public void OnPointerDown(PointerEventData eventData){
        trySkip(eventData);
    }

    ///<summary>
    ///Checks to see if the mouse has been clicked on the timeline bar.
    ///</summary>
    private void trySkip(PointerEventData eventData){
        Vector2 localPoint;
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(totalTimeFrame, eventData.position, null, out localPoint)){
            //Debug.Log(localPoint.x); //Debugging

            float percent = Mathf.InverseLerp(totalTimeFrame.rect.xMin, totalTimeFrame.rect.xMax, localPoint.x);
            skipToPercentage(percent);
        }
    }

    ///<summary>
    /// Given that the trySkip test passed, will skip to the specific frame of the video based on
    /// where the user clicked on the timeline (converted to a percentage and passed to the videoplayer).
    ///</summary>
    
    private void skipToPercentage(float pct){
        float frame = videoPlayer.frameCount * pct;
        videoPlayer.frame = (long)frame;
    }
}
