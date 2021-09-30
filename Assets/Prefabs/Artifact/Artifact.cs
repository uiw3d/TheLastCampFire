using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artifact : Pickup
{
    [SerializeField] float DropDownSlotSearchRadius = 0.2f;
    AtifactSlot CurrentSlot = null;
    private void Start()
    {
        DropedDown();
    }
    public override void PickedUpBy(GameObject PickerGameObject)
    {
        base.PickedUpBy(PickerGameObject);
        if(CurrentSlot)
        {
            CurrentSlot.OnArtifactLeft();
            CurrentSlot = null;
        }
    }
    public override void DropedDown()
    {
        AtifactSlot slot = GetArtifactSlotNearBy();
        if(slot != null)
        {
            slot.OnArtifactPlaced();
            transform.parent = null;
            transform.rotation = slot.GetSlotTrans().rotation;
            transform.position = slot.GetSlotTrans().position;
            CurrentSlot = slot;
        }
        else
        {
            base.DropedDown();
        }
    }

    AtifactSlot GetArtifactSlotNearBy()
    {
        Collider[] Cols = Physics.OverlapSphere(transform.position, DropDownSlotSearchRadius);
        foreach(Collider col in Cols)
        {
            AtifactSlot slot = col.GetComponent<AtifactSlot>();
            if(slot!=null)
            {
                return slot;
            }
        }
        return null;
    }
}
