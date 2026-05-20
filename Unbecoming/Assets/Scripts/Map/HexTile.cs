using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Hex/Hex Tile")]
public class HexTile : Tile {
    public TerrainType terrainType;
    public bool isPassable;
    public bool isTransparent;
}

public enum TerrainType {
    Grass,
    Water,
    Wall
}