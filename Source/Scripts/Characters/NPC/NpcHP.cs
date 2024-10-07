using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Dummy class with basic helth system for NPC's

public class NpcHP : MonoBehaviour
{
    [SerializeField]
    private float maxHp, regeneration;
    private float hp;
    private PlayerPossessHp possessHp; // also functions as a checker value if character is possessed
    private NpcPossessHelper npcPossessHelper; 

    private void Start()
    {
        npcPossessHelper = GetComponent<NpcPossessHelper>();
        possessHp = null;
        hp = maxHp;

        if(regeneration > 0)
            InvokeRepeating("Regen", 0, 1);
    }

    public void TakeDamage(float amount)
    {
        hp -= amount;
        if (possessHp != null)
            possessHp.UpdateBar(hp);
            
        if (hp <= 0){
            npcPossessHelper.OnDeath();
            Destroy(gameObject);
        }
    }

    public void SetPossess(PlayerPossessHp possessHp)
    {
        this.possessHp = possessHp;
        if (possessHp != null)
            possessHp.SetPossess(maxHp, hp, regeneration);
    }

    private void Regen() {
        if (possessHp == null)
            return;

        hp += regeneration;
            
        if (hp >= maxHp)
            hp = maxHp;

        possessHp.UpdateBar(hp);
    }
}
