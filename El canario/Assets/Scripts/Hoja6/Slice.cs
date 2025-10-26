using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;
using static Unity.VisualScripting.Member;

public class SchedEvent: MonoBehaviour {
    private AudioSource head;
    private AudioSource tail;
    //public AudioClip sound01, sound02;
    public AudioClip[] pcmDataHeads, pcmDataTails;
    private int nHeads, nTails;

    [SerializeField]
    float lap;


    void Awake(){
        nHeads = pcmDataHeads.Length;
        nTails = pcmDataTails.Length;
        head = gameObject.AddComponent<AudioSource>();        
        tail = gameObject.AddComponent<AudioSource>();   
    }

    void Start(){

        foreach (AudioClip clip in pcmDataTails)
        {
            if (clip.channels > 1)
            {
                Debug.LogError("El clip " + clip.name + " no es mono");
            } 
            else
               FadeIn(clip);
        }
        foreach (AudioClip clip in pcmDataHeads)
        {
            if (clip.channels > 1)
            {
                Debug.LogError("El clip " + clip.name + " no es mono");
            }
            //else 
                //FadeOut(clip);
        }


    }

    // Update is called once per frame
    void Update(){
        
    }

    public void Play(InputAction.CallbackContext context)
    {
        int h = UnityEngine.Random.Range(0, nHeads), t = UnityEngine.Random.Range(0, nTails);
        head.clip = pcmDataHeads[h];
        tail.clip = pcmDataTails[t];

        double clipLength = (double)head.clip.samples / head.pitch;

        int sRATE = AudioSettings.outputSampleRate;
        //Debug.Log($"head {h} length {clipLength}  p tail {t}  sRATE: {sRATE}");

        head.Play();
        tail.PlayScheduled(AudioSettings.dspTime + clipLength / sRATE);
    }


    private void FadeIn(AudioClip clip)
    {
        //
        float[] samples = new float[clip.samples];
        clip.GetData(samples, 0);

        //
        int lapSamples = (int)(clip.frequency * lap);   // tiempo total en samples (samples totales del fade in)
        float time = 0;                                 // current time relativo a samples (sample actual)

        for (int i = 0; i < lapSamples; i++)
        {
            float a = samples[i];
            samples[i] = samples[i] * Mathf.Sqrt(time/lapSamples);
            time += lap / lapSamples;   // current time relativo a samples
            //Debug.Log("before : " + a + " || after: " + samples[i]);

        }

        clip.SetData(samples, 0);

    }

    private void FadeOut(AudioClip clip)
    {
        //
        float[] samples = new float[clip.samples];
        clip.GetData(samples, 0);

        //
        int lapSamples = (int)(clip.frequency * lap);   // tiempo total en samples (samples totales del fade out)
        float time = 0;                                 // current time relativo a samples (sample actual)

        // float a = (float)Math.Sqrt((time - currentTime) /time);

        for (int i = 0; i < lapSamples; i++)
        {
            float a = samples[i];
            samples[i] = samples[i] * Mathf.Sqrt((lapSamples - time)/lapSamples);
            time += lap / lapSamples;   // current time relativo a samples
            //Debug.Log("before : " + a + " || after: " + samples[i]);

        }

        clip.SetData(samples, 0);

    }


}
