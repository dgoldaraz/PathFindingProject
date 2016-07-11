using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class InputManager : MonoBehaviour {
	
	//This class deals with the selection and the UI oof the app

	public delegate void setDebugSignal();
	public static event setDebugSignal onDebugChanged;

	public delegate void runSimulationSignal(bool run);
	public static event runSimulationSignal onRunSimulation;

	public delegate void moveUnitSignal(int x, int y);
	public static event moveUnitSignal onMoveUnit;

	public delegate void setTarget(int x, int y);
	public static event setTarget onSetTarget;


	private List<GameObject> m_selection;
	private InputManager instance = null;
	private bool m_isRunning = false;

	public GameObject mainCameraTranasform;

	public Toggle wallToggle;
	public Toggle floorToggle;
	public Toggle DijkstraToggle;
	public Toggle AStarToggle;
	public Button runButton;

	private Vector3 m_initCamPos;
	private Vector3 m_initialCamRot;

	private GameObject m_mouseOverTile;

	//Singleton implmentation
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
		m_initCamPos = mainCameraTranasform.transform.position;
		m_initialCamRot = mainCameraTranasform.transform.localEulerAngles;
		m_mouseOverTile = null;
	}
	
	// Update is called once per frame, check if we are running or not
	// If we are running, the main click selects a position to go and we should start the simulation
	// I fwe are not running, we are creating the environment, allow selection of Tiles, Unit
	void Update () 
	{
		if(m_isRunning)
		{
			//setelct path
			selectTarget();
		}
		else
		{
			creationSelection();
			if(Input.GetKeyDown(KeyCode.O))
			{
				changeCamera();
			}
		}

	}

	void changeCamera()
	{
		Camera cam = mainCameraTranasform.GetComponent<Camera>();
		if(cam.orthographic)
		{
			//change to perspective. Move camera
			mainCameraTranasform.transform.position = m_initCamPos;
			mainCameraTranasform.transform.localEulerAngles = m_initialCamRot;
			cam.orthographic = false;
		}
		else
		{
			mainCameraTranasform.transform.localEulerAngles = new Vector3(90.0f, 0.0f, 0.0f);
			mainCameraTranasform.transform.position = new Vector3(0.0f, 5.0f, 2.5f);
			cam.orthographic = true;
		}
	}
	
	//Calculates the target position and send a signal
	void selectTarget()
	{
		if(Input.GetMouseButtonDown(0))
		{
			RaycastHit hit;
			Ray ray = mainCameraTranasform.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(ray, out hit))
			{
				//If there is a hit and is a unit, select it
				if(hit.collider.CompareTag("Tile"))
				{
					GameObject unitGO = hit.collider.gameObject;
					Tile unit = unitGO.GetComponent<Tile>();
					if(onSetTarget != null)
					{
						onSetTarget(unit.getX(), unit.getY ());
					}
				}
			}
		}
	}
	// Call when we are going to change the environment 
	// Allow to select a Tile or Move the Unit
	void creationSelection()
	{
		if(Input.GetMouseButtonDown(0))
		{
			//MultiSelect
			if(Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
			{
				//Only one selection
				selectObject(false);
			}
			else
			{
				selectObject(true);
			}

		}
		else if(Input.GetMouseButtonDown(1))
		{
			//Deselct on right button if we don't hit any object
			checkMoveUnit();
			deselect();
		}
		else
		{
			RaycastHit hit;
			Ray ray = mainCameraTranasform.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(ray, out hit))
			{
				if(hit.collider.CompareTag("Tile") && hit.collider.gameObject != m_mouseOverTile)
				{
					setMouseOver(hit.collider.gameObject);
				}
			}
		}
		
		if(Input.GetKeyDown(KeyCode.D))
		{
			if(onDebugChanged != null)
			{
				onDebugChanged();
			}
		}
	}

	//Deselect Tile
	void deselect()
	{
		foreach(GameObject i in m_selection)
		{
			i.GetComponent<Tile>().setSelected(false);
		}
		m_selection.Clear();

		wallToggle.gameObject.SetActive(false);
		floorToggle.gameObject.SetActive(false);
	}

	//Select an Tile (and deselct if there is another)
	//Updates the Toggles with the types of the Tile selected (if any)
	void selectObject(bool onlyOne)
	{
		RaycastHit hit;
		Ray ray = mainCameraTranasform.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
		if(Physics.Raycast(ray, out hit))
		{
			//If there is a hit and is a unit, select it
			if(hit.collider.CompareTag("Tile"))
			{
				//Deselect current object
				if(onlyOne)
				{
					deselect ();
				}
				GameObject unitGO = hit.collider.gameObject;
				Tile unit = unitGO.GetComponent<Tile>();
				if(unit)
				{
					m_selection.Add (unitGO);
					unit.setSelected(true);
					wallToggle.gameObject.SetActive(true);
					floorToggle.gameObject.SetActive(true);
					Tile.TileType type = unit.getType();
					switch(type)
					{
						case Tile.TileType.Wall:
						wallToggle.isOn = true;
						break;
						case Tile.TileType.Floor:
						floorToggle.isOn = true;
						break;
						default:
						break;
					}
				}
				m_mouseOverTile = null;
			}
		}
	}

	//Call when the toggle changes depending on type
	public void toggleChanged(string type)
	{
		if(m_selection.Count > 0)
		{
			if(type == "Wall")
			{
				setToggleOn(wallToggle, Tile.TileType.Wall);
			}
			else if(type == "Floor")
			{
				setToggleOn(floorToggle, Tile.TileType.Floor);
			}
		}
	}

	//Call when the toggle changes depending on type
	public void toggleAlgorithmChanged(string type)
	{
		if(type == "Dijkstra")
		{
			if(DijkstraToggle.isOn)
			{
				setAlgorithm(PathFinding.pathfindingAlgorithm.Dijkstra);
			}
		}
		else if(type == "AStar")
		{
			if(AStarToggle.isOn)
			{
				setAlgorithm(PathFinding.pathfindingAlgorithm.Astar);
			}
		}
	}

	public void setDiagonals()
	{
		GridCreator gc = GameObject.FindObjectOfType<GridCreator>();
		if(gc)
		{
			gc.setUseDiagonals(!gc.useDiagonals);
		}
	}

	void setAlgorithm(PathFinding.pathfindingAlgorithm type)
	{
		PathFinding pf = GameObject.FindObjectOfType<PathFinding>();
		if(pf)
		{
			pf.setAlgorithm(type);
		}
	}

	//Updates the selected Tile with the toggle type
    void setToggleOn(Toggle toggle, Tile.TileType type)
	{
		if(toggle.gameObject.activeSelf)
		{
			if(toggle.isOn)
			{
				foreach (GameObject g in m_selection)
				{
					g.GetComponent<Tile>().setType(type);
				}
			}
		}
	}

	//Deals with the Run button simulation
	public void runButtonPressed()
	{
		//Get text to know the state
		if(runButton.GetComponentInChildren<Text>().text == "RUN")
		{
			deselect();
			//We are running!
			m_isRunning = true;
			//sent signal to run
			if(onRunSimulation != null)
			{
				onRunSimulation(true);
			}
			runButton.GetComponentInChildren<Text>().text = "STOP";
		}
		else
		{
			m_isRunning = false;
			//Stop the simulation
			if(onRunSimulation != null)
			{
				onRunSimulation(false);
			}
			runButton.GetComponentInChildren<Text>().text = "RUN";
		}
	}

	public void cameraButtonPressed()
	{
		changeCamera();
	}

	//Checks the user wants to move the Unit to a specific position
	void checkMoveUnit()
	{
		RaycastHit hit;
		Ray ray = mainCameraTranasform.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
		if(Physics.Raycast(ray, out hit))
		{
			//If there is a hit and is a unit, select it
			if(hit.collider.CompareTag("Tile"))
			{
				//Move Unit to this Tile
				if(onMoveUnit != null)
				{
					GameObject unitGO = hit.collider.gameObject;
					Tile unit = unitGO.GetComponent<Tile>();

					onMoveUnit(unit.getX(), unit.getY());
				}
			}
		}
	}

	public void setMouseOver(GameObject gg)
	{

		if(m_mouseOverTile)
		{
			m_mouseOverTile.GetComponent<Tile>().setNormalColour();
		}

		m_mouseOverTile = gg;
		m_mouseOverTile.GetComponent<Tile>().setOverColour();
	}
	
}
