using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    [SerializeField] PlayerController player;
    private void Awake()
    {
        player = GetComponentInParent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        player.SetGroundedState(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == player.gameObject)
        {
            return;
        }

        player.SetGroundedState(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == player.gameObject)
        {
            return;
        }

        player.SetGroundedState(true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        player.SetGroundedState(true);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == player.gameObject)
        {
            return;
        }

        player.SetGroundedState(false);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject == player.gameObject)
        {
            return;
        }

        player.SetGroundedState(false);
    }
}
