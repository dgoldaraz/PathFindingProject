using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridCreator : MonoBehaviour {

	//This class creates a grid of WxH objects starting form it's position and using the distance given by the objects
	//Not only creates the objects but stores it.


	public int H = 10;
	public int W = 10;
	public GameObject tile;

	private GameObject[,] m_tiles;

	public GameObject unit;

	private bool m_debugMode = false;


	// Use this for initialization
	void Start () {
		m_tiles = new GameObject[H,W];
		Vector3 pos = this.gameObject.transform.position;
		for(int i  = 0; i < W; ++i)
		{
			for(int j = 0; j < H; ++j)
			{
				GameObject go = Instantiate( tile, new Vector3( pos.x + i, 0, pos.z + j), Quaternion.identity) as GameObject;
				go.GetComponent<Tile>().setX((int)pos.x + i);
				go.GetComponent<Tile>().setY((int)pos.z + j);
				m_tiles[i,j] = go;
			}
		}

		Instantiate(unit, new Vector3( pos.x, 1, pos.z), Quaternion.identity);

		InputManager.onDebugChanged += setDebugMode;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setDebugMode()
	{
		m_debugMode = !m_debugMode;
		foreach( GameObject g in m_tiles)
		{
			g.GetComponent<Tile>().showText(m_debugMode);
		}
	}

}
