using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// Takes care of fidning and generating graphics of walls 

public class TilemapGen : MonoBehaviour
{
    public Tilemap Tilemap;
    public TileBase[] Floor;
    public TileBase[] Walls;

    public void ClearAll(){
        Tilemap.ClearAllTiles();
    }

    public void GenerateTilemap(HashSet<Vector2Int> map){
        // Basic floor
        foreach(var pos in map){
            if(UnityEngine.Random.Range(0, 9) < 4){
                PaintTile(Floor[0], pos);
            }else{
                PaintTile(Floor[UnityEngine.Random.Range(1, Floor.Length)], pos);
            }
        }

        // Basic Walls
        HashSet<Vector2Int> walls = FindWalls(map, Directions2D.CardinalDir);
        GenerateBasicWalls(map, walls);

        // Complex Walls
        walls.Clear();
        walls = FindWalls(map, Directions2D.EightDir);
        GenerateComplexWalls(map, walls);
    }

    private void PaintTile(TileBase tile, Vector2Int pos){
        Vector3Int worldPos = Tilemap.WorldToCell((Vector3Int)pos);
        Tilemap.SetTile(worldPos, tile);
    }

    public HashSet<Vector2Int> FindWalls(HashSet<Vector2Int> map, List<Vector2Int> direction){
        HashSet<Vector2Int> walls = new();

        foreach (var pos in map) {
            foreach (var dir in direction) {
                Vector2Int nextPos = pos + dir;
                if (map.Contains(nextPos) == false)
                    walls.Add(nextPos);
            }
        }

        return walls;
    }

    public void GenerateBasicWalls(HashSet<Vector2Int> map, HashSet<Vector2Int> walls){
        foreach (Vector2Int pos in walls)
        {
            string binaryValue = "";

            foreach (var dir in Directions2D.CardinalDir) {
                var nextPos = pos + dir;
                if (map.Contains(nextPos))
                    binaryValue += "1";
                else
                    binaryValue += "0";
            }

            PaintBasicWalls(pos, binaryValue);
        }
    }

    public void GenerateComplexWalls(HashSet<Vector2Int> map, HashSet<Vector2Int> walls)
    {
        foreach (Vector2Int pos in walls) {
            string binaryValue = "";

            foreach (var dir in Directions2D.EightDir) {
                var nextPos = pos + dir;
                if (map.Contains(nextPos))
                    binaryValue += "1";
                else
                    binaryValue += "0";
            }

            PaintComplexWall(pos, binaryValue);
        }
    }

    public void PaintBasicWalls(Vector2Int wallPos, string binaryValue){
        int typeAsInt = Convert.ToInt32(binaryValue, 2);
        TileBase tile = null;

        if (WallBitValues.TopWall.Contains(typeAsInt))
            tile = Walls[0];
        else if (WallBitValues.BottomWall.Contains(typeAsInt))
            tile = Walls[1];
        else if (WallBitValues.RightWall.Contains(typeAsInt))
            tile = Walls[2];
        else if (WallBitValues.LeftWall.Contains(typeAsInt))
            tile = Walls[3];

        PaintTile(tile, wallPos);
    }

    public void PaintComplexWall(Vector2Int wallPos, string binaryValue)
    {
        int typeAsInt = Convert.ToInt32(binaryValue, 2);
        TileBase tile = null;

        if (WallBitValues.RightWall8Dir.Contains(typeAsInt))
            tile = Walls[3];
        else if (WallBitValues.LeftWall8Dir.Contains(typeAsInt))
            tile = Walls[2];
        else if (WallBitValues.RightTopCorner.Contains(typeAsInt))
            tile = Walls[2];
        else if (WallBitValues.LeftTopCorner.Contains(typeAsInt))
            tile = Walls[3];
        else if (WallBitValues.RightBottomCorner.Contains(typeAsInt))
            tile = Walls[4];
        else if (WallBitValues.LeftBottomCorner.Contains(typeAsInt))
            tile = Walls[5];
        else if (WallBitValues.LCornerRight.Contains(typeAsInt))
            tile = Walls[6];
        else if (WallBitValues.LCornerLeft.Contains(typeAsInt))
            tile = Walls[7];
        else if (WallBitValues.SinglePeak.Contains(typeAsInt))
            tile = Walls[8];
        else if (WallBitValues.SinglePeakConnector.Contains(typeAsInt))
            tile = Walls[9];

        if (tile != null)
            PaintTile(tile, wallPos);
    }
}
