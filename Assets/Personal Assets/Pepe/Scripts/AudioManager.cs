using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource musicMenu;
    public AudioSource addOn;
    public AudioSource SFX;

    [Header("Audio Clips")]
    [SerializeField] AudioClip bg;
    [SerializeField] AudioClip bgGame;
    [SerializeField] AudioClip bgGameHarp;
    public AudioClip attain;
    [SerializeField] AudioClip buttonClick;
    [SerializeField] AudioClip kickBall;
    public AudioClip endSound;
    [SerializeField] AudioClip rightAnswer;
    [SerializeField] AudioClip wrongAnswer;
    
    void Start(){
        PlayMusic();
        addOn.clip = bgGameHarp;
    }

    public void StopMusic(){
        musicMenu.Stop();
    }
    public void PlayMusic(){
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        if(sceneName == "LobbyScene"){
            musicMenu.clip = bg;
        }else if(sceneName == "GameScene" || sceneName == "EndScene"){
            musicMenu.clip = bgGame;
        }
        musicMenu.Play();
    }

    public void WrongAnswer(){
        SFX.clip = wrongAnswer;
        SFX.Play();
    }

    public void RightAnswer(){
        SFX.clip = rightAnswer;
        SFX.Play();
    }

    public void KickBall(){
        SFX.clip = kickBall;
        SFX.Play();
    }

    public void Click(){
        SFX.clip = buttonClick;
        SFX.Play();
    }
}
