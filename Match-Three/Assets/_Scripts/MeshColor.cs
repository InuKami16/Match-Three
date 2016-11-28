using UnityEngine;
using System.Collections;

public class MeshColor : MonoBehaviour {

    public Color color;
    private MeshRenderer meshRenderer;

	// Use this for initialization
	void Awake () {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.color = color;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
