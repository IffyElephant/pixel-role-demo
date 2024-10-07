using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Inspired and taken from https://github.com/FarDust/SpellSystem

public class Damage : AbilityBase
{
    public int damage;
    public float velocity,
                 hitRadius;
    public GameObject spellPrefab;

    public override void Cast(Transform parent, Vector3 position, Vector3 direction, LayerMask spellLayer)
    {
        float zRotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0f, 0f, zRotation);

        GameObject spell = Instantiate(spellPrefab, position, rotation);
        spell.gameObject.layer = spellLayer;

        base.Cast(parent, position, direction, spellLayer);
    }

    public virtual void OnHit(GameObject target)
    {
        target.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
    }
}
