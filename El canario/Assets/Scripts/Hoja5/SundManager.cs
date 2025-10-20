using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class SundManager : MonoBehaviour
{
    [SerializeField]
    float Itraffic;     //<0.2 no suena ninguna muestra del tráfico

    public void Play(InputAction.CallbackContext context)
    {
        IntermitentSound[] sounds = GetComponentsInChildren<IntermitentSound>();
        foreach (IntermitentSound i in sounds) i.Play();
    }

    public void Stop(InputAction.CallbackContext context)
    {
        IntermitentSound[] sounds = GetComponentsInChildren<IntermitentSound>();
        foreach (IntermitentSound i in sounds) i.Stop();
    }
}
