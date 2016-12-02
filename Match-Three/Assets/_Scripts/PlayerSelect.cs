using UnityEngine;
using System.Collections;

public class PlayerSelect : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown()
    {
        //Debug.Log("Mouse Down on : " + gameObject.transform.localPosition);
        gameObject.GetComponentInParent<SimpleMatrix>().playerMove(gameObject.transform.localPosition);
    }
}
