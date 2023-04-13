using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterInformation : ScriptableObject
{
    public string Name;
    public float score; //number of stars

    

    public virtual void Awake()
    {
        score = 0;
    }
}
