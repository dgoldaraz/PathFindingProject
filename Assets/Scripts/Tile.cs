using UnityEngine;
using System.Collections.Generic;

public class Tile : MonoBehaviour {

	//This class is the basic unit in the grid. It can be of different types and will store different values used for the pathfinding

	public enum TileType { Wall,Floor }

	public Color overColour;
	public Color selectionColor;
	public Color wallColor = Color.blue;
	public GameObject FLabel;
	public GameObject SLabel;
	public GameObject TLabel;
	public Vector3 wallScale;

	public TileType m_unitType;
	private Renderer render;
	private Color m_defaultColor;
	private Color m_currentColor;

	private bool m_selected;
	private int m_xPos;
	private int m_yPos;

	private bool m_useDiagonals;

	private List<Tile> m_neighbours;
	private List<Tile> m_DiagN;



	/*
	private int m_F;
	private int m_H;
	private int m_G;
	*/

	private bool m_showText = true;

	// Use this for initialization
	void Start () {
		m_unitType = TileType.Floor;
		render = GetComponent<Renderer>();
		if(!render)
		{
			Debug.LogError("No Render in unit");
		}
		else
		{
			m_defaultColor = render.material.color;
			updateTile();
		}
		showText(false);
	}

	public void setNormalColour()
	{
		if(!m_selected)
		{
			render.material.color = m_currentColor;
		}
		else
		{
			render.material.color = selectionColor;
		}
	}

	public void setOverColour()
	{
		render.material.color = overColour;
	}
	
	//Set Colr function
	public void setColor( Color c )
	{
		render.material.color = c;
	}

	public void setUseDiagonals(bool d)
	{
		m_useDiagonals = d;
	}

	//Updates the colour depending on the type
	void updateTile()
	{
		switch(m_unitType)
		{
			case TileType.Floor:
				setColor( m_defaultColor);
				break;
			case TileType.Wall:
				setColor(wallColor);
				this.gameObject.transform.localScale += wallScale;
				Vector3 newPos = this.gameObject.transform.position;
				newPos.y += wallScale.y * 0.5f;
				this.gameObject.transform.position = newPos;
				break;
			default:
				setColor(m_defaultColor);
				break;
		}
		m_currentColor = render.material.color;

		if(m_selected)
		{
			setColor(selectionColor);
		}
	}

	//Set the new type
	public void setType(TileType newType)
	{
		if(m_unitType == TileType.Wall)
		{
			this.gameObject.transform.localScale -= wallScale;
			Vector3 newPos = this.gameObject.transform.position;
			newPos.y -= wallScale.y * 0.5f;
			this.gameObject.transform.position = newPos;
		}
		m_unitType = newType;
		updateTile();
	}
	
	//Set selected
	public void setSelected(bool s)
	{
		m_selected = s;
		if(s)
		{
			setColor(selectionColor);
		}
		else
		{
			setColor(m_currentColor);
		}
	}

	//Shows the text over the Tile
	public void showText(bool s)
	{
		m_showText = s;
		FLabel.SetActive(m_showText);
		TLabel.SetActive(m_showText);
		SLabel.SetActive(m_showText);
	}

	//Set/Get
	public void setX(int x)
	{
		m_xPos = x;
	}
	public int getX()
	{
		return m_xPos;
	}
	public void setY(int y)
	{
		m_yPos = y;
	}
	public int getY()
	{
		return m_yPos;
	}
	public TileType getType()
	{
		return m_unitType;
	}

	public void addNeighbour(Tile t)
	{
		if(m_neighbours == null)
		{
			m_neighbours = new List<Tile>();
		}
		if(t != null)
		{
			m_neighbours.Add(t);
		}
	}

	public void addDNeighbour(Tile t)
	{
		if(m_DiagN == null)
		{
			m_DiagN = new List<Tile>();
		}
		if(t != null)
		{
			m_DiagN.Add(t);
		}
	}

	public Tile getNeighbour(int i)
	{
		if( i < m_neighbours.Count)
		{
			return m_neighbours[i];
		}
		else if ( i - m_neighbours.Count < m_DiagN.Count)
		{
			return m_DiagN[i - m_neighbours.Count];

		}
		return null;
	}

	public int getNeighbourCount()
	{
		int count = m_neighbours.Count;
		if(m_useDiagonals)
		{
			count += m_DiagN.Count;
		}
		return count;
	}

	public float distance(Tile t)
	{
		if(t.getType() != TileType.Wall)
		{
			return Vector2.Distance( new Vector2(m_xPos, m_yPos), new Vector2(t.getX(), t.getY ())); 
		}
		return Mathf.Infinity;
	}
	
}
