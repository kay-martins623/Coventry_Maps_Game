using UnityEngine;

public class TimeElapsedScript : MonoBehaviour
{
    public RectTransform timeElaped;
    private RectTransform rectTransform;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.sizeDelta.Set(0f, rectTransform.sizeDelta.y);
    }
    
    public void moveTimeElapsed(double playedFraction){
        rectTransform.sizeDelta.Set((timeElaped.sizeDelta.x * (float)playedFraction), rectTransform.sizeDelta.y);
    }
}
