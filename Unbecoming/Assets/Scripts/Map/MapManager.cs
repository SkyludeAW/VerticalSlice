using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {
    public GameObject HexPrefab;
    public Entity ObstaclePrefab;

    public List<Hex> Hexes = new List<Hex>();

    private void Start() {
        GenerateMap();
    }

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

    private void GenerateMap() {
        for (int i = 0; i < 10; i++) {
            for (int j = 0; j < 10; j++) {
                Hex hex = new Hex(i, j);
                Hexes.Add(hex);

                if (System.Array.Exists(ObstacleCoordinates, coord => coord[0] == i && coord[1] == j)) {
                    // Add an obstacle entity to this hex
                    hex.OccupiedEntity = Instantiate(ObstaclePrefab, hex.ToWorldPosition(), Quaternion.identity, this.transform);
                }

                Instantiate(HexPrefab, hex.ToWorldPosition(), Quaternion.identity, this.transform);
            }
        }
    }
}
