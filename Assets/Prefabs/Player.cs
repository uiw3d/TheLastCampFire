using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Player : MonoBehaviour
{
    InputActions inputActions;
    Vector2 MoveInput;
    private void Awake()
    {
        inputActions = new InputActions(); 
    }
    private void OnEnable()
    {
        inputActions.Enable();
    }
    private void OnDisable()
    {
        inputActions.Disable();
    }
    // Start is called before the first frame update
    void Start()
    {
        inputActions.Gameplay.Move.performed += MoveInputUpdated;
        inputActions.Gameplay.Move.canceled += MoveInputUpdated;
    }

    void MoveInputUpdated(InputAction.CallbackContext ctx)
    {
        MoveInput = ctx.ReadValue<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log($"player move input is: {MoveInput}");
    }
}
