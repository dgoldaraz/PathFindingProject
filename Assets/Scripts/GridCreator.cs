using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridCreator : MonoBehaviour {

	//This class creates a grid of WxH objects starting form it's position and using the distance given by the objects
	//Not only creates the objects but stores it.
	
	public int H = 10;
	public int W = 10;
	public GameObject tile;
	public GameObject unit;
	public bool useDiagonals = false;

	private Tile[,] m_tiles;
	private bool m_debugMode = false;

	private static Vector3 m_currentPosition;

	// Use this for initialization
	void Start () 
	{
		m_currentPosition = this.transform.position;
		m_tiles = new Tile[H,W];
		Vector3 pos = this.gameObject.transform.position;
		for(int i  = 0; i < W; ++i)
		{
			for(int j = 0; j < H; ++j)
			{
				GameObject go = Instantiate( tile, new Vector3( pos.x + i, 0, pos.z + j), Quaternion.identity) as GameObject;
				go.GetComponent<Tile>().setX(i);
				go.GetComponent<Tile>().setY(j);
				m_tiles[i,j] = go.GetComponent<Tile>();
			}
		}

		GameObject uGO = Instantiate(unit, new Vector3( pos.x, 1, pos.z), Quaternion.identity) as GameObject;
		this.gameObject.GetComponent<PathFinding>().sourceUnit = uGO;

		InputManager.onDebugChanged += setDebugMode;
		createGraph();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public Tile[,] getTiles()
	{
		return m_tiles;
	}

	public void setDebugMode()
	{
		m_debugMode = !m_debugMode;
		foreach( Tile t in m_tiles)
		{
			t.showText(m_debugMode);
		}
	}

	public void setUseDiagonals( bool useD)
	{
		for(int i = 0; i < W; ++i)
		{
			for(int j = 0; j < H; ++j)
			{
				Tile cT = m_tiles[i,j];
				cT.setUseDiagonals(useD);
			}
		}
		useDiagonals = useD;
	}

	//Add all the neighbours to the tiles, also use the diagonals if neccesary
	public void createGraph()
	{
		for(int i = 0; i < W; ++i)
		{
			for(int j = 0; j < H; ++j)
			{
				Tile cT = m_tiles[i,j];
				cT.setUseDiagonals(useDiagonals);
				if(i > 0 )
				{
					cT.addNeighbour(m_tiles[i-1,j]);
				}
				if(i < W - 1 )
				{
					cT.addNeighbour(m_tiles[i+1,j]);
				}
				if(j > 0 )
				{
					cT.addNeighbour(m_tiles[i,j-1]);
				}
				if(j < H - 1 )
				{
					cT.addNeighbour(m_tiles[i,j+1]);
				}
				if( i > 0 )
				{
					if(j > 0 )
					{
						cT.addDNeighbour(m_tiles[i-1,j-1]);
					}
					if( j < H-1)
					{
						cT.addDNeighbour(m_tiles[i-1,j+1]);
					}
				}
				if( i < W-1 )
				{
					if(j > 0 )
					{
						cT.addDNeighbour(m_tiles[i+1,j-1]);
					}
					if( j < H-1 )
					{
						cT.addDNeighbour(m_tiles[i+1,j+1]);
					}
				}
			}
		}
	}

	//Return a vector with the Global coordinates based on the position of the grid creator
	public static Vector3 TranslateCoordintae(int x, int y)
	{
		return new Vector3( x + m_currentPosition.x, 0, y + m_currentPosition.z);
	}

}
