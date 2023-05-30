using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClothSetting : MonoBehaviour
{
    [Tooltip("UI objects")]
    [SerializeField] private GameObject blockUI;
    [SerializeField] private GameObject priceUI;
    [Tooltip("Instantiated UI for each object")]
    private GameObject instanceBlock;
    private GameObject instancePrice;
    [Tooltip("Info for shoppingMenu")]
    public int cPrice;
    public bool obtained;
    void Start(){
        if(!obtained){
           instanceBlock = Instantiate(blockUI,gameObject.transform);
           //instancePrice = Instantiate(blockUI,new Vector3(gameObject.transform.position.x,gameObject.transform.position.y-40,gameObject.transform.position.z), Quaternion.identity);
           instancePrice = Instantiate(priceUI,gameObject.transform);
           instancePrice.GetComponent<TextMeshProUGUI>().text = cPrice.ToString();
        }
    }
    public void ShowPrice(){
        if(obtained && instanceBlock!=null){
            instanceBlock.SetActive(false);
            instancePrice.SetActive(false);
        }
    }
}
