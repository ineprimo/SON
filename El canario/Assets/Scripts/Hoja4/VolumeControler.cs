using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class VolumeControler : MonoBehaviour
{
    [SerializeField]
    float vol = 0.05f;

    [SerializeField]
    float time = 2f;

    [SerializeField]
    AudioSource [] source;

    AudioSource act;

    bool canFade = true;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < source.Length; i++)
            source[i].volume = 0;
        act = source[0];
        act.volume = 1;
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void changeUp(InputAction.CallbackContext context)
    {
        act.volume += vol;
    }

    public void changeDown(InputAction.CallbackContext context)
    {
        act.volume -= vol;
    }

    public void fadeOut(InputAction.CallbackContext context)
    {
        if (canFade)
        {
            StartCoroutine(AudioFade());
        }

    }

    public static IEnumerator FadeIn(AudioSource source, float time, float goal)
    {
        float currentTime = 0;
        float start = source.volume;

        while (currentTime < time)
        {
            currentTime += Time.deltaTime;
            // formula del equal power cross fade in
            float a = (float)Math.Sqrt(currentTime /time);
            // chequeamos si sale NaN porque si no peta, cuando haya llegado a NaN 100% ya esta en el valor final asi que lo setteamos sin mas
            if (float.IsNaN(a)) {
                source.volume = 1;
            }
            else {
                source.volume = Mathf.Lerp(start, goal, a); 
            }
            yield return null;
        }


        yield return null;
    }
    public static IEnumerator FadeOut(AudioSource source, float time, float goal)
    {
        float currentTime = 0;
        float start = source.volume;

        while (currentTime < time)
        {
            currentTime += Time.deltaTime;
            // formula del equal power cross fade out
            float a = (float)Math.Sqrt((time - currentTime) /time);
            // chequeamos si sale NaN porque si no peta, cuando haya llegado a NaN 100% ya esta en el valor final asi que lo setteamos sin mas
            if (float.IsNaN(a)) {
                source.volume = 0;
            }
            else {
                source.volume = 1 - Mathf.Lerp(start, goal, a);
            }
            yield return null;
        }

        
        yield return null;
    }

    public IEnumerator AudioFade()
    {
        canFade = false;
        StartCoroutine(FadeOut(act, time, 0));
        changeSong();
        yield return StartCoroutine(FadeIn(act, time, 1));
        canFade = true;

    }
    

    void changeSong()
    { 
        act = source[UnityEngine.Random.Range(0, source.Length)];
    }

}
