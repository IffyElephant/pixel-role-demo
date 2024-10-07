using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Help funciton to flag parent to end the stage

public class Boss : MonoBehaviour
{
    private void OnDestroy() {
        SendMessageUpwards("BoosKilled");
    }
}
