using UnityEngine;

public class playerCollisionObstacle : MonoBehaviour
{
    ///<summary>
    ///Function for when the player collides with an Obstacle object (plays a sound effect)
    ///</sumamry>
    void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Obstacle")
        {
            FindObjectOfType<AudioManager>().Play("Bump");
        }
    }
}
