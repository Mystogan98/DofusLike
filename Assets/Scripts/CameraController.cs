using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public float min = 2, max = 7, baseZoom = 4.95f, speedZoom, speedMove;

	private float zoom, mouseScrollWheel;
	private new Camera camera;

	// Use this for initialization
	void Start () {
		zoom = baseZoom;
		camera = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
		mouseScrollWheel = Input.GetAxisRaw("Mouse ScrollWheel");
		if(mouseScrollWheel != 0f)
		{
			if(mouseScrollWheel > 0f) {
				zoom -= speedZoom;
			} else {
				zoom += speedZoom;
			}
			zoom = Mathf.Clamp(zoom, min, max);
			camera.orthographicSize = zoom;
		}
	}
}
