using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float DelayTime = 2f; 
    [SerializeField] AudioClip crashSFX;

    [SerializeField] AudioClip successSFX;

    [SerializeField] ParticleSystem successParticles;

    [SerializeField] ParticleSystem crashParticles;
    [SerializeField] Light successLight;
    [SerializeField] Light crashLight;

    AudioSource audioSource;


    bool isControllable = true;
    bool isCollidable = true;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>(); 
        successLight.enabled = false;
        crashLight.enabled = false;
        
    }
    private void Update()
    {
        ResponceToDebugKeys();
    }
    private void OnCollisionEnter(Collision other) 
    {
        if (!isControllable || !isCollidable)
        { 
            return;
        }
        
        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("Friendly");
                break;
            case "Fuel":
                Debug.Log("game");
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartCrashSequence();

                break;
        }
    }
    void StartSuccessSequence()
    {
        isControllable = false;
        GetComponent<Movement>().enabled = false;
        
         if (!audioSource.isPlaying)
            {
              audioSource.PlayOneShot(successSFX);
            }
        else
        {
            audioSource.Stop();
        }
        successParticles.Play();
        successLight.enabled = true;
        
        Invoke("NextLevel", DelayTime);
    }
    void StartCrashSequence()
    {
        
        GetComponent<Movement>().enabled = false;
        if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(crashSFX);
            }
        else
        {
            audioSource.Stop();
        }
        crashParticles.Play();
        crashLight.enabled = true;

        Invoke("ReloadLevel", DelayTime);
    }
    void ReloadLevel()
    {
        int CurrentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(CurrentScene);
    }
    void NextLevel()
    {
        int CurrentScene = SceneManager.GetActiveScene().buildIndex;
        int nextScene = CurrentScene + 1;

        if (nextScene == SceneManager.sceneCountInBuildSettings)
        {
            nextScene = 0;
        }

        SceneManager.LoadScene(nextScene);
    }
    void ResponceToDebugKeys()
    {
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            NextLevel();
        }
        else if(Keyboard.current.cKey.wasPressedThisFrame)
        {
            isCollidable = !isCollidable; 
        }
    }
}