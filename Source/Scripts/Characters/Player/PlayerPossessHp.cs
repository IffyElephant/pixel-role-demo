using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// UI handler for hp of a possess NPC

public class PlayerPossessHp : MonoBehaviour
{
    public RectTransform possessHpBar;
    private float maxHp;
    private bool possessing = false;

    private void Start()
    {
        possessHpBar.gameObject.SetActive(possessing);
    }

    public void UpdateBar(float hp)
    {
        float val = hp / maxHp;

        if (val < 0)
            val = 0;
        else if (val > 1)
            val = 1;

        possessHpBar.localScale = new Vector3(val, possessHpBar.localScale.y, possessHpBar.localScale.z);
    }

    public void SetPossess(bool status)
    {
        possessing = status;
        possessHpBar.gameObject.SetActive(possessing);
    }

    public void SetPossess(float maxHp, float hp, float regeneration)
    {
        this.maxHp = maxHp;
        possessing = true;

        possessHpBar.gameObject.SetActive(possessing);
        UpdateBar(hp);
    }
}
