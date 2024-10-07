using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Destorys gameobject in a set time in ms

public class TimeDestory : MonoBehaviour
{
    public float Time;

    private void Start() {
        Destroy(gameObject, Time);
    }
}
