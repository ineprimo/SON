using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class SoundManager : MonoBehaviour
{

    [Header("___TRAFICO___")]
    [SerializeField]
    AudioSource traffic_pad;

    [SerializeField]
    IntermitentSound passingDis;

    [SerializeField]
    IntermitentSound trainDis;

    [SerializeField]
    IntermitentSound hornSirenDis;


    [Header("___CONVERSACIÓN___")]
    [SerializeField]
    AudioSource chatter_pad;

    [SerializeField]
    IntermitentSound chatterDis;


    [Header("___CONTROLADORES___")]
    [SerializeField]
    [Range(0f, 1f)]
    float Itraffic;
    [SerializeField]
    [Range(0f, 1f)]
    float Ichatter;

    bool playing = false;

    private void Update()
    {
        if (playing)
        {
            UpdateTraffic();
            UpdateChatter();
        }
    }

    void UpdateTraffic()
    {

        //Controlamos el traffic_pad
        traffic_pad.volume = Mathf.Lerp(0f, 1f, Itraffic);  

        //Manejamos los dispersores
        //Hacemos las cosas con el lerp para que sea gradual
        if (Itraffic >= 0.2f)
        {
            //PASSING
            passingDis.SourceVol = Mathf.Lerp(0.2f, 1f, Itraffic);  //Aumentamos el volumen

            //Aumentamos la probabilidad de aparicion
            passingDis.minTime = Mathf.Lerp(10f, 0.5f, Itraffic);
            passingDis.maxTime = Mathf.Lerp(20f, 1.5f, Itraffic);

            //TRAIN
            trainDis.SourceVol = Mathf.Lerp(0.1f, 0.8f, Itraffic); //Aumentamos el volumen

            //Aumentamos la frecuencia pero en menor medida que el passing
            trainDis.minTime = Mathf.Lerp(20f, 5f, Itraffic); 
            trainDis.maxTime = Mathf.Lerp(40f, 10f, Itraffic);

            //SIREN + HORN
            if (Itraffic >= 0.5f)
            {
                hornSirenDis.SourceVol = Mathf.Lerp(0.3f, 1f, Itraffic);    //Aumentamos el volumen

                //Aumentamos la frecuencia
                hornSirenDis.minTime = Mathf.Lerp(25f, 5f, Itraffic);
                hornSirenDis.maxTime = Mathf.Lerp(50f, 15f, Itraffic);
            }
            else
                hornSirenDis.SourceVol = 0f;
        }
        else
        {
            passingDis.SourceVol = 0f;
            trainDis.SourceVol = 0f;
            hornSirenDis.SourceVol = 0f;
        }
    }

    void UpdateChatter()
    {
        //Controlamos el chatter_pad
        chatter_pad.volume = Mathf.Lerp(0f, 1f, Ichatter);

        if (Ichatter >= 0.5f)
        {
            chatterDis.SourceVol = Mathf.Lerp(0.3f, 1f, Ichatter);  //Aumentamos el volumen

            //Aumentamos la probabilidad
            chatterDis.minTime = Mathf.Lerp(10f, 1f, Ichatter);
            chatterDis.maxTime = Mathf.Lerp(20f, 2f, Ichatter);
        }
        else
            chatterDis.SourceVol = 0f;
    }

    public void Play(InputAction.CallbackContext context)
    {
        passingDis.Play();
        trainDis.Play();
        hornSirenDis.Play();
        chatterDis.Play();

        traffic_pad.Play();
        traffic_pad.volume = 0f;

        chatter_pad.Play();
        chatter_pad.volume = 0f;

        playing = true;
    }

    public void Stop(InputAction.CallbackContext context)
    {
        passingDis.Stop();
        trainDis.Stop();
        hornSirenDis.Stop();
        chatterDis.Stop();

        traffic_pad.Stop();

        chatter_pad.Stop();

        playing = false;
    }
}
