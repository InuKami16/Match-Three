using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour {

    public float speed;
    private Transform trans;
    private float timer;
    private float h;
    private float d;
    private float v;

	// Use this for initialization
	void Start () {
        trans = GetComponent<Transform>();
        timer = 0f;
	}
	
	// Update is called once per frame
	void Update () {
        h = Input.GetAxis("Horizontal") * speed;
        v = Input.GetAxis("Vertical") * speed;
        d = 0f;
        if (Input.GetKey(KeyCode.Space))
        {
            d = speed;
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            d = -speed;
        }

        if (h == 0f && v == 0f && d == 0f)
        {
            timer = 0f;
        }

        timer += Time.deltaTime;

        Vector3 deltaPosZ = new Vector3(Mathf.Sin(trans.rotation.eulerAngles.y * Mathf.Deg2Rad) * v, d / 2, Mathf.Cos(trans.rotation.eulerAngles.y * Mathf.Deg2Rad) * v) * speed;
        Vector3 deltaPosX = new Vector3(Mathf.Cos(trans.rotation.eulerAngles.y * Mathf.Deg2Rad) * h, d / 2, -Mathf.Sin(trans.rotation.eulerAngles.y * Mathf.Deg2Rad) * h) * speed;
        trans.localPosition = Vector3.Lerp(trans.localPosition, trans.localPosition + deltaPosZ + deltaPosX, timer);
	}
}
