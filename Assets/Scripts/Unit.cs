using UnityEngine;
using System.Collections.Generic;

public class Unit : MonoBehaviour {


	private List<Tile> m_path;

	private int m_xPos;
	private int m_yPos;


	// Use this for initialization
	void Start () 
	{
		InputManager.onMoveUnit += jumpToPosition;
		m_path = new List<Tile>();
		m_xPos = 0;
		m_yPos = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{
		followPath();
	}
	//Move the unit instantly to the position
	void jumpToPosition(int x, int y )
	{
		Vector3 renderPos = GridCreator.TranslateCoordintae(x,y);
		this.gameObject.transform.position = new Vector3( renderPos.x, this.gameObject.transform.position.y, renderPos.z);
		m_xPos = x;
		m_yPos = y;
	}

	//Move the unit follow the Path
	void followPath()
	{
		int currNode = 0;

		while( currNode < m_path.Count - 1)
		{
			Vector3 start = GridCreator.TranslateCoordintae( m_path[currNode].getX(), m_path[currNode].getY());
			Vector3 end = GridCreator.TranslateCoordintae( m_path[currNode+1].getX(), m_path[currNode+1].getY());
			start.y = 1;
			end.y = 1;
			Debug.DrawLine(start, end, Color.red);
			currNode++;
		}
	}

	public int getXPos()
	{
		return m_xPos;
	}

	public int getYPos()
	{
		return m_yPos;
	}

	public void setFollowPath(List<Tile> p)
	{
		m_path = p;
	}
}
