using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }
    private AudioSource source;

    private void Awake()
    {

        source = GetComponent<AudioSource>();

        //keep this object even when we go into a new scene
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        //DESTROY DUPLICATE
        else if (instance != null && instance != this)
            Destroy(gameObject);
    }
    public void PlaySound(AudioClip _sound)
    {
        source.PlayOneShot(_sound);
    }
}