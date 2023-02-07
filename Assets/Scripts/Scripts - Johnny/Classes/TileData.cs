using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileData
{
    public Vector3Int position;
    public int state;
    public int seed;

    public TileData(Vector3Int position, int state, int seed)
    {
        this.position = position;
        this.state = state;
        this.seed = seed;
    }
}