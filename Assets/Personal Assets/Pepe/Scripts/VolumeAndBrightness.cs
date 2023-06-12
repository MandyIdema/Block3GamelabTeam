using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeAndBrightness : MonoBehaviour
{
    [SerializeField] private Image blackScreen;
    public AudioMixer audioMixer;

    // Start is called before the first frame update
    public void SetVolume(float volume){
        audioMixer.SetFloat("volume", volume);
    }
   public void SetBrightness(float brightness){
        var _temp = blackScreen.color;
        _temp.a = 255 - brightness;
        blackScreen.color = _temp;
    }
}
