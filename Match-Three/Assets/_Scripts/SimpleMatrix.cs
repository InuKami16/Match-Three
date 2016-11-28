using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimpleMatrix : MonoBehaviour {

    public GameObject[] prefabs;
    private GameObject[,] matrix;
    private List<MatchTwo> matchTwos;
    private List<MatchThree> matchThrees;

	// Use this for initialization
	void Start () {
        matrix = new GameObject[10, 10];
        matchTwos = new List<MatchTwo>();
        matchThrees = new List<MatchThree>();

        //Creates the array of objects from the prefabs
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                matrix[i, j] = Instantiate(prefabs[(int)Random.Range(0f, prefabs.Length)]);
                matrix[i, j].transform.parent = gameObject.transform;
                matrix[i, j].transform.localPosition = new Vector3(j, i, 0f);
                matrix[i, j].transform.rotation = Quaternion.identity;
                matrix[i, j].name = "Cell[" + j + ", " + i + "]";
            }
        }
        checkForTwo();
        checkForThree(matchTwos);
        /*
        Debug.Log(matchThrees.Count);
        */
        /*
        foreach (MatchTwo m in matchTwos)
        {
            Debug.Log(m.ToString());
        }
        */
        /*
        foreach (MatchThree m in matchThrees)
        {
            Debug.Log(m.ToString());
        }
        */
        mark();
        //removeThrees();
        
        foreach(GameObject obj in matrix)
        {
            if (obj != null)
            {
                Debug.Log(obj.name);
            }
            else
            {
                Debug.Log(obj);
            }
        }
        
        //dropDown();
    }

	
	// Update is called once per frame
	void Update () { 

        //Debug quick restarts
        if (Input.GetKeyDown(KeyCode.Space))
        {
            restart();
        }
	}

    
    //Accessor method for the matrix
    public GameObject[,] getMatrix()
    {
        return matrix;
    }

    //Accessor method for the matching pairs
    public List<MatchTwo> getPairs()
    {
        return matchTwos;
    }

    //Accessor method for the matching triples
    public List<MatchThree> getTriples()
    {
        return matchThrees;
    }


    //Debug, can be used later for quick restarts as well
    public void restart()
    {
        foreach(GameObject obj in matrix)
        {
            Destroy(obj);
        }
        Start();
    }

    /* Loops through the array to find at least 2 in a row and creates an array of all two in a row
     */
    private void checkForTwo()
    {
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                /* Checks across in the x direction up to the maximum x - 3 from y = 0 to y = y max
                 */
                if (i < matrix.GetLength(0) - 3)
                {
                    if (matrix[i, j].CompareTag(matrix[i + 1, j].tag))
                    {
                        matchTwos.Add(new MatchTwo(matrix[i, j].transform.localPosition, matrix[i + 1, j].transform.localPosition));
                    }
                    if (matrix[i, j].CompareTag(matrix[i + 2, j].tag))
                    {
                        matchTwos.Add(new MatchTwo(matrix[i, j].transform.localPosition, matrix[i + 2, j].transform.localPosition));
                    }
                }
                /* Checks up in the y direction up to the maximum y - 3 from x = 0 to x = x max
                 */
                if (j < matrix.GetLength(1) - 3)
                {
                    if (matrix[i, j].CompareTag(matrix[i, j + 1].tag))
                    {
                        matchTwos.Add(new MatchTwo(matrix[i, j].transform.localPosition, matrix[i, j + 1].transform.localPosition));
                    }
                    if (matrix[i, j].CompareTag(matrix[i, j + 2].tag))
                    {
                        matchTwos.Add(new MatchTwo(matrix[i, j].transform.localPosition, matrix[i, j + 2].transform.localPosition));
                    }
                }
            }
        }
        /* Checks across in the x direction from maximum x - 3 to maximum x from y = 0 to y = y max
         */
        for (int j = 0; j < matrix.GetLength(1); j++)
        {
            if (matrix[matrix.GetLength(0) - 3, j].CompareTag(matrix[matrix.GetLength(0) - 2, j].tag))
            {
                matchTwos.Add(new MatchTwo(matrix[matrix.GetLength(0) - 3, j].transform.localPosition, matrix[matrix.GetLength(0) - 2, j].transform.localPosition));
            }
            if (matrix[matrix.GetLength(0) - 3, j].CompareTag(matrix[matrix.GetLength(0) - 1, j].tag))
            {
                matchTwos.Add(new MatchTwo(matrix[matrix.GetLength(0) - 3, j].transform.localPosition, matrix[matrix.GetLength(0) - 1, j].transform.localPosition));
            }
            if (matrix[matrix.GetLength(0) - 2, j].CompareTag(matrix[matrix.GetLength(0) - 1, j].tag))
            {
                matchTwos.Add(new MatchTwo(matrix[matrix.GetLength(0) - 2, j].transform.localPosition, matrix[matrix.GetLength(0) - 1, j].transform.localPosition));
            }
        }
        /* Checks up in the y direction from maximum y - 3 to maximum y from x = 0 to x = x max
         */
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            if (matrix[i, matrix.GetLength(1) - 3].CompareTag(matrix[i, matrix.GetLength(1) - 2].tag))
            {
                matchTwos.Add(new MatchTwo(matrix[i, matrix.GetLength(1) - 3].transform.localPosition, matrix[i, matrix.GetLength(1) - 2].transform.localPosition));
            }
            if (matrix[i, matrix.GetLength(1) - 3].CompareTag(matrix[i, matrix.GetLength(1) - 1].tag))
            {
                matchTwos.Add(new MatchTwo(matrix[i, matrix.GetLength(1) - 3].transform.localPosition, matrix[i, matrix.GetLength(1) - 1].transform.localPosition));
            }
            if (matrix[i, matrix.GetLength(1) - 2].CompareTag(matrix[i, matrix.GetLength(1) - 1].tag))
            {
                matchTwos.Add(new MatchTwo(matrix[i, matrix.GetLength(1) - 2].transform.localPosition, matrix[i, matrix.GetLength(1) - 1].transform.localPosition));
            }
        }
    }

    /* Checks for a match three by looping through a list of MatchTwo.
     * If there is a match three on the board, then there are 3 specific matchTwo that must be in the list, which is what this code uses to find the match threes.
     * Populates the matchThrees list for future use and removing.  
     */
    private void checkForThree(List<MatchTwo> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Vector3[] coords = list[i].getCoordPairs();
            if (coords[1].x - coords[0].x == 1)
            {
                Vector3 coord = new Vector3(coords[0].x + 2, coords[0].y);
                if (contains(new MatchTwo(coords[0], coord)) && contains(new MatchTwo(coords[1], coord)))
                {
                    matchThrees.Add(new MatchThree(coords[0], coords[1], coord));
                    matchTwos.RemoveAt(i);
                    i--;
                }
            }
            /*
            else if (coords[1].x - coords[0].x == 2)
            {
                Vector3 coord = new Vector3(coords[0].x + 1, coords[0].y);
                if (contains(new MatchTwo(coords[0], coord)) && contains(new MatchTwo(coord, coords[1])))
                {
                    matchThrees.Add(new MatchThree(coords[0], coord, coords[1]));
                    matchTwos.RemoveAt(i);
                }
            }
            */
            if (coords[1].y - coords[0].y == 1)
            {
                Vector3 coord = new Vector3(coords[0].x, coords[0].y + 2);
                if (contains(new MatchTwo(coords[0], coord)) && contains(new MatchTwo(coords[1], coord)))
                {
                    matchThrees.Add(new MatchThree(coords[0], coords[1], coord));
                    matchTwos.RemoveAt(i);
                    i--;
                }
            }
            /*
            else if (coords[1].y - coords[0].y == 2)
            {
                Vector3 coord = new Vector3(coords[0].x, coords[0].y + 1);
                if (contains(new MatchTwo(coords[0], coord)) && contains(new MatchTwo(coord, coords[1])))
                {
                    matchThrees.Add(new MatchThree(coords[0], coord, coords[1]));
                    matchTwos.RemoveAt(i);
                }
            }
            */
        }
    }

    /* Method used to see if the matchTwo list contains a specific pair 
     * Uses linear search
     */
    private bool contains(MatchTwo pair)
    {
        foreach (MatchTwo m in matchTwos)
        {
            if (m.Equals(pair))
                return true;
        }
        return false;
    }

    /* Debug code that is used to easily see all the three in a rows
     * Will be used later on for the logic of finding higher matches ex. 4 in a row
     */
    private void mark()
    {
        foreach (MatchThree triple in matchThrees)
        {
            Vector3[] coords = triple.getCoordTriple();
            foreach (Vector3 coord in coords)
                matrix[(int)coord.y, (int)coord.x].GetComponentInChildren<MeshRenderer>().material.color = Color.cyan;
        }
    }

    /* Removes all three in a rows from the matrix and matchThrees
     */
    private void removeThrees()
    {
        while (matchThrees.Count != 0)
        {
            Vector3[] coords = matchThrees[0].getCoordTriple();
            foreach (Vector3 coord in coords)
            {
                //Makes sure that it removes higher matches as well
                if (matrix[(int)coord.y, (int)coord.x] != null)
                {
                    Destroy(matrix[(int)coord.y, (int)coord.x].gameObject);
                    matrix[(int)coord.y, (int)coord.x] = null;
                }
            }
            matchThrees.RemoveAt(0);
        }
    }

    /* Moves everything down after the matches are removed
     */
    private void dropDown()
    {
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                if (matrix[i, j] == null)
                {
                    Debug.Log("x: " + i + " y: " + j);
                    for (int h = j + 1; h < matrix.GetLength(1); h++)
                    {
                        if (matrix[i, h] != null)
                        {
                            //Debug.Log(matrix[i, h].name);
                            matrix[i, j] = matrix[i, h];
                            matrix[i, j].gameObject.transform.position = new Vector3(i, j);
                        }
                    }
                }
            }
        }
    }
}
