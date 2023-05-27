using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothSetting : MonoBehaviour
{
    [HideInInspector]public ClothingInformation cInfo;
    [SerializeField] private GameObject whatever;
    public int cPrice;
    void Start(){
        cInfo.name = gameObject.name;
        cInfo.price = cPrice;
    }
    public void ShowPrice(){
/*         Debug.Log("xmlmanager"+XMLManager.instance.ugStats.starsCollectedInTotal);
        Debug.Log("cInfo"+cInfo.obtained);  
        Debug.Log("xmlManager"+XMLManager.instance.ugStats.obtainedClothes[0].Value); */
        if(cInfo.obtained){
            whatever.SetActive(false);
        }
    }

    [System.Serializable]
    public class ClothingInformation{
        public string name;
        public int price;
        public bool obtained;
    }
}
