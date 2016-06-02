using UnityEngine;
using System.Collections;

public class GridCreator : MonoBehaviour {

	//This class creates a grid of WxH objects starting form it's position and using the distance given by the objects
	//It's a singleton for accesibility
	//Not only creates the objects but stores it.


	public int H = 4;
	public int W = 4;
	public GameObject basicUnit;

	private GameObject[,] m_units;

	static GridCreator instance = null;



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
	void Start () {
		m_units = new GameObject[H,W];

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
