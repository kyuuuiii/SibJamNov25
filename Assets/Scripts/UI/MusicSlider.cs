using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicSlider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Slider>().value = SoundManager.Instance.musicSource.volume;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void SoundsVolumeChange() 
    {
        SoundManager.Instance.musicSource.volume = gameObject.GetComponent<Slider>().value;
    }
}
