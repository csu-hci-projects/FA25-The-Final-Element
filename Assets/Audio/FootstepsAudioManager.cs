using System;
using FMODUnity;
using UnityEngine;

public class FootstepsAudioManager : MonoBehaviour
{
    [SerializeField] EventReference FootstepEvent;
    [SerializeField] float rate;
    [SerializeField] GameObject player;
    [SerializeField] FirstPersonController controller;

    float time;

    public void PlayFootstep()
    {
        RuntimeManager.PlayOneShotAttached(FootstepEvent, player);
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (controller.isWalking)
        {
            if (time >= rate)
            {
                PlayFootstep();
                time = 0;
            }
        }

        if (controller.isSprinting)
        {
            if (time >= 0.25)
            {
                PlayFootstep();
                time = 0;
            }
        }
    }
}
