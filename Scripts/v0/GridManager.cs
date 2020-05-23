using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace temp
{
	public class GridManager : MonoBehaviour
	{
		Tile[,] _all_tiles;
		float _time;
		Color[] _all_colors;
		public uint _gridSize;
		int _tileSize;
		public GameObject _ttemplate;
		void Awake()
		{
			_tileSize = (int)(_ttemplate.transform.localScale.x);
			_all_tiles = new Tile[_gridSize, _gridSize];
			GameObject go = GameObject.Find("GridParent");
			for (int i = 0; i < _gridSize; i++)
			{
				for (int j = 0; j < _gridSize; j++)
				{
					GameObject t = GameObject.Instantiate(_ttemplate,
						new Vector3(i * _tileSize, 0, j * _tileSize), Quaternion.identity, go.transform);
					//t.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = i + " " + j;
					_all_tiles[i, j] = t.AddComponent<Tile>() as Tile;
					_all_tiles[i, j].Init();
				}
			}

			_all_colors = new Color[8];
			_all_colors[0] = new Color(1, 1, 1, 1);
			_all_colors[1] = new Color(1, 0, 0, 1);
			_all_colors[2] = new Color(1, 1, 0, 1);
			_all_colors[3] = new Color(1, 0, 1, 1);
			_all_colors[4] = new Color(0, 1, 0, 1);
			_all_colors[5] = new Color(0, 0, 1, 1);
			_all_colors[6] = new Color(0, 1, 0, 1);
			_all_colors[7] = new Color(1, 0.5f, 0.5f, 1);
			SetNeighbours();
		}
		public void SetNeighbours()
		{
			Debug.Log("HELLO WORLD BTDUBS");
			int size = _all_tiles.GetLength(0);
			arr_ptr aptr = new arr_ptr();
			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					//Debug.Log(i + " " + j + " size "+size);
					Tile t = _all_tiles[i, j];

					//top neighbor
					aptr.i = i; aptr.j = j - 1;
					t.SetNeighbor(NIndx.TOP, aptr);

					//top right
					if (i + 1 < size)
					{
						aptr.i = i + 1; aptr.j = j - 1;
						t.SetNeighbor(NIndx.TOPRIGHT, aptr);
					}
					else
					{
						aptr.i = -1; aptr.j = j - 1;
						t.SetNeighbor(NIndx.TOPRIGHT, aptr);
					}
					//right
					if (i + 1 < size)
					{
						aptr.i = i + 1; aptr.j = j;
						t.SetNeighbor(NIndx.RIGHT, aptr);
					}
					else
					{
						aptr.i = -1; aptr.j = j;
						t.SetNeighbor(NIndx.RIGHT, aptr);
					}

					//bot right
					if (i + 1 < size) { aptr.i = i + 1; }
					else { aptr.i = -1; }
					if (j + 1 < size) { aptr.j = j + 1; }
					else { aptr.j = -1; }
					t.SetNeighbor(NIndx.BOTRIGHT, aptr);
					//bot
					if (j + 1 < size)
						aptr.j = j + 1;
					else
						aptr.j = -1;
					aptr.i = i;
					t.SetNeighbor(NIndx.BOT, aptr);
					//bot left
					if (j + 1 < size)
					{
						aptr.j = j + 1;
					}
					else aptr.j = -1;
					aptr.i = i - 1;
					t.SetNeighbor(NIndx.BOTLEFT, aptr);
					//left
					aptr.i = i - 1; aptr.j = j;
					t.SetNeighbor(NIndx.LEFT, aptr);
					//top left
					aptr.i = i - 1; aptr.j = j - 1;
					t.SetNeighbor(NIndx.TOPLEFT, aptr);
				}
			}
		}
		public Tile GetTile(Vector3 pos)
		{

			pos += new Vector3(_tileSize * 0.5f, 0, _tileSize * 0.5f);
			//i*10, j*10
			int i = 0;// (int)(pos.x / _tileSize);
			if (pos.x >= 0 && pos.x < _tileSize * _all_tiles.GetLength(0))
			{
				i = (int)(pos.x / _tileSize);
			}
			else
			{
				i = 0;
			}
			int j = 0;
			if (pos.z >= 0 && pos.z < _tileSize * _all_tiles.GetLength(0))
			{
				j = (int)(pos.z / _tileSize);
			}
			else
			{
				j = 0;
			}

			return _all_tiles[i, j];
		}
		public void Update()
		{
		}
		/*public void TestPath(Vector3 start, Vector3 end, ref  List<Tile> path) {
			_tstr.FindPath(start, end, ref path);
		}*/


		public void FindPath(Vector3 startPos, Vector3 targetPos, ref List<Tile> path)
		{
			//path = new List<Tile>();
			Tile startNode = GetTile(startPos);
			Tile targetNode = GetTile(targetPos);

			List<Tile> openSet = new List<Tile>();
			HashSet<Tile> closedSet = new HashSet<Tile>();
			openSet.Add(startNode);

			while (openSet.Count > 0)
			{
				Tile node = openSet[0];
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
					RetracePath(startNode, targetNode, ref path);
					return;
				}

				for (int i = 0; i < (int)(NIndx.TOPLEFT) + 1; i++)
				{
					arr_ptr ap = node.GetNTile((NIndx)(i));
					//Debug.Log(ap.i + " " + ap.j);
					if (ap.i < 0 || ap.j < 0) continue;
					Tile neighbour = _all_tiles[ap.i, ap.j];

					if (!neighbour._walkable || closedSet.Contains(neighbour))
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
		}

		void RetracePath(Tile startNode, Tile endNode, ref List<Tile> path)
		{
			//path = new List<Tile>();
			Tile currentNode = endNode;

			while (currentNode != startNode)
			{
				path.Add(currentNode);
				//currentNode.gameObject.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color = Color.green;
				currentNode = currentNode.parent;
			}
			path.Add(startNode);
			path.Reverse();
			for (int i = 0; i < path.Count; i++)
			{
				if (i + 1 < path.Count)
					path[i].parent = path[i + 1];
				else
				{
					path[i].parent = null;
					break;
				}
			}
		}

		int GetDistance(Tile nodeA, Tile nodeB)
		{
			int dstX = (int)Mathf.Abs(nodeA.gameObject.transform.position.x - nodeB.transform.position.x);
			int dstY = (int)Mathf.Abs(nodeA.gameObject.transform.position.z - nodeB.transform.position.z);

			if (dstX > dstY)
				return 14 * dstY + 10 * (dstX - dstY);
			return 14 * dstX + 10 * (dstY - dstX);
		}

		// KEEPING THIS FOR DEBUG PURPOSES
		/*int tmp = 0;
		int tmp1 = 0;
		public void Update() {
			_time += Time.deltaTime;
			if (_time > 1.0f) {
				_time = 0;
				Debug.Log(tmp + " , " + tmp1);
				tmp++;
				//tmp1++;
				int size = _all_tiles.GetLength(0);
				if (tmp >= size) {
					tmp = 0;
					tmp1++;
				}
				DrawNext(tmp1, tmp);
			}
			DrawNext(tmp1, tmp);
		}
		void DrawNext(int i , int j) {
			Tile t = _all_tiles[i, j];
			int max = t.GetMaxNS();
			arr_ptr aptr;
			for (int x = 0; x < max; x++) {
				aptr = t.GetNTile((NIndx)(x));
				if (aptr.i == -1 || aptr.j == -1) continue;
				Debug.DrawLine(new Vector3(i * 10, 1, j * 10), _all_tiles[aptr.i, aptr.j].gameObject.transform.position, _all_colors[x]);
			}
		}*/
	}

}