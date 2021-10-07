using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    [SerializeField] Transform TopSnapTransform;
    [SerializeField] Transform BottomSnapTransform;
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
        LadderCimbingComponent otherClimbingComp = other.GetComponent<LadderCimbingComponent>();
        if(otherClimbingComp != null)
        {
            otherClimbingComp.NotifyLadderNearby(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        LadderCimbingComponent otherClimbingComp = other.GetComponent<LadderCimbingComponent>();
        if (otherClimbingComp != null)
        {
            otherClimbingComp.NotifyLadderExit(this);
        }
    }

    public Transform GetClosestSnapTransform(Vector3 Postion)
    {
        float DistanceToTop = Vector3.Distance(Postion, TopSnapTransform.position);
        float DistanceToBot = Vector3.Distance(Postion, BottomSnapTransform.position);
        return DistanceToTop < DistanceToBot ? TopSnapTransform : BottomSnapTransform;
    }
}
