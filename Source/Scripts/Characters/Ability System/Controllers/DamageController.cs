using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Inspired and taken from https://github.com/FarDust/SpellSystem

public class DamageController : MonoBehaviour
{
    public GameObject Particles = null;
    public Damage spell;

    [SerializeField]
    private bool destroy = false;

    private Rigidbody2D rb;
    private LayerMask targetLayer;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.AddForce(transform.right * (float)(Mathf.Pow(spell.velocity, 2) * 0.5));

        switch (gameObject.layer)
        {
            case (int)SpellLayers.PlayerAbility:
                targetLayer = (1 << (int)NpcLayers.Enemy);
                gameObject.tag = "Player";
                break;
            case (int)SpellLayers.EnemyAbility:
                targetLayer = (1 << (int)NpcLayers.Player) |
                              (1 << (int)NpcLayers.Golem) |
                              (1 << (int)NpcLayers.NPC);
                gameObject.tag = "Enemy";
                break;
            default:
                gameObject.layer = (int)SpellLayers.Ability;
                targetLayer = (1 << (int)NpcLayers.Player) |
                              (1 << (int)NpcLayers.Golem) |
                              (1 << (int)NpcLayers.Enemy) |
                              (1 << (int)NpcLayers.NPC);
                gameObject.tag = "NPC";
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        spell.OnHit(other.gameObject);

        // Collateral Damage
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, spell.hitRadius, targetLayer);
        foreach (Collider2D target in targets)
        {
            if (target.gameObject != other.gameObject)
                spell.OnHit(target.gameObject);
        }

        if (destroy){
            if(Particles != null){
                Instantiate(Particles, transform.position, transform.rotation, null);
            }
            Destroy(gameObject);
        }
    }
}
