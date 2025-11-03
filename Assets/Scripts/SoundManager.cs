using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public float musicVolume = 1f;
    //public float soundsVolume = 1f;
    //public AudioSource soundSource;
    public AudioSource musicSource;
    //public AudioSource deathSoundSource;
    //public Transform playerTransform; // —сылка на игрока

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        AudioSource[] audioSources = GetComponents<AudioSource>();
        //soundSource = audioSources[0];
        musicSource = audioSources[0];
        //deathSoundSource = audioSources[2];
        //playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start()
    {
        //soundSource.ignoreListenerPause = true;
        //soundSource.ignoreListenerVolume = true;

        musicSource.ignoreListenerPause = true;
        musicSource.ignoreListenerVolume = true;

        musicSource.loop = true;

       // playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    //public void PlaySound(AudioClip clip)
    //{
    //    soundSource.PlayOneShot(clip);
    //}
    public void PlayMusic(AudioClip musicClip)
    {
        musicSource.clip = musicClip;
        musicSource.Play();
        
    }
    //public void PlayDeathSound(AudioClip musicClip)
    //{
    //    deathSoundSource.clip = musicClip;
    //    soundSource.mute = true;
    //    musicSource.mute = true;
    //    deathSoundSource.Play();

    //}
}
