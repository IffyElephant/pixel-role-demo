using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script takes care of generaml player movement and adjusts movement speed based on body its in, including chanigng rigibody

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float setSpeed;
    private float speed,
                  mutliplayer = 1;
    private Rigidbody2D playerRb, rb;
    private Vector2 movementAxis;
    private bool possessing = false;

    private const float diagonalFix = 0.70710678118f;

    private void Start()
    {
        playerRb = rb = GetComponent<Rigidbody2D>();
        speed = setSpeed;
    }

    private void Update()
    {
        movementAxis.x = Input.GetAxis("Horizontal");
        movementAxis.y = Input.GetAxis("Vertical");

        if (movementAxis.x != 0 && movementAxis.y != 0)
        {
            movementAxis.x = Mathf.Clamp(movementAxis.x, -diagonalFix, diagonalFix);
            movementAxis.y = Mathf.Clamp(movementAxis.y, -diagonalFix, diagonalFix);
        }

        if (possessing)
            transform.position = rb.position;
    }

    private void FixedUpdate()
    {
        rb.velocity = movementAxis * speed * mutliplayer * Time.fixedDeltaTime;
    }

    public void SetPossess(float speed, Rigidbody2D rb)
    {
        this.speed = speed;
        this.rb = rb;
        possessing = true;

        transform.position = rb.position;
    }

    public void SetPossess()
    {
        this.speed = setSpeed;
        this.rb = playerRb;
        possessing = false;
    }

    public void BoostSpeed(float mutliplayer)
    {
        Debug.Log("HERE" + mutliplayer);
        this.mutliplayer = mutliplayer;
    }
}
