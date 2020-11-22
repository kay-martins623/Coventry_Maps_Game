using UnityEngine;

public class MiniMapBuilder : MonoBehaviour
{
    //The icon prefab that will be used
    public GameObject miniMapIcon;
    //The player's material to be added via script
    public Material playerMaterial;
    //The obstacle's material to be added via script
    public Material obstacleMaterial;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        /*This collects all of the objects under the "Obstacle" tag and puts them into an array.
        Then, for each obstacle object, an icon prefab is created as a child of the object, giving the obstacle material and positioning it
        above the 3D model. It is rotated to face the minimap camera. It is also assigned to the "minimap icons" layer so the minimap camera
        can render it*/
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

        for(int i = 0; i < obstacles.Length; i++){
            var icon = Instantiate(miniMapIcon, new Vector3(0, 3, 0), new Quaternion(90, 0, 0, 1), obstacles[i].transform) as GameObject;
            icon.gameObject.layer = 8;
            icon.transform.position = new Vector3(obstacles[i].transform.position.x, obstacles[i].transform.position.y + 3, obstacles[i].transform.position.z);
            icon.transform.Rotate(-90, 0, 0);   
            icon.GetComponent<Renderer>().sharedMaterial = obstacleMaterial;
        }

        //This is pretty much the same as the one above, but for the player instead. The player has its separate icon.
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        var playerIcon = Instantiate(miniMapIcon, new Vector3(0, 3, 0), new Quaternion(90, 0, 0, 1), player.transform) as GameObject;
        playerIcon.gameObject.layer = 8;
        playerIcon.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 3, player.transform.position.z);
        playerIcon.transform.Rotate(-90, 0, 0);
        playerIcon.GetComponent<Renderer>().sharedMaterial = playerMaterial;  
    }
}
