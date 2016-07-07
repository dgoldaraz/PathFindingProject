using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class PathFinding : MonoBehaviour {

	//This class deals with the Pathfinding algorithms
	public enum pathfindingAlgorithm { Dijkstra, Astar};

	public pathfindingAlgorithm m_algorithm = pathfindingAlgorithm.Dijkstra;
	public GameObject sourceUnit;

	// Use this for initialization
	void Start () 
	{
		//change path when the target is set
		InputManager.onSetTarget += calculatePath;
	}
	// Calculate the next path depending on the algorithm choosen
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
	//set the algorithm type
	public void setAlgorithm(pathfindingAlgorithm type)
	{
		m_algorithm = type;
	}
	//Use Dijkstra algorithm to calculate path
	void calculateDijkstra(int targetX, int targetY)
	{
		Debug.Log( "Dijkstra! target: " + targetX + "," + targetY);

		Tile[,] tiles = this.GetComponent<GridCreator>().getTiles();
		List<Tile> Q = new List<Tile>();

		Dictionary<Tile,float> dist= new Dictionary<Tile,float>();
		Dictionary<Tile,Tile> prev = new Dictionary<Tile,Tile>();

		Tile tSource = tiles[sourceUnit.GetComponent<Unit>().getXPos(), sourceUnit.GetComponent<Unit>().getYPos()];
		Tile tTarget = tiles[targetX, targetY];

		foreach(Tile v in tiles)
		{
			dist[v] = Mathf.Infinity;
			prev[v] = null;
			Q.Add(v);
		}

		dist[tSource] = 0;

		while(Q.Count > 0 )
		{
			//Get Node with less distance
			Tile u = null;
			foreach(Tile cT in Q)
			{
				if( u == null || dist[cT] < dist[u])
				{
					u = cT;
				}
			}
			Q.Remove (u);

			//If is the target, create path
			if(u == tTarget)
			{
				List<Tile> path = new List<Tile>();
				//Create the path and assign to the unit
				while(prev[u] != null)
				{
					path.Add(u);
					u = prev[u];
				}
				path.Reverse();
				sourceUnit.GetComponent<Unit>().setFollowPath(path);
				break;
			}

			for(int i = 0; i < u.getNeighbourCount(); ++i)
			{
				Tile v = u.getNeighbour(i);
				float alt = dist[u] + u.distance(v);
				if(alt < dist[v])
				{
					dist[v] = alt;
					prev[v] = u;
				}
			}
		}

	}
	//Calculate path user A*
	void calculateAStar(int targetX, int targetY)
	{
		Debug.Log( "A* target: " + targetX + "," + targetY);
	}
}
