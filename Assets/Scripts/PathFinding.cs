using UnityEngine;
using System.Collections;

public class PathFinding : MonoBehaviour {

	//This class deals with the Pathfinding algorithms
	public enum pathfindingAlgorithm { Dijkstra, Astar};

	public pathfindingAlgorithm m_algorithm = pathfindingAlgorithm.Dijkstra;
	public GameObject source;

	// Use this for initialization
	void Start () 
	{
		//change path when the target is set
		InputManager.onSetTarget += calculatePath;

	}

	void calculatePath(int targetX, int targetY)
	{
		switch( m_algorithm)
		{
		case pathfindingAlgorithm.Dijkstra:
			calculateDijkstra(targetX, targetY);
			break;
		case pathfindingAlgorithm.Astar:
			calculateAStar(targetX, targetY);
			break;
		}
	}

	void calculateDijkstra(int targetX, int targetY)
	{
		Debug.Log( "Dijkstra! target: " + targetX + "," + targetY);
	}

	void calculateAStar(int targetX, int targetY)
	{
		Debug.Log( "A* target: " + targetX + "," + targetY);
	}
}
