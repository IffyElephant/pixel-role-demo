using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script for enemy movement utilizing context steering and pathfinding

public class EnemyFollow : MonoBehaviour
{
    [SerializeField]
    private float visionRange, distance;

    private Rigidbody2D rb;
    private ContextSteering steering = new();
    private EnemyRandomWalk pathWalk = null;
    private Vector3 lastPosition;
    private LayerMask mask;
    private float speed;
    public float Speed { set { speed = value; } get { return speed; } }
    private bool possessed;
    public bool Possessed { set { possessed = value; } get { return possessed; } }
    public bool Patrol = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if(Patrol)
            pathWalk = GetComponent<EnemyRandomWalk>();

        possessed = false;
        lastPosition = transform.position;
        mask = (1 << (int)NpcLayers.Player) |
               (1 << (int)ObjectLayers.Wall);
    }

    private void FixedUpdate()
    {
        if (possessed == false)
        {
            Transform player = FindPlayer();
            if (player != null) {
                rb.velocity = steering.DirToPlayer(player.position, transform.position) * speed * Time.fixedDeltaTime;
                lastPosition = player.position;
                pathWalk.ResetPath();
            }
            else if (lastPosition != transform.position) {
                rb.velocity = steering.DirToPlayer(lastPosition, transform.position) * speed * Time.fixedDeltaTime;
                if (Vector2.Distance(transform.position, lastPosition) < 0.5f) {
                    lastPosition = transform.position;
                    rb.velocity = Vector2.zero;
                }
            }
            else {
                if (Patrol) {
                    Vector2 pos = pathWalk.GetPoint();
                    lastPosition = new Vector3(pos.x, pos.y , 0);

                    if(lastPosition == Vector3.zero)
                        lastPosition = transform.position;
                }
            }
        }
    }

    private void MoveToPos(Vector3 position)
    {
        if (Vector2.Distance(position, transform.position) > distance)
        {
            Vector3 dir = position - transform.position;
            dir.Normalize();
            rb.velocity = speed * Time.fixedDeltaTime * dir;
        }
    }

    public Transform FindPlayer()
    {
        Transform player = null;
        RaycastHit2D ray = new();
        RaycastHit2D circleRay = Physics2D.CircleCast(transform.position, visionRange, Vector2.right, visionRange, 1 << (int)NpcLayers.Player);

        if (circleRay.collider != null)
        {
            Vector2 dir = circleRay.transform.position - transform.position;
            dir.Normalize();
            ray = Physics2D.Raycast(transform.position, dir, Mathf.Infinity, mask);
            if (ray.collider != null && ray.collider.CompareTag("Player"))
            {
                player = ray.collider.transform;
            }
        }

        return player;
    }

    public void SetPos(Vector3 pos) {
        lastPosition = pos;
    }   
}
