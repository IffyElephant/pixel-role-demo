using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedCombat : EnemyCombat
{
    private void Start()
    {
        npcCombat = GetComponent<NpcCombat>();
        enemyFollow = GetComponent<EnemyFollow>();

        abilities = npcCombat.GetAbilities;
        abilityPoint = npcCombat.GetAbilityPoint;

        cooldowns.Add(0f);

        possessed = false;
    }

    private void FixedUpdate()
    {
        if (possessed == false)
        {
            Transform player = enemyFollow.FindPlayer();
            if (player != null && Vector2.Distance(transform.position, player.position) <= distance)
            {
                if (cooldowns[0] <= 0)
                {
                    cooldowns[0] = abilities[0].GetCooldown;
                    Vector3 dir = player.position - transform.position;
                    dir = dir.normalized;
                    abilities[0].Cast(transform, abilityPoint.position + 0.2f * dir, dir, (int)SpellLayers.EnemyAbility);
                }
            }

            if (cooldowns[0] > 0)
            {
                cooldowns[0] -= Time.fixedDeltaTime;
            }
        }
    }
}
