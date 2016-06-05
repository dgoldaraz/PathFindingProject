using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class InputManager : MonoBehaviour {

	//This class deals with the selection and the UI oof the app
	private List<GameObject> m_selection;
	private InputManager instance = null;

	public GameObject mainCamera;
	public Toggle goalToggle;
	public Toggle startToggle;
	public Toggle wallToggle;
	public Toggle floorToggle;


	//Singleton
	void Awake () {
		if(instance != null )
		{
			//Destroy duplicate Instances
			Destroy (this.gameObject);
		}
		else
		{
			//Assign the instance and don't destroy
			instance = this;
			GameObject.DontDestroyOnLoad(gameObject);
		}
	}

	// Use this for initialization
	void Start () 
	{
		m_selection = new List<GameObject>();
		deselect();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetMouseButtonDown(0))
		{
			//MultiSelect
			//TODO Allow multiselection
			//if(!Input.GetKeyDown(KeyCode.AltGr))
			//{
				//Only one selection
				//deselect ();
			//}
			selectOneObject();
		}
		else if(Input.GetMouseButtonDown(1))
		{
			//Deselct on right button
			deselect();
		}
	}
	
	//Deselect object
	void deselect()
	{
		foreach(GameObject i in m_selection)
		{
			i.GetComponent<UnitScript>().setSelected(false);
		}
		m_selection.Clear();

		goalToggle.gameObject.SetActive(false);
		startToggle.gameObject.SetActive(false);
		wallToggle.gameObject.SetActive(false);
		floorToggle.gameObject.SetActive(false);
	}
	//Select an object (and deselct if there is another)
	void selectOneObject()
	{
		RaycastHit hit;
		Ray ray = mainCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
		if(Physics.Raycast(ray, out hit))
		{
			//If there is a hit and is a unit, select it
			if(hit.collider.CompareTag("Unit"))
			{
				//Deselect current object
				deselect ();
				GameObject unitGO = hit.collider.gameObject;
				UnitScript unit = unitGO.GetComponent<UnitScript>();
				if(unit)
				{
					m_selection.Add (unitGO);
					unit.setSelected(true);
					goalToggle.gameObject.SetActive(true);
					startToggle.gameObject.SetActive(true);
					wallToggle.gameObject.SetActive(true);
					floorToggle.gameObject.SetActive(true);
					UnitScript.UnitType type = unit.getType();
					switch(type)
					{
						case UnitScript.UnitType.Goal:
						goalToggle.isOn = true;
						break;
						case UnitScript.UnitType.Start:
						startToggle.isOn = true;
						break;
						case UnitScript.UnitType.Wall:
						wallToggle.isOn = true;
						break;
						case UnitScript.UnitType.Floor:
						floorToggle.isOn = true;
						break;
						default:
						break;
					}
				}
			}
		}
	}
	//Call when the toggle Wall changes
	public void toggleChanged(string type)
	{
		if(m_selection.Count > 0)
		{
			if(type == "Wall")
			{
				setToggleOn(wallToggle, UnitScript.UnitType.Wall);
			}
			else if(type == "Start")
			{
				setToggleOn(startToggle, UnitScript.UnitType.Start);
			}
			else if(type == "Goal")
			{
				setToggleOn(goalToggle, UnitScript.UnitType.Goal);
			}
			else if(type == "Floor")
			{
				setToggleOn(floorToggle, UnitScript.UnitType.Floor);
			}
		}
	}

	public void setToggleOn(Toggle toggle, UnitScript.UnitType type)
	{
		if(toggle.gameObject.activeSelf)
		{
			if(toggle.isOn)
			{
				foreach (GameObject g in m_selection)
				{
					g.GetComponent<UnitScript>().setType(type);
				}
			}
		}
	}

}
