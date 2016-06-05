using UnityEngine;
using System.Collections;

public class UnitScript : MonoBehaviour {

	//This class is the basic unit in the grid. It can be of different types and will store different values used for the pathfinding

	public enum UnitType { Wall, Goal, Start, Floor }

	public Color overColour;
	public Color selectionColor;
	public Color wallColor = Color.blue;
	public Color goalColor = Color.red;
	public Color startColor = Color.green;
	public GameObject HText;
	public GameObject GText;
	public GameObject FText;
	public UnitType m_unitType = UnitType.Floor;

	private Renderer render;
	private Color m_defaultColor;
	private Color m_currentColor;
	private bool m_selected;
	private int xPos{ get; set; }
	private int yPos{ get; set; }
	private int m_F { get; set; }
	private int m_H { get; set; }
	private int m_G { get; set; }
	private bool m_showText = true;

	// Use this for initialization
	void Start () {

		m_F = 0;
		m_H = 0;
		m_G = 0;
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
			case UnitType.Floor:
				setColor( m_defaultColor);
				break;
			case UnitType.Goal:
				setColor(goalColor);
				break;
			case UnitType.Start:
				setColor(startColor);
				break;
			case UnitType.Wall:
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
	public void setType(UnitType newType)
	{
		m_unitType = newType;
		updateColour();
	}
	public UnitType getType()
	{
		return m_unitType;
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

	public void showText(bool s)
	{
		m_showText = s;
		HText.SetActive(m_showText);
		FText.SetActive(m_showText);
		GText.SetActive(m_showText);
	}
	
}
