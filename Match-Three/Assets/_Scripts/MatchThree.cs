using UnityEngine;
using System.Collections;

public class MatchThree : MatchTwo {

    protected Vector3[] coordTriple;

    public MatchThree(Vector3 coord1, Vector3 coord2, Vector3 coord, string s) : base(coord1, coord2, s)
    {
        coordTriple = new Vector3[3];
        coordTriple[0] = coordPairs[0];
        coordTriple[1] = coordPairs[1];
        coordTriple[2] = coord;
        shape = s;
    }

    public Vector3[] getCoordTriple()
    {
        return coordTriple;
    }

    public override string ToString()
    {
        return base.ToString() + "Coordinate 3\nx: " + coordTriple[2].x + " y: " + coordTriple[2].y + " z: " + coordTriple[2].z + "\n";
    }
}
