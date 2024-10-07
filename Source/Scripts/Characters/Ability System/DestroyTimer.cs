using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Works as a simple destory script to limit lifetime of spells

public class DestroyTimer : MonoBehaviour
{
    [SerializeField]
    private float time;

    void Start()
    {
        Destroy(gameObject, time);
    }
}
