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
    [SerializeField] private XMLManager saveSystem;
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
    }

    void Update(){
        if(currencyText){
            currencyText.text = "Currency: " + XMLManager.instance.LoadStarScore();
        }
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
        //put it on the prefab model of the player
    }

    public void GetCloth(Image Cloth){
        var nameOfCloth = Cloth.name;
        var _clothNum = -1;
/*         for(int i=0;i<clothesPrefabs.Length;i++){
            if(nameOfCloth==clothesPrefabs[i].name){
                if(!saveSystem.ugStats.obtainedClothes[i].Value){
                    _clothNum = i;
                    Debug.Log(_clothNum);
                    break;
                }
            }
        } */

        //we still dont have prices but oh well
        //compares how much money the player has and the price
        if(Cloth.GetComponent<ClothSetting>() && !Cloth.GetComponent<ClothSetting>().cInfo.obtained){
            var _currency = XMLManager.instance.LoadStarScore();
            var _item = Cloth.GetComponent<ClothSetting>().cPrice;
            if(_currency>=_item){
                _currency-=_item;
                XMLManager.instance.SaveStarScoreShop(_currency);
                Cloth.GetComponent<ClothSetting>().cInfo.obtained = true;
                XMLManager.instance.AddToInventory(Cloth.GetComponent<ClothSetting>().cInfo);
                //Debug.Log("IT WORKS");
            }
        }
    }
}