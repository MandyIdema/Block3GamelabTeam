using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Analytics : MonoBehaviour
{

    public TMP_Text goed;
    public TMP_Text fout;

    public static int AnalyticsRight;

    public static int AnalyticsWrong;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        goed.SetText(AnalyticsRight.ToString());
        fout.SetText(AnalyticsWrong.ToString());
    }
}
