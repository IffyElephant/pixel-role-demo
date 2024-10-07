using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player Hp manager that takes care of player hp, graphic blink on damage and UI

public class PlayerHp : MonoBehaviour
{
    public Material[] Materials;
    public GameObject PlayerHit;
    public RectTransform hpBar;
    public GameObject DeathMenu;

    [SerializeField]
    private float maxHp, regeneration;
    private float hp, paymentLevel = 2;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        hp = maxHp;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void UpdateBar()
    {
        float val = hp / maxHp;

        if (val < 0)
        {
            Time.timeScale = 0;
            val = 0;
            DeathMenu.SetActive(true);
        }
        else if (val > 1)
            val = 1;

        hpBar.localScale = new Vector3(val, hpBar.localScale.y, hpBar.localScale.z);
    }

    public void TakeDamage(int amount)
    {
        hp -= amount;
        UpdateBar();
        StartCoroutine(Blink());
    }

    // This is nice thing to have but using it at this stage would feel unbalanced and generally not fun since it can kill player even if he missess cooldown by 0.1ms
    public void PayManaCost() 
    {
        // float amount = maxHp / paymentLevel;
        // hp -= amount;
        UpdateBar();
    }

    private IEnumerator Blink(){
        Instantiate(PlayerHit, transform);
        spriteRenderer.material = Materials[1];
        yield return new WaitForSecondsRealtime(0.33f);
        spriteRenderer.material = Materials[0];
    }
}
