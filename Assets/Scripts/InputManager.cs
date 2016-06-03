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
			{
				//Only one selection
				deselect ();
			}
			selectObject();
		}
	}

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
	}

	void selectObject()
	{
		RaycastHit hit;
		Ray ray = mainCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
		if(Physics.Raycast(ray, out hit))
		{
			//If there is a hit and is a unit, select it
			if(hit.collider.CompareTag("Unit"))
			{
				GameObject unitGO = hit.collider.gameObject;
				UnitScript unit = unitGO.GetComponent<UnitScript>();
				if(unit)
				{
					m_selection.Add (unitGO);
					unit.setSelected(true);
					goalToggle.gameObject.SetActive(true);
					startToggle.gameObject.SetActive(true);
					wallToggle.gameObject.SetActive(true);
					goalToggle.isOn = false;
					startToggle.isOn = false;
					wallToggle.isOn = false;
					switch(unit.getType())
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
						default:
						break;
					}
				}
			}
		}
		else
		{
			deselect ();
		}
	}
}
