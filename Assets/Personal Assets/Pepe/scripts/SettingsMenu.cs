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
    [SerializeField] private Image[] clothesUI;
    [SerializeField] private XMLManager saveSystem;
    [SerializeField] private TextMeshProUGUI currencyText;
    Resolution[] resolutions;

    #region SettingsMenu
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

    void Update(){
        if(currencyText){
            currencyText.text = "Currency: " + XMLManager.instance.LoadStarScore();
        }
    }
    #endregion

    #region ClothingMenu

    void Awake(){
        saveSystem.LoadOutfits();
        var _listOfObtainedClothes = saveSystem.obtainedClothes;
        for(var i = 0; i<_listOfObtainedClothes.Count;i++){
            if(_listOfObtainedClothes[i]){
                clothesUI[i].GetComponent<ClothSetting>().obtained = true;
            }
        }
    }

    public void SetClothing(Image cloth){
        if(cloth.GetComponent<ClothSetting>().obtained){
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
    }

    public void ApplyToPlayer(){
        //put it on the prefab model of the player
    }

    public void GetCloth(Image Cloth)
    {
        try{
            //variables for this function
            var _cInfo = Cloth.GetComponent<ClothSetting>();
            var _currency = XMLManager.instance.LoadStarScore();
            var _item = Cloth.GetComponent<ClothSetting>().cPrice;
            var _orderItem = -1;

            //find the orderNum for XML list
            for (var i=0;i<clothesPrefabs.Length;i++){
                if(clothesPrefabs[i].name == _cInfo.name){
                    _orderItem = i;
                    break;
                }
            }

            //deal
            if(_currency >= _item && !saveSystem.obtainedClothes[_orderItem]){
                _currency -=_item;
                Cloth.GetComponent<ClothSetting>().obtained = true;
                XMLManager.instance.SaveStarScoreShop(_currency);
                saveSystem.obtainedClothes[_orderItem] = true;
                XMLManager.instance.SaveOutfits();
                Debug.Log("MADE DEAL");
            }
            
            //exception
        }catch(System.NullReferenceException e){
            Debug.Log("Missing ClothSetting component"+e.Message);
        }
    }

    #endregion
}