using UnityEngine;
using UnityEngine.InputSystem;

public class movement : MonoBehaviour
{
    CharacterController characterController;
    Vector2 moveInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movePos = new Vector3(moveInput.x, 0, moveInput.y);
        characterController.Move(movePos * Time.deltaTime * 5);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
}
