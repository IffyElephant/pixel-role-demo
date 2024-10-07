using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

// Sript taking care of picking correct generator and setting up seed as well as generating start/end positions for level
// Also generates enemies and initializes pathfinding object with a finished map for NPC's

public class LevelGen : MonoBehaviour
{
    public AbstractGen[] AbstractGens;
    public GameObject Player, Portal, Ladder;
    public TilemapGen TilemapGen;
    public GameObject[] Enemies;
    public JumpPointSearch PathfindingAStar;

    int difficulty, seed, stage;
    
    private void Awake(){
        TilemapGen.ClearAll();

        AbstractGen activeGen = AbstractGens[PlayerPrefs.GetInt("Gen", 0)];
        int mapSize = PlayerPrefs.GetInt("Size", 0);
        difficulty = PlayerPrefs.GetInt("Diff", 0);
        seed = PlayerPrefs.GetInt("Seed", 0);
        stage = PlayerPrefs.GetInt("Stage", 0);

        activeGen.Setup(mapSize, seed, this);
        activeGen.Generate();
    }

    // Generate start pos and end pos on map
    public Vector2Int GenObjPositions(System.Random random, int mapSize, HashSet<Vector2Int> map){
        float minDistance = mapSize * 0.65f;
        Vector2Int startPos = new(0,0), endPos = new(0,0);

        while(Vector2Int.Distance(startPos, endPos) < minDistance){
            startPos = map.ElementAt(random.Next(map.Count));
            endPos = GenerateEndPos(random, map);

            // Check if map is playable
            List<Vector2Int> path = AStar.FindPath(startPos, endPos, map);
            if(path.Count == 0)
                startPos = endPos;
        }

        Player.transform.position = new Vector3(startPos.x + 0.5f, startPos.y + 0.5f, 0);
        Ladder.transform.position = new Vector3(startPos.x + 0.5f, startPos.y, 0);
        Portal.transform.position = new Vector3(endPos.x + 0.5f, endPos.y, 0);

        return startPos;
    }

    public void SetObjPositions(Vector2Int player, Vector2Int portal) {
        Player.transform.position = new Vector3(player.x + 0.5f, player.y + 0.5f, 0);
        Ladder.transform.position = new Vector3(player.x + 0.5f, player.y, 0);
        Portal.transform.position = new Vector3(portal.x + 0.5f, portal.y, 0);
    }

    private Vector2Int GenerateEndPos(System.Random random, HashSet<Vector2Int> map) {
        int neighbours;
        Vector2Int pos;
        do {
            neighbours = 0;
            pos = map.ElementAt(random.Next(map.Count));
            foreach (Vector2Int dir in Directions2D.EightDir) {
                if(map.Contains(pos + dir))
                    neighbours++;
            }
        } while (neighbours < 8);

        return pos;
    }

    public void GenTiles(HashSet<Vector2Int> map){
        PathfindingAStar.SetMap(map);
        TilemapGen.ClearAll();
        TilemapGen.GenerateTilemap(map);
        GenerateEnemies(map);
    }

    private void GenerateEnemies(HashSet<Vector2Int> map){
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies) {
            DestroyImmediate(enemy);
        }

        System.Random random = new(seed);
        Debug.Log(difficulty);
        Debug.Log(stage);
        int enemyCount = difficulty * 15 + stage * 5 + 25;

        HashSet<Vector2Int> enemyPos = new();
        Vector2Int playerPos = new( Mathf.RoundToInt(Player.transform.position.x), Mathf.RoundToInt(Player.transform.position.y));
        for (int i = 0; i < enemyCount; i++) {
            Vector2Int pos;
            do {
                pos = map.ElementAt(random.Next(map.Count));
            } while(Vector2Int.Distance(playerPos, pos) < 8f);

            if(!enemyPos.Contains(pos)) {
                enemyPos.Add(pos);
                int randomType = random.Next(0, Enemies.Count());
                Instantiate(Enemies[randomType], new Vector3(pos.x + 0.5f, pos.y + 0.25f, 0), transform.rotation);
            }
        }
    }
}
