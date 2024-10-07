using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Inspired and taken from https://github.com/FarDust/SpellSystem

public class BoostController : MonoBehaviour
{
    public Boost spell;

    private LayerMask targetLayer;
    private Coroutine coroutine;
    private float time;

    void Start()
    {
        time = Time.time + spell.duration;
        spell.Apply(transform.position);
    }

    private void Update()
    {
        if (Time.time >= time)
        {
            Cancel();
        }
    }

    public void SetTargetLayer(LayerMask targetLayer)
    {
        this.targetLayer = targetLayer;
    }

    public void Cancel()
    {
        spell.Cancel(transform.position);
        Destroy(gameObject);
    }
}
