using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Context steering behaviours for NPC's
// Gets distances to player and walls around object and calculates direction to player, while avoiding walls

public class ContextSteering
{
    private float[] avoidDir = new float[8];
    private float[] followDir = new float[8];

    public Vector2 DirToPlayer(Vector2 player, Vector2 pos) {
        Vector2 final = Vector2.zero;

        AvoidanceDir(player, pos);
        PlayerDir(player, pos);

        for (int i = 0; i < 8; i++) {
            float val = Mathf.Clamp01(followDir[i] + avoidDir[i]);
            final += Directions2D.FloatEightDir[i] * val;
        }

        Vector2 ret = final.normalized;
        return ret;
    }

    private void AvoidanceDir(Vector2 player, Vector2 pos) {
        Vector2 playerDir = (player - pos).normalized;

        for (int i = 0; i < 8; i++){
            Vector2 dir = Directions2D.FloatEightDir[i];
            float angle = Vector2.Angle(playerDir, dir);

            if(angle > 90f){ 
                avoidDir[i] = 0.5f;
            }
            else {
                RaycastHit2D hit = Physics2D.Raycast(pos + new Vector2(0f, 0.15f), dir, 1f, 1 << (int)ObjectLayers.Wall);
                if(hit.collider != null) {
                    avoidDir[i] = - (1f - Vector2.Distance(hit.point, pos));
                }
                else {
                    avoidDir[i] = 0f;
                }
            }
        }
    }

    private void PlayerDir(Vector2 player, Vector2 pos)
    {
        Vector2 playerDir = (player - pos).normalized;
        float totalAngle = 0;
        int count = 0;
        for (int i = 0; i < 8; i++) {
            Vector2 dir = Directions2D.FloatEightDir[i];
            float angle = Vector2.Angle(playerDir, dir);

            if(angle > 90f) {
                followDir[i] = -0.5f;
            }
            else {
                followDir[i] = 90f - angle;
                totalAngle += angle;
                count++;
            }
        }

        for (int i = 0; i < 8; i++) {
            if(followDir[i] >= 0) {
                followDir[i] = followDir[i] / totalAngle;
            }
        }
    }
}
