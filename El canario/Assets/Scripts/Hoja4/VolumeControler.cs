using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;


public class VolumeControler : MonoBehaviour
{
    [SerializeField]
    float vol = 0.05f;

    AudioSource source;

    [SerializeField]
    float time = 2f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    public void changeUp(InputAction.CallbackContext context)
    {
        source.volume += vol;
    }

    public void changeDown(InputAction.CallbackContext context)
    {
        source.volume -= vol;
    }

    public void fadeOut(InputAction.CallbackContext context)
    {
        StartCoroutine(StartFade(source, time, 0));
    }

    public static IEnumerator StartFade(AudioSource source, float time, float goal)
    {
        Debug.Log("Inicia rutina");
        float currentTime = 0;
        float start = source.volume;

        while (currentTime < time)
        {
            Debug.Log("Baja el volumen");
            currentTime += Time.deltaTime;
            //El 0 es el volumen al que queremos llegar podríamos hacer una variable
            source.volume = Mathf.Lerp(start, goal, currentTime / time);
            yield return null;
        }
        yield return null;
    }
}
