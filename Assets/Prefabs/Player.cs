using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Player : MonoBehaviour
{
    [SerializeField] float WalkingSpeed = 5f;
    [SerializeField] Transform GroundCheck;
    [SerializeField] float GroundCheckRadius = 0.1f;
    [SerializeField] float rotationSpeed = 5f;
    [SerializeField] float EdgeCheckTracingDistance = 0.8f;
    [SerializeField] float EdgeCheckTracingDepth= 1f;
    [SerializeField] float LadderClimbCommitAngleDegrees = 20f;
    [SerializeField] LayerMask GroundLayerMask;
    [SerializeField] Transform PickuipSocketTransform;
    InputActions inputActions;
    Vector2 MoveInput;
    Vector3 Velocity;
    float Gravity = -9.8f;
    CharacterController characterController;
    Ladder CurrentClimbingLadder;
    List<Ladder> LaddersNearby = new List<Ladder>();
    public Transform GetPickupSocketTransform()
    {
        return PickuipSocketTransform;
    }
    public void NotifyLadderNearby(Ladder ladderNearby)
    {
        LaddersNearby.Add(ladderNearby);
    }

    public void NotifyLadderExit(Ladder ladderExit)
    {
        if(ladderExit == CurrentClimbingLadder)
        {
            CurrentClimbingLadder = null;
            Velocity.y = 0;
        }
        LaddersNearby.Remove(ladderExit);
    }

    Ladder FindPlayerClimbingLadder()
    {
        Vector3 PlayerDesiredMoveDir = GetPlayerDesiredMoveDir();
        Ladder ChosenLadder = null;
        float CloestAngle = 180.0f;
        foreach(Ladder ladder in LaddersNearby)
        {
            Vector3 LadderDir = ladder.transform.position - transform.position;
            LadderDir.y = 0;
            LadderDir.Normalize();
            float Dot = Vector3.Dot(PlayerDesiredMoveDir, LadderDir);
            float AngleDegrees = Mathf.Acos(Dot) * Mathf.Rad2Deg;
            if(AngleDegrees < LadderClimbCommitAngleDegrees && AngleDegrees < CloestAngle)
            {
                ChosenLadder = ladder;
                CloestAngle = AngleDegrees;
            }
        }
        return ChosenLadder;
    }

    bool IsOnGround()
    {
        return Physics.CheckSphere(GroundCheck.position, GroundCheckRadius, GroundLayerMask);
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
        characterController = GetComponent<CharacterController>();
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
        MoveInput = ctx.ReadValue<Vector2>();
    }

    void HopOnLadder(Ladder ladderToHopOn)
    {
        if (ladderToHopOn == null) return;

        if(ladderToHopOn != CurrentClimbingLadder)
        {
            Transform snapToTransform = ladderToHopOn.GetClosestSnapTransform(transform.position);
            CurrentClimbingLadder = ladderToHopOn;
            StartCoroutine(MoveToTransform(snapToTransform, 0.2f));
            Debug.Log("Hopped on ladder");
        }
    }
    IEnumerator MoveToTransform(Transform Destination, float transformTime)
    {
        inputActions.Gameplay.Move.Disable();

        Vector3 StartPos = transform.position;
        Vector3 EndPos = Destination.position;
        Quaternion StartRot = transform.rotation;
        Quaternion EndRot = Destination.rotation;

        float timmer = 0f;
        while(timmer < transformTime)
        {
            timmer += Time.deltaTime;
            Vector3 DeltaMove = Vector3.Lerp(StartPos, EndPos, timmer/transformTime) - transform.position;
            characterController.Move(DeltaMove);
            transform.rotation = Quaternion.Lerp(StartRot,EndRot,timmer/transformTime);
            yield return new WaitForEndOfFrame();
        }

        inputActions.Gameplay.Move.Enable();
    }
   

    // Update is called once per frame
    void Update()
    {
        if (CurrentClimbingLadder == null)
        {
            HopOnLadder(FindPlayerClimbingLadder());
        }
        if (CurrentClimbingLadder)
        {
            CalculateClimbingVelocity();
        }
        else
        { 
            CaculateWalkingVelocity();
        }

        characterController.Move(Velocity * Time.deltaTime);
        UpdateRotation();
    }

    void CalculateClimbingVelocity()
    {
        if(MoveInput.magnitude == 0)
        {
            Velocity = Vector3.zero;
            return;
        }

        Vector3 LadderDir = CurrentClimbingLadder.transform.forward;
        Vector3 PlayerDesiredMoveDir = GetPlayerDesiredMoveDir();

        float Dot = Vector3.Dot(LadderDir, PlayerDesiredMoveDir);

        Velocity = Vector3.zero;
        if(Dot < 0)
        {
            Velocity = GetPlayerDesiredMoveDir() * WalkingSpeed;
            Velocity.y = WalkingSpeed;
        }else
        {
            if(IsOnGround())
            {
                Velocity = GetPlayerDesiredMoveDir() * WalkingSpeed;
            }
            Velocity.y = -WalkingSpeed;
        }
    }

    private void CaculateWalkingVelocity()
    {
        if (IsOnGround())
        {
            Velocity.y = -0.2f;
        }

        Velocity.x = GetPlayerDesiredMoveDir().x * WalkingSpeed;
        Velocity.z = GetPlayerDesiredMoveDir().z * WalkingSpeed;
        Velocity.y += Gravity * Time.deltaTime;


        Vector3 PosXTracePos = transform.position + new Vector3(EdgeCheckTracingDistance, 0.5f, 0f);
        Vector3 NegXTracePos = transform.position + new Vector3(-EdgeCheckTracingDistance, 0.5f, 0f);
        Vector3 PosZTracePos = transform.position + new Vector3(0f, 0.5f, EdgeCheckTracingDistance);
        Vector3 NegZTracePos = transform.position + new Vector3(0f, 0.5f, -EdgeCheckTracingDistance);

        bool CanGoPosX = Physics.Raycast(PosXTracePos, Vector3.down, EdgeCheckTracingDepth, GroundLayerMask);
        bool CanGoNegX = Physics.Raycast(NegXTracePos, Vector3.down, EdgeCheckTracingDepth, GroundLayerMask);
        bool CanGoPosZ = Physics.Raycast(PosZTracePos, Vector3.down, EdgeCheckTracingDepth, GroundLayerMask);
        bool CanGoNegZ = Physics.Raycast(NegZTracePos, Vector3.down, EdgeCheckTracingDepth, GroundLayerMask);

        float xMin = CanGoNegX ? float.MinValue : 0f;
        float xMax = CanGoPosX ? float.MaxValue : 0f;
        float zMin = CanGoNegZ ? float.MinValue : 0f;
        float zMax = CanGoPosZ ? float.MaxValue : 0f;

        Velocity.x = Mathf.Clamp(Velocity.x, xMin, xMax);
        Velocity.z = Mathf.Clamp(Velocity.z, zMin, zMax);
    }

    Vector3 GetPlayerDesiredMoveDir()
    {
        return new Vector3(-MoveInput.y, 0f, MoveInput.x).normalized;
    }

    void UpdateRotation()
    {
        if (CurrentClimbingLadder != null)
        {
            return;
        }
        Vector3 PlayerDesiredDir = GetPlayerDesiredMoveDir();
        if(PlayerDesiredDir.magnitude == 0)
        {
            PlayerDesiredDir = transform.forward;
        }
        
        Quaternion DesiredRotation = Quaternion.LookRotation(PlayerDesiredDir, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, DesiredRotation, Time.deltaTime * rotationSpeed);
    }
}
