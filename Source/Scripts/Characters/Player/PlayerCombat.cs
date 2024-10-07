using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Player combat with all needed icons, holds references to UI, holds options for inputs to cast spells

public class PlayerCombat : MonoBehaviour
{
    public List<Image> possessIcons = new(),
                       playerIcons = new();

    public List<Image> cooldownOverlay = new();

    [SerializeField]
    private List<KeyCode> keyBindings = new();

    [SerializeField]
    private List<AbilityBase> abilities = new();

    private List<float> cooldowns = new();
    private PlayerHp pHp;

    [SerializeField]
    private Transform setAbilityPoint;
    private Transform abilityPoint;

    private void Start() {
        pHp = GetComponent<PlayerHp>();

        for (int i = 0; i < 3; i++) {
            cooldowns.Add(0);
        }

        if (abilities.Count > 0 ){
            for (int i = 0; i < possessIcons.Count; i++) {
                if (abilities.Count > i) {
                    possessIcons[i].enabled = true;
                    possessIcons[i].sprite = abilities[i].Icon;
                }
                else
                    possessIcons[i].enabled = false;
            }
        }
    }

    private void Update() {
        for (int i = 0; i < abilities.Count; i++) {
            if (Input.GetKey(keyBindings[i]) && abilities[i] != null) {
                if (cooldowns[i] <= 0 || abilities[i].GetManaCost) {
                    if (cooldowns[i] > 0 && abilities[i].GetManaCost)
                        pHp.PayManaCost();

                    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector3 dir = mousePos - abilityPoint.position;
                    dir = dir.normalized;

                    cooldowns[i] = abilities[i].GetCooldown;
                    cooldownOverlay[i].gameObject.SetActive(true);

                    abilities[i].Cast(transform, abilityPoint.position + dir, dir, (int)SpellLayers.PlayerAbility);
                }
            }

            if (cooldowns[i] > 0f) {
                cooldowns[i] -= Time.fixedDeltaTime;
                cooldownOverlay[i].fillAmount = cooldowns[i] / abilities[i].GetCooldown;

                if (cooldowns[i] <= 0f)
                    cooldownOverlay[i].gameObject.SetActive(false);
            }
        }
    }

    public void SetPossess(List<AbilityBase> abilities, Transform abilityPoint) {
        this.abilities = abilities;
        this.abilityPoint = abilityPoint;

        for (int i = 0; i < possessIcons.Count; i++) {
            if (abilities.Count > i) {
                possessIcons[i].enabled = true;
                possessIcons[i].sprite = abilities[i].Icon;
            }
            else
                possessIcons[i].enabled = false;
        }
    }

    public void SetPossess() {
        abilities = new();
        abilityPoint = setAbilityPoint;

        for (int i = 0; i < possessIcons.Count; i++)
            possessIcons[i].enabled = false;
    }
}
