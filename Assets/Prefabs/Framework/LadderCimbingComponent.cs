using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LadderCimbingComponent : MonoBehaviour
{
    [SerializeField] float LadderClimbCommitAngleDegrees = 20f;
    [SerializeField] float LadderHopOnTime = 0.2f;

    Ladder CurrentClimbingLadder;
    List<Ladder> LaddersNearby = new List<Ladder>();
    MovementComponent movementComp;
    IInputActionCollection InputAction;
    public void SetInput(IInputActionCollection inputAction)
    {
        InputAction = inputAction;
    }

    public void NotifyLadderNearby(Ladder ladderNearby)
    {
        LaddersNearby.Add(ladderNearby);
    }

    public void NotifyLadderExit(Ladder ladderExit)
    {
        if (ladderExit == CurrentClimbingLadder)
        {
            CurrentClimbingLadder = null;
            movementComp.SetClimbingInfo(Vector3.zero, false);
            movementComp.ClearVerticalVelocity();
        }
        LaddersNearby.Remove(ladderExit);
    }

    Ladder FindPlayerClimbingLadder()
    {
        Vector3 PlayerDesiredMoveDir = movementComp.GetPlayerDesiredMoveDir();
        Ladder ChosenLadder = null;
        float CloestAngle = 180.0f;
        foreach (Ladder ladder in LaddersNearby)
        {
            Vector3 LadderDir = ladder.transform.position - transform.position;
            LadderDir.y = 0;
            LadderDir.Normalize();
            float Dot = Vector3.Dot(PlayerDesiredMoveDir, LadderDir);
            float AngleDegrees = Mathf.Acos(Dot) * Mathf.Rad2Deg;
            if (AngleDegrees < LadderClimbCommitAngleDegrees && AngleDegrees < CloestAngle)
            {
                ChosenLadder = ladder;
                CloestAngle = AngleDegrees;
            }
        }
        return ChosenLadder;
    }

    void HopOnLadder(Ladder ladderToHopOn)
    {
        if (ladderToHopOn == null) return;

        if (ladderToHopOn != CurrentClimbingLadder)
        {
            Transform snapToTransform = ladderToHopOn.GetClosestSnapTransform(transform.position);
            CurrentClimbingLadder = ladderToHopOn;
            movementComp.SetClimbingInfo(ladderToHopOn.transform.forward, true);
            DisableInput();
            StartCoroutine(movementComp.MoveToTransform(snapToTransform, LadderHopOnTime));
            Invoke("EnableInput", LadderHopOnTime);
        }
    }

    void EnableInput()
    {
        InputAction.Enable();
    }

    void DisableInput()
    {
        InputAction.Disable();
    }


    // Start is called before the first frame update
    void Start()
    {
        movementComp = GetComponent<MovementComponent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentClimbingLadder == null)
        {
            HopOnLadder(FindPlayerClimbingLadder());
        }

    }
}
