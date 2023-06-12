using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] AudioSource musicMenu;
    [SerializeField] AudioSource SFX;
    [Header("Audio Clips")]
    [SerializeField] AudioClip bg;
    void Start(){
        musicMenu.clip = bg;
        musicMenu.Play();
    }
}
