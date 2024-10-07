using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is not really used its just dummy class for further work on upgrades

public class PlayerMoney : MonoBehaviour
{
    private int currency;

    private void Start()
    {
        currency = 0;
    }

    public int GetCurency()
    {
        return currency;
    }

    public void PickUpAmount(int amount)
    {
        currency += amount;
    }

    public bool PayAmount(int amount)
    {
        if (currency - amount < 0)
            return false;

        currency -= amount;
        return true;
    }
}
