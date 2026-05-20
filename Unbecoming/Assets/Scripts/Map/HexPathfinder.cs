using System.Collections.Generic;
using UnityEngine;

public static class HexPathfinder {

    public static List<Hex> FindPath(
        MapManager map,
        Hex start,
        Hex goal,
        int maxDistance
    ) {

        List<Hex> openSet = new List<Hex>();
        HashSet<Hex> closedSet = new HashSet<Hex>();

        openSet.Add(start);

        Dictionary<Hex, Hex> cameFrom = new Dictionary<Hex, Hex>();

        Dictionary<Hex, int> gScore = new Dictionary<Hex, int>();
        Dictionary<Hex, int> fScore = new Dictionary<Hex, int>();

        gScore[start] = 0;
        fScore[start] = start.DistanceTo(goal);

        while (openSet.Count > 0) {

            // Find node with lowest fScore
            Hex current = openSet[0];

            foreach (Hex hex in openSet) {

                int currentF = fScore.ContainsKey(current)
                    ? fScore[current]
                    : int.MaxValue;

                int hexF = fScore.ContainsKey(hex)
                    ? fScore[hex]
                    : int.MaxValue;

                if (hexF < currentF) {
                    current = hex;
                }
            }

            // Reached goal
            if (current.Equals(goal)) {

                List<Hex> path = ReconstructPath(cameFrom, current);

                // Check agility movement limit
                if (path.Count - 1 <= maxDistance) {
                    return path;
                }

                return null;
            }

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (Hex neighborCoord in current.GetNeighbors()) {

                Hex neighbor = map.GetHex(
                    neighborCoord.Q,
                    neighborCoord.R,
                    neighborCoord.S
                );

                // Outside map
                if (neighbor == null)
                    continue;

                // Blocked
                if (!neighbor.IsPassable && !neighbor.Equals(goal))
                    continue;

                if (closedSet.Contains(neighbor))
                    continue;

                int tentativeG = gScore[current] + 1;

                // Early rejection if exceeding agility
                if (tentativeG > maxDistance)
                    continue;

                if (!openSet.Contains(neighbor)) {
                    openSet.Add(neighbor);
                } else if (
                      gScore.ContainsKey(neighbor) &&
                      tentativeG >= gScore[neighbor]
                  ) {
                    continue;
                }

                cameFrom[neighbor] = current;
                gScore[neighbor] = tentativeG;
                fScore[neighbor] =
                    tentativeG + neighbor.DistanceTo(goal);
            }
        }

        return null;
    }

    private static List<Hex> ReconstructPath(
        Dictionary<Hex, Hex> cameFrom,
        Hex current
    ) {

        List<Hex> totalPath = new List<Hex>();
        totalPath.Add(current);

        while (cameFrom.ContainsKey(current)) {
            current = cameFrom[current];
            totalPath.Insert(0, current);
        }

        return totalPath;
    }
}
