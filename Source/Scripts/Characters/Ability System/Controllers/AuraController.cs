using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Inspired and taken from https://github.com/FarDust/SpellSystem

public class AuraController : MonoBehaviour
{
    [SerializeField]
    private Aura spell;

    private Coroutine coroutine;
    private float time;

    void Start()
    {
        coroutine = StartCoroutine(Activate());
        time = Time.time;
    }

    private void Update()
    {
        if (Time.time - time > spell.duration) {
            Cancel();
        }
    }

    IEnumerator Activate()
    {
        while (true) {
            yield return new WaitForSeconds(spell.castPeriod);
            spell.Apply(transform.position);
        }
    }

    public void Cancel()
    {
        StopCoroutine(coroutine);
        Destroy(gameObject);
    }
}
