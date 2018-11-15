using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CellScript : MonoBehaviour {

	public TargetScript target;

	private bool _selected, _over, hasChanged;
	private bool selected { set { _selected = value; hasChanged = true; } get { return _selected; } }
	private bool over { set { _over = value; hasChanged = true; } get { return _over; } }
	
	// Update is called once per frame
	void Update () {
		if(hasChanged)
		{
			if(selected)
				((SpriteRenderer) GetComponent<Renderer>()).color = Color.red;
			else if (over)
				((SpriteRenderer) GetComponent<Renderer>()).color = Color.blue;
			else
				((SpriteRenderer) GetComponent<Renderer>()).color = Color.white;
			hasChanged = false;
		}
	}

	private void OnMouseEnter() {
		over = true;
	}

	private void OnMouseExit() {
		over = false;
	}

	private void OnMouseUp() {
		PlayerScript.SelectCell(this);
	}

	public void Select()
	{
		selected = true;
	}

	public void Unselect()
	{
		selected = false;
	}
}