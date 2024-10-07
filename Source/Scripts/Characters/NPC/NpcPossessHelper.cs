using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class taking care of possess mechanic by comunicating with player possess helper, 
// holds every needed information about object and changes them for references on possess health bar 

public class NpcPossessHelper : MonoBehaviour
{
    public Sprite[] Sprites;

    [SerializeField]
    private NpcLayers originLayer;

    private NpcHP hp;
    private NpcMovement movement;
    private NpcCombat abilities;
    private SpriteRenderer spriteRenderer;
    private PlayerPossessHelper helper;
    public bool possesable = true;

    private void Start()
    {
        hp = GetComponent<NpcHP>();
        movement = GetComponent<NpcMovement>();
        abilities = GetComponent<NpcCombat>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (originLayer == NpcLayers.Enemy)
            gameObject.GetComponent<EnemyFollow>().Speed = movement.GetSpeed; ;
    }

    public void SetPossess(PlayerPossessHelper helper)
    {
        if(possesable == false) {
            gameObject.tag = "Enemy";
            gameObject.layer = (int)NpcLayers.Enemy;
            gameObject.GetComponent<EnemyFollow>().Possessed = false;
            gameObject.GetComponent<EnemyCombat>().Possessed = false;
            return;
        }

        this.helper = helper;
        
        // On null reset state do default behaviour 
        if (helper == null) { 
            // Easy way to make sure we set previous layer and tag to possessable character
            if (originLayer == NpcLayers.Golem) {
                gameObject.tag = "Golem";
                gameObject.layer = (int)NpcLayers.Golem;
            }
            else if (originLayer == NpcLayers.Enemy) {
                gameObject.tag = "Enemy";
                gameObject.layer = (int)NpcLayers.Enemy;
                gameObject.GetComponent<EnemyFollow>().Possessed = false;
                gameObject.GetComponent<EnemyCombat>().Possessed = false;
            }
            else {
                gameObject.tag = "NPC";
                gameObject.layer = (int)NpcLayers.NPC;
            }

            spriteRenderer.sprite = Sprites[0];
            hp.SetPossess(null);
            abilities.SetPossess(false);
        }
        // If helper exists, establish every needed information for player possess helper
        else {
            gameObject.tag = "Player";
            gameObject.layer = (int)NpcLayers.Player;

            hp.SetPossess(helper.GetPossessHp());
            abilities.SetPossess(true);

            if (originLayer == NpcLayers.Enemy) {
                gameObject.GetComponent<EnemyFollow>().Possessed = true;
                gameObject.GetComponent<EnemyCombat>().Possessed = true;
            }

            spriteRenderer.sprite = Sprites[1];
            helper.SetMovement(movement.GetSpeed, movement.GetRb);
            helper.SetAbilities(abilities.GetAbilities, abilities.GetAbilityPoint);
        }
    }

    public void OnDeath(){
        helper?.gameObject.GetComponent<PlayerPossessSystem>().PossessDied();
    }
}
