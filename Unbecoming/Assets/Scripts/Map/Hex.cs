using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A point-top hexagonal tile on the map, represented in cube coordinates (q, r, s) where q + r + s = 0.
/// </summary>
public class Hex {
    #region Coordinates
    public readonly int Q; 
    public readonly int R; 
    public readonly int S; 

    static readonly float WIDTH_MULTIPLIER = Mathf.Sqrt(3) / 2; 

    public Hex(int q, int r, int s) {
        if (q + r + s != 0) {
            throw new System.ArgumentException("q + r + s must be 0");
        }
        Q = q;
        R = r;
        S = s;
    }

    public Hex(int q, int r) : this(q, r, -(q + r)) { }

    public Vector3 ToWorldPosition(float HexSize = 1.0f) {
        float height = HexSize * 2;
        float width = height * WIDTH_MULTIPLIER;

        return new Vector3(
            width * (Q + R / 2f),
            0,
            height * (R * 0.75f)
        );
    }
    #endregion

    #region Occupancy
    public bool Passable => OccupiedEntity == null;

    public List<Terrain> Terrains = new List<Terrain>();
    public Entity OccupiedEntity = null;
    #endregion
}
