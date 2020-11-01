using UnityEngine.Audio;
using System;
using UnityEngine;

public class audioManager : MonoBehaviour
{
    public sound[] sounds;

    public static audioManager instance;
    void Awake()    
    {
        //the audio manager does not get lost as the scene changes
        if (instance==null)
        {
            instance = this;    
        }
        else
        {
            //if there is another auido manager in the changed scene delete it
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);


        
        foreach(sound s in sounds)
        {
            s.source=gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.pitch = s.pitch;
            s.source.volume = s.volume;
        }
    }

    //We can call the sound by typing its name in anywhere
    public void Play(string name)
    {
        sound s=Array.Find(sounds, sound => sound.name == name);
        s.source.Play(); 
    }
}
