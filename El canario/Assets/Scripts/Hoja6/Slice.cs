using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEditor;
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
        FadeOut();
        FadeIn();
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
        Debug.Log($"head {h} length {clipLength}  p tail {t}  sRATE: {sRATE}");

        head.Play();
        tail.PlayScheduled(AudioSettings.dspTime + clipLength / sRATE);
    }

    private void FadeIn()
    {
        foreach (AudioClip clip in pcmDataTails)
        {
            int sR = clip.frequency;

            float[] samples = new float[clip.samples];
            clip.GetData(samples, 0);


            for (int i = 0; i < samples.Length; i++)
            {
                samples[i] *= Mathf.Lerp(0, 1, sR*lap);
            }
        }
    }

    private void FadeOut()
    {
        foreach (AudioClip clip in pcmDataHeads)
        {
            int sR = clip.frequency;

            float[] samples = new float[clip.samples];
            clip.GetData(samples, 0);

            for (int i = 0; i < samples.Length; i++)
            {
                ///Debug.Log(Mathf.Lerp(0, 1, (float) sR * lap));
                samples[i] *= Mathf.Lerp(1, 0, (float) sR * lap);
                
            }

            clip.SetData(samples, 0);
        }
    }
  

}
