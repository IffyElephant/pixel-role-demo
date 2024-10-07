using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Dummy class for movement of NPC's

public class NpcMovement : MonoBehaviour
{
    [SerializeField]
    private float speed;
    public float GetSpeed { get { return speed; } }

    private Rigidbody2D rb;
    public Rigidbody2D GetRb { get { return rb; } }

    private float mutliplayer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void BoostSpeed(float mutliplayer)
    {
        this.mutliplayer = mutliplayer;
    }
}
