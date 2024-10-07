using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

// Dummy class holding basic structure of every generator

public abstract class AbstractGen : MonoBehaviour
{
    public int MapSize = 50;
    public int Seed = 0;
    public LevelGen LevelGen;

    protected static System.Random random;

    public void Setup(int mapSize, int seed, LevelGen levelGen){
        MapSize = mapSize;
        Seed = seed;
        LevelGen = levelGen;
    }

    public abstract void Generate();
}
