using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Helper script for patrolling enemies, to find new path or reverse it based on state

public class EnemyRandomWalk : MonoBehaviour
{
    private JumpPointSearch jps;
    private List<Vector2Int> path = new();
    private List<Vector2Int> currentPath = new();

    private void Start() {
        jps = GameObject.Find("_path_finder_jps").GetComponent<JumpPointSearch>();
    }

    public void NewPath() {
        Vector2Int randomPoint = jps.RandomPoint();
        Vector2Int pos = new(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
        path = jps.FindPath(pos, randomPoint);
    }

    public Vector2 GetPoint() {
        if(currentPath.Count == 0) {
            if(path.Count == 0) {
                NewPath();
            }

            currentPath = new List<Vector2Int>(path);
            path.Reverse();
        }

        if(currentPath.Count > 0) {
            Vector2 retVal = currentPath.First() + new Vector2(0.5f, 0.5f);
            currentPath.RemoveAt(0);
            return retVal;
        }

        return Vector2.zero;
    }

    public void ResetPath() {
        if(currentPath.Count != 0){
            currentPath = new();
            path = new();
        }
    }
}
