using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;
using Mirror;

public class SettingsMenu : NetworkBehaviour
{
    [Header("For Exit Menu")]
    [SerializeField] private Image blackScreen;
    [SerializeField] private Dropdown resolutionsDropDown;
    [Header("For Shopping Menu")]
    [SerializeField] private Image[] avatarTransforms;
    [SerializeField] private GameObject[] clothesPrefabs;
    [SerializeField] private TextMeshProUGUI currencyText;
    Resolution[] resolutions;

    void Start(){
        //this is to set the setting in the dropdown to the available resolutions by unity
/*         resolutions = Screen.resolutions;
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
        resolutionsDropDown.RefreshShownValue(); */

        currencyText.text = "Currency: " + XMLManager.instance.LoadStarScore();
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

    public void SetClothing(Image cloth){
        switch(cloth.tag){
            case "head":
                avatarTransforms[0].sprite = cloth.sprite;
                var _a = avatarTransforms[0].color;
                _a.a = 255;
                avatarTransforms[0].color = _a;

            break;
            case "body":
                avatarTransforms[1].sprite = cloth.sprite;
                var _b = avatarTransforms[1].color;
                _b.a = 255;
                avatarTransforms[1].color = _b;
            break;
            case "feet":
                avatarTransforms[2].sprite = cloth.sprite;
                var _c = avatarTransforms[2].color;
                _c.a = 255;
                avatarTransforms[2].color = _c;
            break;
        }
    }

    public void ApplyToPlayer(){
        if(isLocalPlayer){
            Transform childTransform = transform.Find("head");
            Transform childTransform2 = transform.Find("body");
            Transform childTransform3 = transform.Find("feet");
            childTransform.GetComponent<SpriteRenderer>().sprite = avatarTransforms[0].sprite;
            childTransform2.GetComponent<SpriteRenderer>().sprite = avatarTransforms[1].sprite;
            childTransform3.GetComponent<SpriteRenderer>().sprite = avatarTransforms[2].sprite;
        }else{
        }
    }
}