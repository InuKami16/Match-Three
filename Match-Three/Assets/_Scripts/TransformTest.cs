using UnityEngine;
using System.Collections;

public class TransformTest : MonoBehaviour {

    GameObject[] objs;

	// Use this for initialization
	void Start () {
        objs = GameObject.FindGameObjectsWithTag("Object");
        Debug.Log(objs[0] + "\n" + objs[1]);
        GameObject temp = objs[0];
        Vector3[] positions = { objs[0].transform.localPosition, objs[1].transform.localPosition };
        objs[0] = objs[1];
        objs[1] = temp;
        objs[0].transform.localPosition = positions[0];
        objs[1].transform.localPosition = positions[1];
        Debug.Log(objs[0] + "\n" + objs[1]);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
