using UnityEngine;

[System.Serializable]
public class Sound
{
    //attributes for the sound
    public string name; //sound name
    public AudioClip clip; //sound file

    [Range(0f, 1f)]
    public float volume; //a slider for the volume from 0f->1f
    [Range(0.1f, 3f)]
    public float pitch; //a slider for the pitch from 0.1f->3f

    public bool loop; //an option to toggle loop

    [HideInInspector] //hides value since this will be added automatically via AudioManager.cs
    public AudioSource source;

    
}
