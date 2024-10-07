using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Inspired and taken from https://github.com/FarDust/SpellSystem

public class Aura : AbilityBase
{
    public readonly LayerMask targetLayers;
    public readonly float hitRadius,
                          castPeriod,
                          duration;
    public readonly GameObject spellPrefab;

    public override void Cast(Transform parent, Vector3 position, Vector3 direction, LayerMask spellLayer)
    {
        GameObject spell = Instantiate(spellPrefab, parent);
        spell.transform.localScale = hitRadius * spell.transform.localScale;
        base.Cast(parent, position, direction, spellLayer);
    }

    public virtual void Apply(Vector3 position) { }
}
