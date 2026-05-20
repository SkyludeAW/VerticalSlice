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
    public Vector3Int Position => new Vector3Int(Q, R, S);

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

    public Hex(Vector3Int position) : this(position.x, position.y, position.z) { }

    public Vector3 ToWorldPosition(float HexSize = 1.0f) {
        float height = HexSize * 2;
        float width = height * WIDTH_MULTIPLIER;

        return new Vector3(
            width * (Q + R / 2f),
            0,
            height * (R * 0.75f)
        );
    }

    public int DistanceTo(Hex other) {
        return (Mathf.Abs(Q - other.Q) + Mathf.Abs(R - other.R) + Mathf.Abs(S - other.S)) / 2;
    }
    #endregion

    #region Neighbors

    private static readonly Hex[] Directions = new Hex[] {
        new Hex( 1, -1,  0),
        new Hex( 1,  0, -1),
        new Hex( 0,  1, -1),
        new Hex(-1,  1,  0),
        new Hex(-1,  0,  1),
        new Hex( 0, -1,  1)
    };

    public Hex Add(Hex other) {
        return new Hex(
            Q + other.Q,
            R + other.R,
            S + other.S
        );
    }

    public List<Hex> GetNeighbors() {
        List<Hex> neighbors = new List<Hex>();

        foreach (Hex dir in Directions) {
            neighbors.Add(Add(dir));
        }

        return neighbors;
    }

    #endregion

    #region Equality

    public override bool Equals(object obj) {
        if (obj is Hex other) {
            return Q == other.Q &&
                   R == other.R &&
                   S == other.S;
        }

        return false;
    }

    public override int GetHashCode() {
        return (Q, R, S).GetHashCode();
    }

    #endregion

    #region Occupancy
    public bool IsPassable;
    public bool IsTransparent;

    public List<Terrain> Terrains = new List<Terrain>();
    public Entity OccupiedEntity = null;
    #endregion
}
