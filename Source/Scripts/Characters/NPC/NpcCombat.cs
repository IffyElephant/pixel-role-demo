using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Dummy class to hold abilities

public class NpcCombat : MonoBehaviour
{
    [SerializeField]
    private List<AbilityBase> abilities;
    public List<AbilityBase> GetAbilities { get { return abilities; } }

    [SerializeField]
    private Transform abilityPoint;
    public Transform GetAbilityPoint { get { return abilityPoint; } }

    private bool possessed = false;

    public void SetPossess(bool status)
    {
        possessed = status;
    }
}
