using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum NIndx
{
	TOP = 0,
	TOPRIGHT,
	RIGHT,
	BOTRIGHT,
	BOT,
	BOTLEFT,
	LEFT,
	TOPLEFT
};
public struct arr_ptr
{
	public int i;
	public int j;
	public arr_ptr(int iv, int jv) { i = iv; j = jv; }
};
public class GridManager : MonoBehaviour
{
	public GameObject _tileTemplate;
	public uint _gridSize;
	uint _tileSize;
	Transform[,] _allTiles;
	Node[,] _allNodes;
    void Awake()
    {
		_allTiles = new Transform[_gridSize, _gridSize];
		_tileSize = (uint)(_tileTemplate.transform.localScale.x);
		GameObject gridParent = GameObject.Find("GridParent");
		_allNodes = new Node[_gridSize, _gridSize];
		for (int i = 0; i < _gridSize; i++) {
			for (int j = 0; j < _gridSize; j++)
			{
				GameObject t = GameObject.Instantiate(_tileTemplate,
						new Vector3(i * _tileSize, 0, j * _tileSize), Quaternion.identity, gridParent.transform);
				_allTiles[i, j] = t.transform;
				_allNodes[i, j] = new Node(new Vector3(t.transform.position.x, 6, t.transform.position.z), new arr_ptr(i,j));

				t.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = i + " " + j;
			}
		}
	}

	
    void Update()
    {
        
    }
	public Node GetNeighbour(NIndx index, Vector3 myPos) {
		arr_ptr pos = GetNodeAP(myPos);
		int size = _allNodes.GetLength(0);
		if (index == NIndx.TOP) {
			pos.j += 1;
			if (pos.j >= size) {
				pos.j = -1;
			}
		}
		else if (index == NIndx.TOPRIGHT)
		{
			pos.j += 1;
			pos.i += 1;
			if (pos.j >= size || pos.i >= size)
			{
				pos.j = -1;
				pos.i = -1;
			}
		}
		else if(index == NIndx.RIGHT)
		{
			pos.i += 1;
			if (pos.i >= size)
			{
				pos.i = -1;
			}
		}
		else if(index == NIndx.BOTRIGHT)
		{
			pos.i += 1;
			pos.j -= 1;
			if (pos.j < 0 || pos.i >= size)
			{
				pos.j = -1;
				pos.i = -1;
			}
		}
		else if(index == NIndx.BOT) {
			pos.j -= 1;
			if (pos.j < 0 )
			{
				pos.j = -1;
			}
		}
		else if(index == NIndx.BOTLEFT) {
			pos.i -= 1;
			pos.j -= 1;
			if (pos.j < 0 || pos.i < 0)
			{
				pos.j = -1;
				pos.i = -1;
			}
		}
		else if(index == NIndx.LEFT) {
			pos.i -= 1;
			if (pos.i < 0)
			{
				pos.i = -1;
			}
		}
		else if(index == NIndx.TOPLEFT) {
			pos.i -= 1;
			pos.j += 1;
			if (pos.j >= size || pos.i < 0)
			{
				pos.j = -1;
				pos.i = -1;
			}
		}

		if (pos.j == -1 || pos.i == -1) return null;
		return _allNodes[pos.i, pos.j];
	}
	public Node GetNode(Vector3 pos)
	{
		pos += new Vector3(_tileSize * 0.5f, 0, _tileSize * 0.5f);
		//i*10, j*10
		int i = 0;
		if (pos.x >= 0 && pos.x < _tileSize * _allNodes.GetLength(0))
		{
			i = (int)(pos.x / _tileSize);
		}
		else
		{
			i = 0;
		}
		int j = 0;
		if (pos.z >= 0 && pos.z < _tileSize * _allNodes.GetLength(0))
		{
			j = (int)(pos.z / _tileSize);
		}
		else
		{
			j = 0;
		}

		return _allNodes[i, j];
	}
	public Transform GetTile( arr_ptr ptr) {
		return _allTiles[ptr.i, ptr.j];
	}
	public Pair<Link, Link> FindPath(Vector3 startPos, Vector3 targetPos)
	{
		Link startPath = new Link();
		Link endPath = new Link();
		Node startNode = GetNode(startPos);
		Node targetNode = GetNode(targetPos);

		List<Node> openSet = new List<Node>();
		HashSet<Node> closedSet = new HashSet<Node>();
		openSet.Add(startNode);

		while (openSet.Count > 0)
		{
			Node node = openSet[0];
			for (int i = 1; i < openSet.Count; i++)
			{
				if (openSet[i].fCost < node.fCost || openSet[i].fCost == node.fCost)
				{
					if (openSet[i].hCost < node.hCost)
						node = openSet[i];
				}
			}

			openSet.Remove(node);
			closedSet.Add(node);

			if (node == targetNode)
			{
				RetracePath(startNode, targetNode, ref startPath, ref endPath);
				break;
			}

			for (int i = 0; i < (int)(NIndx.TOPLEFT) + 1; i++)
			{
				Node neighbour = GetNeighbour((NIndx)(i), node.position);
				if (neighbour == null) continue;
				if (!neighbour.free || closedSet.Contains(neighbour))
				{
					continue;
				}
				int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);
				if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
				{
					neighbour.gCost = newCostToNeighbour;
					neighbour.hCost = GetDistance(neighbour, targetNode);
					neighbour.parent = node;

					if (!openSet.Contains(neighbour))
						openSet.Add(neighbour);
				}
			}
		}
		if (startPath.Next == null)
			return null;
		return new Pair<Link, Link>(startPath, endPath);
	}

	void RetracePath(Node startNode, Node endNode, ref Link path, ref Link endPath)
	{
		//path = new List<Tile>();
		Node currentNode = endNode;
		Link p = new Link(endNode.position, endNode.Index, null);//last link
		endPath = p;
		while (currentNode != startNode)
		{
			currentNode = currentNode.parent;
			Link p2 = new Link(currentNode.position, currentNode.Index, p);//last link
			p = p2;
		}
		path = p;
	}

	int GetDistance(Node nodeA, Node nodeB)
	{
		int dstX = (int)Mathf.Abs(nodeA.position.x - nodeB.position.x);
		int dstY = (int)Mathf.Abs(nodeA.position.z - nodeB.position.z);

		if (dstX > dstY)
			return 14 * dstY + 10 * (dstX - dstY);
		return 14 * dstX + 10 * (dstY - dstX);
	}
	public arr_ptr GetNodeAP(Vector3 pos)
	{
		pos += new Vector3(_tileSize * 0.5f, 0, _tileSize * 0.5f);
		//i*10, j*10
		int i = 0;
		if (pos.x >= 0 && pos.x < _tileSize * _allNodes.GetLength(0))
		{
			i = (int)(pos.x / _tileSize);
		}
		else
		{
			i = 0;
		}
		int j = 0;
		if (pos.z >= 0 && pos.z < _tileSize * _allNodes.GetLength(0))
		{
			j = (int)(pos.z / _tileSize);
		}
		else
		{
			j = 0;
		}

		return new arr_ptr(i, j);
	}
	public Node GetNode(arr_ptr ptr) {
		return _allNodes[ptr.i, ptr.j];
	}
	public uint GridSize {
		get { return _gridSize; }
	}
}
