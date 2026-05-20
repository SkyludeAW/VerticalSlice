using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour {
    [SerializeField] private Tilemap tilemap;

    public GameObject HexPrefab;
    // public Entity ObstaclePrefab;

    public Dictionary<Vector3Int, Hex> Hexes
    = new Dictionary<Vector3Int, Hex>();

    private void Start() {
        GenerateMap();
    }

    /*
    public int[][] ObstacleCoordinates = new int[][] {
        new int[] { 2, 3 },
        new int[] { 2, 4 },
        new int[] { 3, 4 },
        new int[] { 3, 5 },
        new int[] { 4, 0 },
        new int[] { 4, 1 },
        new int[] { 4, 2 },
        new int[] { 4, 2 },
        new int[] { 4, 5 },
        new int[] { 5, 4 },
        new int[] { 6, 3 },
        new int[] { 6, 7 }
    };
    */

    private void GenerateMap() {
        /*
        for (int i = 0; i < 10; i++) {
            for (int j = 0; j < 10; j++) {
                Hex hex = new Hex(i, j);
                Hexes[(hex.Q, hex.R, hex.S)] = hex;

                if (System.Array.Exists(ObstacleCoordinates, coord => coord[0] == i && coord[1] == j)) {
                    // Add an obstacle entity to this hex
                    hex.OccupiedEntity = Instantiate(ObstaclePrefab, hex.ToWorldPosition(), Quaternion.identity, this.transform);
                }

                Instantiate(HexPrefab, hex.ToWorldPosition(), Quaternion.identity, this.transform);
            }
        }
        */

        Hexes.Clear();

        BoundsInt bounds = tilemap.cellBounds;

        foreach (Vector3Int pos in bounds.allPositionsWithin) {
            TileBase tileBase = tilemap.GetTile(pos);

            if (tileBase == null)
                continue;

            HexTile hexTile = tileBase as HexTile;

            if (hexTile == null)
                continue;

            Hex hex = new Hex(OffsetToCube(pos));
            hex.IsPassable = hexTile.isPassable;
            hex.IsTransparent = hexTile.isTransparent;
            // TO-DO add terrain types to hex based on hexTile.terrainType
            Hexes.Add(hex.Position, hex);

            if (hex.IsPassable)
                Instantiate(HexPrefab, hex.ToWorldPosition() + new Vector3(0, 0.1f, 0), Quaternion.identity, this.transform);
        }

        Vector3Int OffsetToCube(Vector3Int offset) {
            int q = offset.x - (offset.y - (offset.y & 1)) / 2;
            int r = offset.y;
            int s = -q - r;
             
            return new Vector3Int(q, r, s);
        }
    }

    public Hex GetHex(int q, int r, int s) {
        Hexes.TryGetValue(new Vector3Int(q, r, s), out Hex hex);
        return hex;
    }
}