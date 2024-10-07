using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Simple Enemy combat script holding imporatnt script
// Mostly taking care of possessed state so we can easily turn off enemy behaviours

public class EnemyCombat : MonoBehaviour
{
    [SerializeField]
    protected float distance;

    protected NpcCombat npcCombat;
    protected EnemyFollow enemyFollow;

    protected List<AbilityBase> abilities = new();
    protected List<float> cooldowns = new();

    protected Transform abilityPoint;

    protected bool possessed;
    public bool Possessed { set { possessed = value; } get { return possessed; } }
}
