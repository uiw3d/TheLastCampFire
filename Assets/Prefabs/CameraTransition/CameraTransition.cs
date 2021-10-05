using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraTransition : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera DestinationCam;
    [SerializeField] float TransitionTime = 1.0f;
    CinemachineBrain cinemachenBrain;
    private void Start()
    {
        cinemachenBrain = Camera.main.GetComponent<CinemachineBrain>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Player>()!=null)
        {
            cinemachenBrain.m_DefaultBlend.m_Time = TransitionTime;
            DestinationCam.Priority = 11;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Player>() != null)
        {
            DestinationCam.Priority = 9;
        }
    }
}
