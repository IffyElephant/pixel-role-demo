using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

// Simple script to show what seed we used to generate map

public class PrintSeedUI : MonoBehaviour
{
    void Start() {
        TMP_Text text = GetComponent<TMP_Text>();
        text.SetText("Seed:" + PlayerPrefs.GetString("SeedString"));
    }
}