using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractComponent : MonoBehaviour
{
    List<Interactable> interactables = new List<Interactable>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Interactable otherAsInteractable = other.GetComponent<Interactable>();
        if(otherAsInteractable != null)
        {
            Debug.Log("Find Interacable");
            if (!interactables.Contains(otherAsInteractable))
            { 
                interactables.Add(otherAsInteractable);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Interactable otherAsInteractable = other.GetComponent<Interactable>();
        if (otherAsInteractable != null)
        {
            if (interactables.Contains(otherAsInteractable))
            {
                interactables.Remove(otherAsInteractable);
            }
        }
    }

    public void Interact()
    {
        Debug.Log("Interacting");
        Interactable closestInteracable = GetClosestInteractable();
        if(closestInteracable!=null)
        {
            closestInteracable.Interact();
        }
    }

    Interactable GetClosestInteractable()
    {
        Interactable cloesetInteractable = null;
        if(interactables.Count == 0)
        {
            return cloesetInteractable;
        }

        float ClosestDist = float.MaxValue;
        foreach(var itemInteractable in interactables)
        {
            float Dist = Vector3.Distance(transform.position, itemInteractable.transform.position);
            if(Dist < ClosestDist)
            {
                cloesetInteractable = itemInteractable;
                ClosestDist = Dist;
            }
        }

        return cloesetInteractable;
    }
}
