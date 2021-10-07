using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtifactSlot : MonoBehaviour
{
    [SerializeField] Transform ArtifactSlotTrans;
    [SerializeField] GameObject TogglingObject;
    public void OnArtifactLeft()
    {
        TogglingObject.GetComponent<Togglable>().ToggleOff();
    }

    public void OnArtifactPlaced()
    {
        TogglingObject.GetComponent<Togglable>().ToggleOn();
    }

    public Transform GetSlotTrans()
    {
        return ArtifactSlotTrans;
    }
}
