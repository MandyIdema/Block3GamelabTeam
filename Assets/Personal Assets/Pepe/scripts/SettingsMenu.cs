using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private Image blackScreen;
    [SerializeField] private Dropdown resolutionsDropDown;
    Resolution[] resolutions;

    void Start(){
        //this is to set the setting in the dropdown to the available resolutions by unity
        resolutions = Screen.resolutions;
        resolutionsDropDown.ClearOptions();
        List<string> options = new List<string>();
        int currentResIndex = 0;
        for(int i=0; i<resolutions.Length; i++){
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);
            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height){
                currentResIndex = i;
            }
        }
        resolutionsDropDown.AddOptions(options);
        resolutionsDropDown.value = currentResIndex;
        resolutionsDropDown.RefreshShownValue();
    }
    public void SetVolume(float volume){

    }
    public void SetBrightness(float brightness){
        var _temp = blackScreen.color;
        _temp.a = brightness;
        blackScreen.color = _temp;
    }
    public void SetFullScreen(bool isFullScreen){
        Screen.fullScreen = isFullScreen;
    }
    public void SetResolution(int resIndx){
        Resolution resolution = resolutions[resIndx];
        Screen.SetResolution(resolution.width,resolution.height,Screen.fullScreen);
    }

}