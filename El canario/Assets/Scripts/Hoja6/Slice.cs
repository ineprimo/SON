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
        int lapSamples = (int)(clip.frequency * lap);   // tiempo total en samples (samples totales del fade in)
        float time = 0;                                 // current time relativo a samples (sample actual)

        float[] samples = new float[totalSamples];
        clip.GetData(samples, 0);
       
        for (int i = 0; i < lapSamples; i++)
        {
            samples[i] = samples[i] * Mathf.Sqrt(time);
            time++;
        }

        clip.SetData(samples, 0);

    }

    private void FadeOut(AudioClip clip)
    {
        //
        int totalsamples = clip.samples;
        int lapSamples = (int)(clip.frequency * lap);   // tiempo total en samples (samples totales del fade out)
        float time = 0;                                 // current time relativo a samples (sample actual)

        float[] samples = new float[totalsamples];
        int offset = samples.Length - lapSamples;

        clip.GetData(samples, offset);   //Cogemos los samples


        //Aplicamos fadeOut
        for (int i = 0; i < samples.Length; i++)
        {
            samples[i] = samples[i] * Mathf.Sqrt((lapSamples - time)/lapSamples);
            time++;
        }

        clip.SetData(samples, offset);
    }


}
