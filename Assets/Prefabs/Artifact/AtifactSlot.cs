using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtifactSlot : MonoBehaviour
{
    [SerializeField] Transform ArtifactSlotTrans;
    [SerializeField] Platform platformToMove;
    public void OnArtifactLeft()
    {
        Debug.Log("Artifact Left Me");
        platformToMove.MoveTo(platformToMove.StartTrans);
    }

    public void OnArtifactPlaced()
    {
        platformToMove.MoveTo(platformToMove.EndTrans);
        Debug.Log("Artifact Place on me");
    }

    public Transform GetSlotTrans()
    {
        return ArtifactSlotTrans;
    }
}
