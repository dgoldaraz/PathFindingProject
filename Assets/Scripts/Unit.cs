using UnityEngine;
using System.Collections.Generic;

public class Unit : MonoBehaviour {


	private List<Tile> m_path;

	// Use this for initialization
	void Start () 
	{
		InputManager.onMoveUnit += jumpToPosition;
		m_path = new List<Tile>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		followPath();
	}
	//Move the unit instantly to the position
	void jumpToPosition(int x, int y )
	{

		this.gameObject.transform.position = new Vector3( x, this.gameObject.transform.position.y, y);
	}

	//Move the unit follow the Path
	void followPath()
	{
		if(m_path.Count > 0)
		{
			Debug.Log("MOVE");
		}
	}
}
