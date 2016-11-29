using UnityEngine;
using System.Collections;

public class MatchTwo {

    protected Vector3[] coordPairs;
    protected string shape;

    public MatchTwo(Vector3 coord1, Vector3 coord2, string s)
    {
        coordPairs = new Vector3[2];
        coordPairs[0] = coord1;
        coordPairs[1] = coord2;
        shape = s;
    }

    public Vector3[] getCoordPairs()
    {
        return coordPairs;
    }

    public string getShape()
    {
        return shape;
    }

    public override string ToString()
    {
        return "Coordinate 1\nx: " + coordPairs[0].x + " y: " + coordPairs[0].y + " z: " + coordPairs[0].z + "\n" + "Coordinate 2\nx: " + coordPairs[1].x + " y: " + coordPairs[1].y + " z: " + coordPairs[1].z + "\n";
    }

    public bool Equals(MatchTwo obj)
    {
        return obj.getCoordPairs()[0].Equals(coordPairs[0]) && obj.getCoordPairs()[1].Equals(coordPairs[1]) && obj.getShape().Equals(shape);
    }
}
