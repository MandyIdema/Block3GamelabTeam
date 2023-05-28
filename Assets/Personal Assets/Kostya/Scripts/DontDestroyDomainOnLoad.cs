using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyDomainOnLoad : MonoBehaviour
{
    // Start is called before the first frame update

    public enum DDOLTypes
    {
        Default,
        AvoidDuplicates
    }

    public DDOLTypes DDOLType = DDOLTypes.Default;
    private void Start()
    {
        switch (DDOLType)
        {
            case DDOLTypes.Default:
                DefaultDDOL();
                break;
            case DDOLTypes.AvoidDuplicates:
                AvoidDuplicatesDDOL();
                break;
        }
    }

    void AvoidDuplicatesDDOL()
    {

        DontDestroyOnLoad(gameObject);

        if (GameObject.Find(gameObject.name) && GameObject.Find(gameObject.name) != this.gameObject)
        {
            Destroy(GameObject.Find(gameObject.name));
        }
    }

    void DefaultDDOL()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
