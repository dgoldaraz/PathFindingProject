using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

	//This class is the basic unit in the grid. It can be of different types and will store different values used for the pathfinding

	public enum TileType { Wall, Goal, Floor }

	public Color overColour;
	public Color selectionColor;
	public Color wallColor = Color.blue;
	public Color goalColor = Color.red;
	public GameObject FLabel;
	public GameObject SLabel;
	public GameObject TLabel;
	public TileType m_unitType = TileType.Floor;

	private Renderer render;
	private Color m_defaultColor;
	private Color m_currentColor;
	private bool m_selected;
	private int m_xPos;
	private int m_yPos;

	/*
	private int m_F;
	private int m_H;
	private int m_G;
	*/

	private bool m_showText = true;

	// Use this for initialization
	void Start () {
		/*
		m_F = 0;
		m_H = 0;
		m_G = 0;
		*/
		render = GetComponent<Renderer>();
		if(!render)
		{
			Debug.LogError("No Render in unit");
		}
		else
		{
			m_defaultColor = render.material.color;
			updateColour();
		}
		showText(false);
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Do a raycast with the collider and the camera position to decide the clour
		if(!m_selected)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInfo;
			if(collider.Raycast(ray,out hitInfo,Mathf.Infinity))
			{
				render.material.color = overColour;
			}
			else
			{
				render.material.color = m_currentColor;
			}
		}
	}

	//Set Colr function
	public void setColor( Color c )
	{
		render.material.color = c;
	}

	//Updates the colour depending on the type
	void updateColour()
	{
		switch(m_unitType)
		{
			case TileType.Floor:
				setColor( m_defaultColor);
				break;
			case TileType.Goal:
				setColor(goalColor);
				break;
			case TileType.Wall:
				setColor(wallColor);
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
		m_unitType = newType;
		updateColour();
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
	
}
