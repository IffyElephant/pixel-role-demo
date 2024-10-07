using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Inspired and taken from https://github.com/FarDust/SpellSystem

[CreateAssetMenu(fileName = "Melee", menuName = "Abilities/Damage/MeleeAttack", order = 1)]
public class MeleeAttack : Damage
{
    public override void Cast(Transform parent, Vector3 position, Vector3 direction, LayerMask spellLayer)
    {
        base.Cast(parent, position, direction, spellLayer);
    }
}
