using UnityEngine;
using System.Collections.Generic;

public class Unit : MonoBehaviour {


	public float speed = 0.5f;

	private List<Tile> m_path;
	private int m_xPos;
	private int m_yPos;
	private Vector3 m_nextPosition;
	private bool m_moving;


	// Use this for initialization
	void Start () 
	{
		InputManager.onMoveUnit += jumpToPosition;
		m_path = new List<Tile>();
		m_xPos = 0;
		m_yPos = 0;
		m_moving = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		followPath();
		moveToNextPosition();
	}
	void moveToNextPosition()
	{
		if(m_moving)
		{
			//Check if we are in the right posiiton
			if(transform.position == m_nextPosition)
			{
				m_moving = false;
			}
			else
			{
				//Update values and position
				transform.position = Vector3.Lerp(transform.position, m_nextPosition, speed);
			}
		}
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
		if(!m_moving && m_path.Count > 0)
		{
			//int currNode = 0;
			Tile nextTile = m_path[0];
			m_path.RemoveAt(0);
			Vector3 nP = GridCreator.TranslateCoordintae( nextTile.getX(), nextTile.getY());
			
			m_nextPosition.x = nP.x;
			m_nextPosition.y = this.gameObject.transform.position.y;
			m_nextPosition.z = nP.z;
			m_moving = true;
			m_xPos = nextTile.getX();
			m_yPos = nextTile.getY();
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
