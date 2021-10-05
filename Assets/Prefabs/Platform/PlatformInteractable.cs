using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformInteractable : Interactable
{
    public override void Interact(GameObject InteractingObject = null)
    {
        GetComponentInChildren<Platform>().MoveTo(true);
    }
}
