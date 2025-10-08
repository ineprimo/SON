using UnityEngine;
using UnityEngine.InputSystem;


public class VolumeControler : MonoBehaviour
{
    [SerializeField]
        float vol = 0.05f;
    AudioSource source;

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
}
