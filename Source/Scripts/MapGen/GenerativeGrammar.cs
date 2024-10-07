using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GenerativeGrammar : AbstractGen
{
    public int minCorLen, maxCorLen;
    public Vector2Int minRoomSize, maxRoomSize;

    public override void Generate()
    {
        // Init
        random = new(Seed);
        HashSet<Vector2Int> map = new();
        Vector2Int startPos = new(0, 0);
        Vector2Int endPos = new();

        // Generate layout
        string set = GenerateLayout();

        // Generate layout
        endPos = GenerateMap(map, set);

        // Set Start and finish points
        LevelGen.SetObjPositions(startPos, endPos);
        
        // Generate tiles
        LevelGen.GenTiles(map);
        
        // Cleanup
        // map.Clear(); // Cleanup Issue explained in CA
    }

    private string GenerateLayout() {
        string set = "Scbr";
        int mapSize = MapSize / 15;
        for (int i = 0; i < mapSize; i++) {
            set += "cbc";
        }
        set += "lcE";

        string newSet = "";
        bool loop = true;
        
        while (loop == true) {
            loop = false;
            int len = set.Length;
            for (int i = 0; i < len; i++) {
                if(char.IsLower(set[i]) == true) {
                    loop = true;
                    newSet += char.ToUpper(set[i]);
                    int next = random.Next(0, 100);
                    if(next < 18) {
                        newSet += 'R';
                    } else if (next < 36) {
                        newSet += 'L';
                    } else if (next < 45) {
                        newSet += 'C';
                    } else if (next < 80) {
                        newSet += 'B';
                    } else if (next < 90) {
                        newSet += 'b';
                    } else {
                        int rand = random.Next(0, 3);
                        if (rand == 0) 
                            newSet += 'c';
                        else if (rand == 1) 
                            newSet += 'l';
                        else 
                            newSet += 'r';
                    }
                } else {
                    newSet += set[i];
                }
            }
            set = newSet;
            newSet = "";
        }

        return set;
    }

    private Vector2Int GenerateMap(HashSet<Vector2Int> map, string set) {
        int len = set.Length;
        Vector2Int curPos = Vector2Int.zero;
        Vector2Int dir = Vector2Int.right;

        for (int i = 0; i < len; i++) {
            if (set[i] == 'S' || 
                set[i] == 'B' ||
                set[i] == 'E') {
                GenerateRoom(map, curPos);
            } else {
                switch (set[i]) {
                    case 'R':
                        dir = Directions2D.CardinalRightOf(dir);
                        break;
                    case 'L':
                        dir = Directions2D.CardinalLeftOf(dir);
                        break;
                    case 'C':
                    default:
                        break;
                }
                curPos = GenerateCorridor(map, curPos, dir);
            }
        }

        return curPos;
    }

    private void GenerateRoom(HashSet<Vector2Int> map, Vector2Int pos)
    {
        HashSet<Vector2Int> room = new();
        Vector2Int roomSize = new(random.Next(minRoomSize.x, maxRoomSize.x), random.Next(minRoomSize.y, maxRoomSize.y));
        Vector2Int startPoint = pos - (roomSize / 2);

        for (int x = 0; x < roomSize.x; x++) {
            for (int y = 0; y < roomSize.y; y++) {
                room.Add(startPoint + new Vector2Int(x, y));
            }
        }

        map.UnionWith(room);
    }

    private Vector2Int GenerateCorridor(HashSet<Vector2Int> map, Vector2Int pos, Vector2Int dir) {
        HashSet<Vector2Int> corridor = new() {pos};
        Vector2Int curPos = pos;
        int len = random.Next(minCorLen, maxCorLen);

        for (int i = 0; i < len; i++) {   
            curPos += dir;
            corridor.Add(curPos);
        }

        map.UnionWith(corridor);
        return curPos;
    }
}
