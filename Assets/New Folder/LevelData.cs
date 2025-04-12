using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TileData
{
    public string type;
    public Vector2 position;
}

[Serializable]
public class LevelData
{
    public List<TileData> tiles = new();
}
