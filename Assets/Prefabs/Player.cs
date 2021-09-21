using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Player : MonoBehaviour
{
    [SerializeField] float WalkingSpeed = 5f;
    InputActions inputActions;
    Vector2 MoveInput;
    Vector3 Velocity;
    CharacterController characterController;
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
        characterController = GetComponent<CharacterController>();
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
        Velocity = GetPlayerDesiredMoveDir() * WalkingSpeed;
        characterController.Move(Velocity * Time.deltaTime);
    }

    Vector3 GetPlayerDesiredMoveDir()
    {
        return new Vector3(-MoveInput.y, 0f, MoveInput.x).normalized;
    }
}
