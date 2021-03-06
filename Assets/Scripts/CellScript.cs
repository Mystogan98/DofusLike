﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CellScript : MonoBehaviour {

	public TargetScript target;
	[HideInInspector]
	public int x, y;

	private bool _selected, _over, _isInPath, _isInMoveRange, _isInSpellRange, hasChanged;
	public bool selected { set { _selected = value; hasChanged = true; } get { return _selected; } }
	public bool over { set { _over = value; hasChanged = true; } get { return _over; } }
	public bool isInPath { set { _isInPath = value; hasChanged = true; } get { return _isInPath; } }
	public bool isInMoveRange { set { _isInMoveRange = value; hasChanged = true; } get { return _isInMoveRange; } }
	public bool isInSpellRange { set { _isInSpellRange = value; hasChanged = true; } get { return _isInSpellRange; } }

	private new SpriteRenderer renderer;

	private void Start() {
		renderer = GetComponent<SpriteRenderer>();
		try{
			if(target == null)
				target = GetComponentInChildren<TargetScript>();
				//target = transform.GetChild(0).gameObject.GetComponent<TargetScript>();
		} catch {
			// Ignore the error as it's just quality of life for me, not a need for the game
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(hasChanged)
		{
			if(selected)
				renderer.color = new Color32(255,0,0,100);
			else if (isInPath)
				renderer.color = new Color32(0,255,0,100);
			else if (over)
				renderer.color = new Color32(0,0,255,100);
			else if (isInMoveRange)
				renderer.color = new Color32(180,255,180,100);
			else if (isInSpellRange)
				renderer.color = new Color32(255,180,180,100);
			else
				renderer.color = Color.clear;
			hasChanged = false;
		}
	}

	private void OnMouseEnter() {
		if(isInMoveRange)
			CellManager.ShowPath(this);
		over = true;
	}

	private void OnMouseExit() {
		CellManager.ResetGrid(ResetMode.soft);
		over = false;
	}

	private void OnMouseUp() {
		PlayerScript.instance.SelectCell(this);
	}

	public void Select()
	{
		selected = true;
	}

	public void Unselect()
	{
		selected = false;
	}

	public int Distance(CellScript target)
	{
		return Mathf.Abs(x - target.x) + Mathf.Abs(y - target.y);
	}
}