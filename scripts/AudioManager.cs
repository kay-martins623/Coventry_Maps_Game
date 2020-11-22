using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //creates an array of sound objects
    //the size will determine how many sound objects there are
    public Sound[] sounds;

    private AudioSource[] allAudio;
  
    void Awake()
    {
        foreach(Sound s in sounds)
        {
            //instantiates the audio source, adding the component, 
            //the clip file, volume and pitch
            s.source =gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;

        }
        allAudio = FindObjectsOfType(typeof(AudioSource)) as AudioSource[]; //adds all audiosource objects 
    }

    //given the string of a audio clip name, plays the audio clip
    public void Play(string name)
    {
        //finds the sound object in the array of sounds you have
        //wouldn't really do this for a large scale game, though
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }

    public void Pause(string name)
    {
        //finds the sound object in the array of sounds you have
        //wouldn't really do this for a large scale game, though
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Pause();
    }

    ///<summary>
    ///Stops all audio currently playing in the game
    ///</summary>
    public void stopAllAudio(){
        for(int i = 0; i < allAudio.Length; i++){
            allAudio[i].Stop();
        }

    }

    void Start()
    {
        Play("Theme");
    }
}
