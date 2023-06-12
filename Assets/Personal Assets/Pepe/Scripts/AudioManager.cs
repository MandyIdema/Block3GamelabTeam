using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource musicMenu;
    public AudioSource SFX;

    [Header("Audio Clips")]
    [SerializeField] AudioClip bg;
    [SerializeField] AudioClip bgGame;
    public AudioClip attain;
    [SerializeField] AudioClip buttonClick;
    [SerializeField] AudioClip kickBall;
    [SerializeField] AudioClip endSound;
    [SerializeField] AudioClip rightAnswer;
    [SerializeField] AudioClip wrongAnswer;
    
    void Start(){
        PlayMusic();
    }

    public void StopMusic(){
        musicMenu.Stop();
    }
    public void PlayMusic(){
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        if(sceneName == "LobbyScene"){
            musicMenu.clip = bg;
        }else if(sceneName == "GameScene"){
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
