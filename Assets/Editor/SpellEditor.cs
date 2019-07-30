using UnityEngine;
using UnityEditor;

class MyWindow : EditorWindow {
	private MonoScript target;
	private int minRange, maxRange;
	private bool onEmpty, onAlly, onEnnemy, onObstacle, onWater, needVision;


    [MenuItem ("Window/My Window")]
    public static void  ShowWindow () {
        EditorWindow.GetWindow(typeof(MyWindow));
    }
    
    public void OnGUI()
    {
		target = (MonoScript) EditorGUILayout.ObjectField("Script :", target, typeof(MonoScript),true);
		minRange = EditorGUILayout.IntField("Min Range :", minRange);

		if(target != null)
		{
			//SpellScript spell = ;
			//Debug.Log(spell + " " + target);
			//minRange = spell.minRange;
			Debug.Log(target.GetType());
		}
 
        //spell.minRange = Vector3.one * size.SizeOfCube;
	}
}