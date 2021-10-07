using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Player : MonoBehaviour
{
    
    
    [SerializeField] Transform PickuipSocketTransform;
    InputActions inputActions;

    LadderCimbingComponent climbingComp;
    MovementComponent movementComp;

    public Transform GetPickupSocketTransform()
    {
        return PickuipSocketTransform;
    }
   
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
        movementComp = GetComponent<MovementComponent>();
        climbingComp = GetComponent<LadderCimbingComponent>();
        climbingComp.SetInput(inputActions);

        inputActions.Gameplay.Move.performed += MoveInputUpdated;
        inputActions.Gameplay.Move.canceled += MoveInputUpdated;
        inputActions.Gameplay.Interact.performed += Interact;
    }

    void Interact(InputAction.CallbackContext ctx)
    {
        InteractComponent interactComp = GetComponentInChildren<InteractComponent>();
        if(interactComp!=null)
        {
            interactComp.Interact();
        }
    }

    void MoveInputUpdated(InputAction.CallbackContext ctx)
    {
         movementComp.SetMovementInput(ctx.ReadValue<Vector2>());
    }
}
