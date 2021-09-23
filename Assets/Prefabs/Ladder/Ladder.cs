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
        Player otherAsPlayer = other.GetComponent<Player>();
        if(otherAsPlayer!=null)
        {
            otherAsPlayer.NotifyLadderNearby(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Player otherAsPlayer = other.GetComponent<Player>();
        if (otherAsPlayer != null)
        {
            otherAsPlayer.NotifyLadderExit(this);
        }
    }
}
