using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    private Player player;
    private float footstepsTimer;
    private float footstepsTimerMax = 0.1f;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        footstepsTimer -= Time.deltaTime;

        if(footstepsTimer < 0f)
        {
            footstepsTimer = footstepsTimerMax;    

            if(player.IsWalking())
            {
            float volume = 1f;
            SoundManager.Instance.PlayFootstepSound(player.transform.position, volume);
            }
        }
    }

}
