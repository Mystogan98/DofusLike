using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Type {
	obstacle, ennemy, ally
}

public abstract class TargetScript : MonoBehaviour {

	protected Type type;

	protected bool _canPlay;

	public bool canPlay { set{ _canPlay = value; } get { return _canPlay; }}

	public Type GetTypeOfTarget()
	{
		return type;
	}
	
}
