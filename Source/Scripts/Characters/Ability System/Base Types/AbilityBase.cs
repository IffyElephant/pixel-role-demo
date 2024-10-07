using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Inspired and taken from https://github.com/FarDust/SpellSystem

public class AbilityBase : ScriptableObject
{
    public Sprite Icon;

    [SerializeField]
    private float cooldown;
    public float GetCooldown { get { return cooldown; } }

    [SerializeField]
    private bool manaCost;
    public bool GetManaCost { get { return manaCost; } }

    public virtual void Cast(Transform parent, Vector3 position, Vector3 direction, LayerMask spellLayer) { }

    public bool CheckCooldown(float lastTimeCasted)
    {
        return ((Time.time - lastTimeCasted) > cooldown);
    }
}
