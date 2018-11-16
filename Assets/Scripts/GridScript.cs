using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class GridScript : MonoBehaviour {

	[Header("DO NOT WORK WITH ROTATIONS YET")]
	[Space(10)]

	[Header("Run in Update ?")]
	public bool debug;
	[Space(10)]

	[Header("Grid parameters")]
	public int nbX;
	public int nbY;
	public float sizeX, sizeY, offsetX, offsetY;

	private float globalOffsetX, globalOffsetY;
	private int ix = 0, iy = 0;
	private bool outOfBounds = false;

	// Use this for initialization
	void Start () {
		globalOffsetX = -transform.position.x + (nbX*(sizeX+offsetX))/2 - (sizeX+offsetX)/2;
		globalOffsetY = transform.position.y + (nbY*(sizeY+offsetY))/2 - (sizeY+offsetY)/2;

		foreach(Transform child in transform)
		{
			if(outOfBounds)
				child.gameObject.SetActive(false);
			else
			{
				child.transform.position = new Vector3(-globalOffsetX + ix * (sizeX + offsetX), globalOffsetY - iy * (sizeY + offsetY), 0);
				//child.transform.rotation *= transform.rotation;		// a * on Quaternions is a + on angles 	DO NOT WORK

				CellManager.AddCell(child.gameObject.GetComponent<CellScript>(), ix, iy);

				ix++;
				if(ix >= nbX)
				{
					ix = 0;
					iy++;
				}
				if(iy > nbY)
				{
					outOfBounds = true;
					Debug.LogError("OutOfBounds : more children than spaces in the grid.");
				}
			}
		}
	}

	void Update() {
		if(debug)
		{
			ix = 0; iy = 0;
			outOfBounds = false;
			Start();
		}
	}
}


 [CustomEditor(typeof(GridScript))]
 public class GridScriptEditor : Editor {
    
    override public void OnInspectorGUI () {

        DrawDefaultInspector();
		EditorGUILayout.Space();
		
        if(GUILayout.Button("Set all children to Active")) {

			foreach(Transform child in ((GridScript) target).transform)
				child.gameObject.SetActive(true);
        }
    }
}