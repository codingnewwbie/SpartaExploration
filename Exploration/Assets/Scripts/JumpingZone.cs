using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingZone : MonoBehaviour
{
    public float jumpingPower = 4f;    
    
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if(playerController != null) playerController.HighJump(jumpingPower);
        }
    }
}
