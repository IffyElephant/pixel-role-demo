using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class SimpleRandomWalk : AbstractGen
{
    public int CorridorIter, CorridorLen, RoomIter, RoomLen;
    public bool CorridorWalkBack = false, RandomiseRoomStartPos = false, MapBounds = false;

    public override void Generate()
    {
        // Init
        random = new(Seed);
        HashSet<Vector2Int> map = new();
        List<Vector2Int> potentialRoomCenters = new(),
                         deadEnds = new();

        // Generate corridors
        GenerateCorridors(map, potentialRoomCenters);

        // Generate rooms on potential points
        float percentRoomCoverage = MapSize / 200f;
        int roomCount = Mathf.RoundToInt(potentialRoomCenters.Count * percentRoomCoverage);
        for (int i = 0; i < roomCount; i++) {
            Vector2Int startPos = potentialRoomCenters[random.Next(0, potentialRoomCenters.Count)];
            potentialRoomCenters.Remove(startPos);
            GenerateRoom(map, startPos);

            if(potentialRoomCenters.Count == 0)
                break;
        }

        // Generate dead ends
        FindDeadEnds(map, potentialRoomCenters, deadEnds);
        for (int i = 0; i < deadEnds.Count; i++) {
            // Generate rooms at dead ends
            GenerateRoom(map, deadEnds[i]);
        }

        // Generate start pos and end pos on map
        LevelGen.GenObjPositions(random, MapSize, map);
        
        // Generate tiles
        LevelGen.GenTiles(map);
        
        // Cleanup
        // map.Clear(); // Cleanup Issue explained in CA
    }

    private void GenerateCorridors(HashSet<Vector2Int> map, List<Vector2Int> potentialRoomCenters)
    {
        Vector2Int curPos = Vector2Int.zero;
        potentialRoomCenters.Add(curPos);

        for (int i = 0; i < CorridorIter;)
        {
            HashSet<Vector2Int> corridor = new();
            Vector2Int dir = Directions2D.GetRandomCardinalDir(random);

            // Check if we can walk back, and if not and direction 
            //   would make us to walk back then ignore this
            //   iteration and rerun it
            if (CorridorWalkBack == false && corridor.Contains(curPos + dir)){
                dir = Directions2D.CardinalRightOf(dir);
            }
            
            for (int step = 0; step < CorridorLen; step++)
            {
                curPos += dir;
                if(MapBounds == true)
                    if(curPos.x < 0 || curPos.y < 0 || curPos.x > MapSize || curPos.y > MapSize){
                        dir = Directions2D.CardinalLeftOf(dir);
                        continue;
                    }
   
                corridor.Add(curPos);
            }

            // Last position on corridor is good place for a room
            //   therefore add it to potential room centers for later user
            potentialRoomCenters.Add(curPos);
            map.UnionWith(corridor);

            i++;
        }
    }

    private void GenerateRoom(HashSet<Vector2Int> map, Vector2Int startPos)
    {
        HashSet<Vector2Int> room = new();
        Vector2Int curPos = startPos;

        for (int i = 0; i < RoomIter; i++)
        {
            for (int step = 0; step < RoomLen; step++)
            {
                Vector2Int dir = Directions2D.GetRandomCardinalDir(random);

                curPos += dir;

                if(MapBounds == true)
                    if(curPos.x < 0 || curPos.y < 0 || curPos.x > MapSize || curPos.y > MapSize ) {
                        curPos -= dir;
                        dir = Directions2D.CardinalRightOf(dir);
                        curPos += dir;
                    }

                if (room.Contains(curPos) == false)
                    room.Add(curPos);
            }

            // Check for randomising room start pos
            //   false = we will generate more circular rooms
            //   true  = rooms will be spread around more but this often leads to thinner rooms
            if (RandomiseRoomStartPos)
                curPos = room.ElementAt(random.Next(0, room.Count));
            else
                curPos = startPos;
        }

        map.UnionWith(room);
    }

    public static void FindDeadEnds(HashSet<Vector2Int> map, List<Vector2Int> deadEnds)
    {
        foreach (var pos in map)
        {
            int neighboursCount = 0;

            foreach (var dir in Directions2D.EightDir)
            {
                Vector2Int checkPos = pos + dir;
                if (map.Contains(checkPos))
                    neighboursCount++;

                // Dead-ends can have only one neighbouring tile
                if (neighboursCount > 1)
                    break;
            }

            if (neighboursCount == 1)
                deadEnds.Add(pos);
        }
    }

    public static void FindDeadEnds(HashSet<Vector2Int> map, List<Vector2Int> potentialRoomCenters, List<Vector2Int> deadEnds)
    {
        foreach (Vector2Int pos in potentialRoomCenters) {
            int neighboursCount = 0;
            foreach (var dir in Directions2D.EightDir){
                Vector2Int checkPos = pos + dir;
                if(map.Contains(checkPos))
                    neighboursCount++;
                
                if (neighboursCount > 1)
                    break;
            }

            if (neighboursCount == 1)
                deadEnds.Add(pos);
        }
    }
}
