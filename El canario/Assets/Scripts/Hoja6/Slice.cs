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
    private AudioSource casing;
 
    public AudioClip[] pcmDataHeads, pcmDataTails, pcmDataCasing;
    private int nHeads, nTails, nCasing;

    [SerializeField]
    float lap;


    void Awake(){
        nHeads = pcmDataHeads.Length;
        nTails = pcmDataTails.Length;
        nCasing = pcmDataCasing.Length;
        head = gameObject.AddComponent<AudioSource>();        
        tail = gameObject.AddComponent<AudioSource>();
        casing = gameObject.AddComponent<AudioSource>();

        // Configuración básica
        head.playOnAwake = false;
        tail.playOnAwake = false;
        casing.playOnAwake = false;
        head.loop = false;
        tail.loop = false;
        casing.loop = false;
    }

    void Start(){

        if (pcmDataHeads != null)
        {
            foreach (AudioClip clip in pcmDataHeads)
            {
                if (clip.channels > 1)
                    Debug.LogError("El clip " + clip.name + " no es mono");
                else
                    FadeOut(clip);
            }
        }

        if (pcmDataTails != null)
        {
            foreach (AudioClip clip in pcmDataTails)
            {
                if (clip.channels > 1)
                    Debug.LogError("El clip " + clip.name + " no es mono");
                else
                    FadeIn(clip); //<3

            }

        }
        
    }

    public void Play(InputAction.CallbackContext context)
    {
        int h = UnityEngine.Random.Range(0, nHeads), 
            t = UnityEngine.Random.Range(0, nTails),
            c = UnityEngine.Random.Range(0, nCasing);

        head.clip = pcmDataHeads[h];
        tail.clip = pcmDataTails[t];
        casing.clip = pcmDataCasing[c];

        double clipLength = (double)head.clip.samples / head.clip.frequency;
        double tailLength = (double)tail.clip.samples / tail.clip.frequency;

        int sRATE = AudioSettings.outputSampleRate;
        double dsp = AudioSettings.dspTime;
        //Debug.Log($"head {h} length {clipLength}  p tail {t}  sRATE: {sRATE}");

        head.PlayScheduled(dsp);
        tail.PlayScheduled(dsp + clipLength - lap);
        casing.PlayScheduled(dsp + tailLength - lap);
    }


    private void FadeIn(AudioClip clip)
    {
        //
        int totalSamples = clip.samples;
        int lapSamples = Mathf.Clamp((int)(clip.frequency * lap), 1, totalSamples - 1); ;

        float[] samples = new float[totalSamples];
        clip.GetData(samples, 0);

       
        for (int i = 0; i < lapSamples; i++)
        {
            Debug.Log(samples[i]);
            float t = (i + 1) / (float)lapSamples;
            samples[i] = samples[i] * Mathf.Sqrt(t);
            Debug.Log(samples[i]);
        }

        clip.SetData(samples, 0);

    }

    private void FadeOut(AudioClip clip)
    {
        //
        int totalsamples = clip.samples;
        int lapSamples = Mathf.Clamp((int)(clip.frequency * lap), 1, totalsamples - 1); ;

        float[] samples = new float[totalsamples];
        clip.GetData(samples, 0);   //Cogemos los samples


        //Aplicamos fadeOut
        int start = totalsamples - lapSamples;  
        for (int i = 0; i < lapSamples; i++)
        {
            float t = (i + 1) / (float)lapSamples;
            samples[start + i] = samples[start + i] * Mathf.Sqrt(1.0f - t);
        }

        clip.SetData(samples, 0);

    }


}
