using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Dummy class not used in final game
// Takes care of pciking up coins

public class Currency : MonoBehaviour
{
    [SerializeField]
    private int value;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.SendMessage("PickUpAmount", value, SendMessageOptions.DontRequireReceiver);
            Destroy(gameObject);
        }
    }
}
