using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
	public AudioSource bgm;
	public static AudioController AC;
	public float maxBGMvol = 0.15f;
    float soundVel = 0;
    float smoothTime = 0.3f;
    private Dictionary<string, AudioClip[]> sounds;

    // Start is called before the first frame update
    void Awake()
    {
        AC = this;
        //bgm.loop = true;
        sounds = new Dictionary<string, AudioClip[]>();
        sounds.Add("bgm", new AudioClip[] { Resources.Load(bgm.clip.name, typeof(AudioClip)) as AudioClip });
        bgm.clip = sounds["bgm"][0];
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    public void PlaySound(float vol = 0.5f){
        
        AudioClip clip = sounds["bgm"][Random.Range(0, sounds["bgm"].Length)];
        bgm.clip = clip;
        
        bgm.volume = 0.5f;
        bgm.Play();
    }

    public void StopSound() 
    {
        bgm.Stop();
    }

    public void muteAll(){
        bgm.volume = 0; 
    }

    public void unmuteAll(){
        bgm.volume = 0.5f; 
    }

}