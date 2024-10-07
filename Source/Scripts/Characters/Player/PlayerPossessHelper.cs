using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player possess helper comunicates and changes ifnormation with object we want to possess

public class PlayerPossessHelper : MonoBehaviour
{
    public GameObject effectObj;
    public Collider2D[] colliders;
    private SpriteRenderer sprite;
    private PlayerPossessHp possessHp;
    private PlayerMovement playerMovement;
    private PlayerCombat playerCombat;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        possessHp = GetComponent<PlayerPossessHp>();
        playerMovement = GetComponent<PlayerMovement>();
        playerCombat = GetComponent<PlayerCombat>();
    }

    public void SetPossess(bool status)
    {
        sprite.enabled = !status;         // Enable / disable player sprite
        effectObj.SetActive(!status);

        foreach (var col in colliders)    // Enable / disable player colliders
            col.enabled = !status;

        if (!status) {                    // On false - reset player movement
            playerMovement.SetPossess();
            playerCombat.SetPossess();
            possessHp.SetPossess(false);
        }
    }

    // Contacting possess object changes based upon status value
    //   true  - tell possessing obj we are gettin control of him
    //   false - tell possessing obj we are letting it and it can return to default behaviour
    public bool ContactPossess(GameObject obj, bool status)
    {
        NpcPossessHelper helper = null;
        helper = obj.GetComponent<NpcPossessHelper>();

        if(helper?.possesable == true) {
            helper.SetPossess(status == true ? this : null);
            return true;
        }

        return false;
    }

    public void SetMovement(float speed, Rigidbody2D rb)
    {
        playerMovement.SetPossess(speed, rb);
    }

    public PlayerPossessHp GetPossessHp()
    {
        return possessHp;
    }

    public void SetAbilities(List<AbilityBase> abilities, Transform abilityPoint)
    {
        playerCombat.SetPossess(abilities, abilityPoint);
    }
}
