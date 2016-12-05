using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimpleMatrix : MonoBehaviour {

    public GameObject[] prefabs;
    private GameObject[,] matrix;
    private List<MatchTwo> matchTwos;
    private List<MatchThree> matchThrees;
    private bool wasChanged;
    private Vector3[] playerSelected;

    // Use this for initialization
    void Start()
    {
        matrix = new GameObject[10, 10];
        matchTwos = new List<MatchTwo>();
        matchThrees = new List<MatchThree>();
        playerSelected = new Vector3[2];
        wasChanged = true;

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

        StartCoroutine(updateGame());
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

        /*
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
        */
    }

    // Update is called once per frame
    void Update()
    {
        //Debug quick restarts
        if (Input.GetKeyDown(KeyCode.Space))
        {
            restart();
        }

        if (isValidMove())
        {
            //Debug.Log("Valid");
            StopAllCoroutines();
            validMove();
            StartCoroutine(updateGame());
        }
        else if ((playerSelected[0] - playerSelected[1]).magnitude == 1)
        {
            //Debug.Log("Invalid");
            StartCoroutine(invalidMove());
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
            StopAllCoroutines();
            Destroy(obj);
        }
        Start();
    }

    /* Updates the game with a delay so the moves can be seen
     */
    private IEnumerator updateGame()
    {
        yield return new WaitForSeconds(1f);
        if (wasChanged)
        {
            checkForTwo();
            checkForThree(matchTwos);

            Debug.Log("Match threes");
            foreach (MatchThree i in matchThrees)
            {
                Debug.Log(i.ToString());
            }

            mark();
            yield return new WaitForSeconds(1f);
            removeThrees();
            yield return new WaitForSeconds(1f);
            dropDown();
            yield return new WaitForSeconds(1f);
            repopulate();
            if (wasChanged)
            {
                StartCoroutine(updateGame());
            }
        }
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
                        matchTwos.Add(new MatchTwo(matrix[i, j].transform.localPosition, matrix[i + 1, j].transform.localPosition, matrix[i, j].tag));
                    }
                    if (matrix[i, j].CompareTag(matrix[i + 2, j].tag))
                    {
                        matchTwos.Add(new MatchTwo(matrix[i, j].transform.localPosition, matrix[i + 2, j].transform.localPosition, matrix[i, j].tag));
                    }
                }
                /* Checks up in the y direction up to the maximum y - 3 from x = 0 to x = x max
                 */
                if (j < matrix.GetLength(1) - 3)
                {
                    if (matrix[i, j].CompareTag(matrix[i, j + 1].tag))
                    {
                        matchTwos.Add(new MatchTwo(matrix[i, j].transform.localPosition, matrix[i, j + 1].transform.localPosition, matrix[i, j].tag));
                    }
                    if (matrix[i, j].CompareTag(matrix[i, j + 2].tag))
                    {
                        matchTwos.Add(new MatchTwo(matrix[i, j].transform.localPosition, matrix[i, j + 2].transform.localPosition, matrix[i, j].tag));
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
                matchTwos.Add(new MatchTwo(matrix[matrix.GetLength(0) - 3, j].transform.localPosition, matrix[matrix.GetLength(0) - 2, j].transform.localPosition, matrix[matrix.GetLength(0) - 3, j].tag));
            }
            if (matrix[matrix.GetLength(0) - 3, j].CompareTag(matrix[matrix.GetLength(0) - 1, j].tag))
            {
                matchTwos.Add(new MatchTwo(matrix[matrix.GetLength(0) - 3, j].transform.localPosition, matrix[matrix.GetLength(0) - 1, j].transform.localPosition, matrix[matrix.GetLength(0) - 3, j].tag));
            }
            if (matrix[matrix.GetLength(0) - 2, j].CompareTag(matrix[matrix.GetLength(0) - 1, j].tag))
            {
                matchTwos.Add(new MatchTwo(matrix[matrix.GetLength(0) - 2, j].transform.localPosition, matrix[matrix.GetLength(0) - 1, j].transform.localPosition, matrix[matrix.GetLength(0) - 2, j].tag));
            }
        }
        /* Checks up in the y direction from maximum y - 3 to maximum y from x = 0 to x = x max
         */
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            if (matrix[i, matrix.GetLength(1) - 3].CompareTag(matrix[i, matrix.GetLength(1) - 2].tag))
            {
                matchTwos.Add(new MatchTwo(matrix[i, matrix.GetLength(1) - 3].transform.localPosition, matrix[i, matrix.GetLength(1) - 2].transform.localPosition, matrix[i, matrix.GetLength(1) - 3].tag));
            }
            if (matrix[i, matrix.GetLength(1) - 3].CompareTag(matrix[i, matrix.GetLength(1) - 1].tag))
            {
                matchTwos.Add(new MatchTwo(matrix[i, matrix.GetLength(1) - 3].transform.localPosition, matrix[i, matrix.GetLength(1) - 1].transform.localPosition, matrix[i, matrix.GetLength(1) - 3].tag));
            }
            if (matrix[i, matrix.GetLength(1) - 2].CompareTag(matrix[i, matrix.GetLength(1) - 1].tag))
            {
                matchTwos.Add(new MatchTwo(matrix[i, matrix.GetLength(1) - 2].transform.localPosition, matrix[i, matrix.GetLength(1) - 1].transform.localPosition, matrix[i, matrix.GetLength(1) - 2].tag));
            }
        }
    }

    /* Checks for a match three by looping through a list of MatchTwo
     * If there is a match three on the board, then there are 3 specific matchTwo that must be in the list, which is what this code uses to find the match threes
     * Populates the matchThrees list for future use and removing
     */
    private void checkForThree(List<MatchTwo> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Vector3[] coords = list[i].getCoordPairs();
            string shape = list[i].getShape();

            if (coords[1].x - coords[0].x == 1f || coords[0].x - coords[1].x == 1f)
            {
                Vector3 coord = new Vector3(coords[0].x + 2, coords[0].y);
                if (contains(new MatchTwo(coords[0], coord, shape)) && contains(new MatchTwo(coords[1], coord, shape)))
                {
                    matchThrees.Add(new MatchThree(coords[0], coords[1], coord, shape));
                }
            }

            if (coords[1].y - coords[0].y == 1f || coords[0].y - coords[1].y == 1f)
            {
                Vector3 coord = new Vector3(coords[0].x, coords[0].y + 2);
                if (contains(new MatchTwo(coords[0], coord, shape)) && contains(new MatchTwo(coords[1], coord, shape)))
                {
                    matchThrees.Add(new MatchThree(coords[0], coords[1], coord, shape));
                }
            }
        }
        matchTwos.Clear();
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
                    //Debug.Log("x: " + j + " y: " + i);
                    for (int h = i + 1; h < matrix.GetLength(0); h++)
                    {
                        if (matrix[h, j] != null)
                        {
                            //Debug.Log(matrix[h, j].name);
                            matrix[i, j] = matrix[h, j];
                            matrix[h, j] = null;
                            matrix[i, j].name = "Cell[" + j + ", " + i + "]";
                            matrix[i, j].transform.localPosition = new Vector3(j, i);
                            break;
                        }
                    }
                }
            }
        }
    }

    /* Fills the empty spaces in the matrix with new shapes 
     */
    private void repopulate()
    {
        bool change = false;
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                if (matrix[i, j] == null)
                {
                    matrix[i, j] = Instantiate(prefabs[(int)Random.Range(0f, prefabs.Length)]);
                    matrix[i, j].transform.parent = gameObject.transform;
                    matrix[i, j].transform.localPosition = new Vector3(j, i, 0f);
                    matrix[i, j].transform.rotation = Quaternion.identity;
                    matrix[i, j].name = "Cell[" + j + ", " + i + "]";
                    change = true;
                }
            }
        }
        wasChanged = change;
    }

    /* Called outside of the class to add the r to the playerSelected array
     * playerSelected array can only hold two Vector3's, each representing an object in matrix 
     * that was selected by the player
     */
    public void playerMove(Vector3 r)
    {
        for (int i = 0; i < playerSelected.Length; i++)
        {
            if (playerSelected[i].Equals(new Vector3(-1f, -1f, -1f)))
            {
                playerSelected[i] = r;
                break;
            }
        }
    }

    /* Initializes playerSelected to -1, -1, -1
     */
    private void initializeMove()
    {
        for (int i = 0; i < playerSelected.Length; i++)
        {
            playerSelected[i] = new Vector3(-1f, -1f, -1f);
        }
    }

    /* If the playerSelected array is full of non initialized Vector3, then continue
     * If the selected objects are next to each other, then continue checking
     * If the game is not moving anything else, then continue
     */
    private bool isValidMove()
    {
        /* Makes sure that the player's moves are only valid while the board is not changing
         */
        if (wasChanged)
        {
            initializeMove();
            return false;
        }

        //Checks if the playerSelected is full or not, if it is full then continue, else returns false
        foreach (Vector3 r in playerSelected)
        {
            if (r.Equals(new Vector3(-1f, -1f, -1f)))
                return false;
        }

        /* Checks if the selected tiles are 1 apart ie. next to each other
         * The magnitude of the difference between two positions that are next to each other is 1
         * If they're not next to each other, then the older position is removed and the new one is kept
         */
        if ((playerSelected[0] - playerSelected[1]).magnitude != 1f)
        {
            Vector3 temp = playerSelected[1];
            initializeMove();
            playerSelected[0] = temp;
            return false;
        }

        /* Holds the old fields in case the switch does not yield a valid move
         */
        GameObject[,] oldMatrix = new GameObject[matrix.GetLength(0), matrix.GetLength(1)];
        System.Array.Copy(matrix, oldMatrix, matrix.Length);

        Debug.Log("Old matrix");
        foreach (GameObject obj in matrix)
        {
            Debug.Log(obj.name + ": " + obj.tag + " " + obj.transform.localPosition.ToString());
        }

        List<MatchTwo> oldMatchTwos = new List<MatchTwo>();
        oldMatchTwos.AddRange(matchTwos);

        List<MatchThree> oldMatchThrees = new List<MatchThree>();
        oldMatchThrees.AddRange(matchThrees);
        
        /* Checks if the move creates a match three
         * If the move does create a match three, then wasChanged will be true and is a valid move
         * If the move does not create a match three, then wasChanged will be false and is an invalid move
         */
        GameObject[] holder = { matrix[(int)playerSelected[0].y, (int)playerSelected[0].x], matrix[(int)playerSelected[1].y, (int)playerSelected[1].x] };

        //Debug.Log(matrix[(int)playerSelected[0].y, (int)playerSelected[0].x].tag + "\n" + matrix[(int)playerSelected[1].y, (int)playerSelected[1].x].tag);
        matrix.SetValue(holder[0], (int)playerSelected[1].y, (int)playerSelected[1].x);
        matrix.SetValue(holder[1], (int)playerSelected[0].y, (int)playerSelected[0].x);
        matrix[(int)playerSelected[0].y, (int)playerSelected[0].x].transform.localPosition = playerSelected[0];
        matrix[(int)playerSelected[1].y, (int)playerSelected[1].x].transform.localPosition = playerSelected[1];

        /*
        Debug.Log(matrix[(int)playerSelected[0].y, (int)playerSelected[0].x] + "\n" + matrix[(int)playerSelected[1].y, (int)playerSelected[1].x]);
        Debug.Log(matrix[(int)playerSelected[0].y, (int)playerSelected[0].x].transform.localPosition + "\n" + matrix[(int)playerSelected[1].y, (int)playerSelected[1].x].transform.localPosition);
        Debug.Log(matrix[(int)playerSelected[0].y, (int)playerSelected[0].x].tag + "\n" + matrix[(int)playerSelected[1].y, (int)playerSelected[1].x].tag);
        */

        checkForTwo();

        /*
        foreach(MatchTwo m in matchTwos)
        {
            Debug.Log(m.ToString());
        }
        */

        checkForThree(matchTwos);

        /*
        Debug.Log("New Match Threes");
        foreach(MatchThree m in matchThrees)
        {
            Debug.Log(m.ToString());
        }
        */

        matrix.SetValue(holder[0], (int)playerSelected[0].y, (int)playerSelected[0].x);
        matrix.SetValue(holder[1], (int)playerSelected[1].y, (int)playerSelected[1].x);
        matrix[(int)playerSelected[0].y, (int)playerSelected[0].x].transform.localPosition = playerSelected[0];
        matrix[(int)playerSelected[1].y, (int)playerSelected[1].x].transform.localPosition = playerSelected[1];
        
        Debug.Log("Revert to old matrix");
        foreach (GameObject obj in matrix)
        {
            Debug.Log(obj.name + ": " + obj.tag + " " + obj.transform.localPosition.ToString());
        }

        return matchThrees.Count > 0;
    }

    /* validMove and invalidMove swaps the selected cells if they are next to each other
     * If the move was invalid, the cells are switched back
     */
    private void validMove()
    {
        Debug.Log("Start valid");
        foreach (GameObject obj in matrix)
        {
            Debug.Log(obj.name + ": " + obj.tag + " " + obj.transform.localPosition.ToString());
        }

        GameObject[] temp = { matrix[(int)playerSelected[0].y, (int)playerSelected[0].x], matrix[(int)playerSelected[1].y, (int)playerSelected[1].x] };
        matrix.SetValue(temp[0], (int)playerSelected[1].y, (int)playerSelected[1].x);
        matrix.SetValue(temp[1], (int)playerSelected[0].y, (int)playerSelected[0].x);

        matrix[(int)playerSelected[0].y, (int)playerSelected[0].x].name = "Cell[" + playerSelected[0].x + ", " + playerSelected[0].y + "]";
        matrix[(int)playerSelected[1].y, (int)playerSelected[1].x].name = "Cell[" + playerSelected[1].x + ", " + playerSelected[1].y + "]";
        
        matrix[(int)playerSelected[0].y, (int)playerSelected[0].x].transform.localPosition = playerSelected[0];
        matrix[(int)playerSelected[1].y, (int)playerSelected[1].x].transform.localPosition = playerSelected[1];

        wasChanged = true;

        Debug.Log("End valid");
        foreach (GameObject obj in matrix)
        {
            Debug.Log(obj.name + ": " + obj.tag + " " + obj.transform.localPosition.ToString());
        }
        //Debug.Log(matrix[(int)playerSelected[0].y, (int)playerSelected[0].x].ToString() + ": " + matrix[(int)playerSelected[0].y, (int)playerSelected[0].x].tag + matrix[(int)playerSelected[1].y, (int)playerSelected[1].x].ToString() + ": " + matrix[(int)playerSelected[1].y, (int)playerSelected[1].x].tag);

        initializeMove();
    }

    private IEnumerator invalidMove()
    {
        matrix[(int)playerSelected[0].y, (int)playerSelected[0].x].transform.localPosition = playerSelected[1];
        matrix[(int)playerSelected[1].y, (int)playerSelected[1].x].transform.localPosition = playerSelected[0];
        yield return new WaitForSeconds(1f);
        matrix[(int)playerSelected[0].y, (int)playerSelected[0].x].transform.localPosition = playerSelected[0];
        matrix[(int)playerSelected[1].y, (int)playerSelected[1].x].transform.localPosition = playerSelected[1];
        initializeMove();
    }
}
