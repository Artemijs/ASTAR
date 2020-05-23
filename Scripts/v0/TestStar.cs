using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using temp;
namespace DEBUG
{
	public class TestStar
	{
		/// <summary>
		/// THIS IS A DEBUG FILE ONLY
		/// </summary>
		/// <param name="gmgr"></param>
		/// 
		Tile[,] _all_tiles;
		public TestStar(ref Tile[,] tts)
		{
			_all_tiles = tts;

		}
		public void Update()
		{

			if (Input.GetButtonDown("Fire1"))
			{
				MouseDown();
			}
		}
		void MouseDown()
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit, 1000))
			{
				Debug.Log(hit.point);
				Tile t = GetTile(hit.point);
				t.transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_Color", Color.black);
				t._walkable = false;
			}
			else
			{
				Debug.Log(" NO HIT DETECTED ");
			}
		}
		public Tile GetTile(Vector3 pos)
		{
			pos += new Vector3(5, 0, 5);
			int i = 0;

			i = (int)(pos.x / 10);
			int j = 0;
			j = (int)(pos.z / 10);

			return _all_tiles[i, j];
		}

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
					temp.arr_ptr ap = node.GetNTile((temp.NIndx)(i));
					Debug.Log(ap.i + " " + ap.j);
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
		}

		int GetDistance(Tile nodeA, Tile nodeB)
		{
			int dstX = (int)Mathf.Abs(nodeA.gameObject.transform.position.x - nodeB.transform.position.x);
			int dstY = (int)Mathf.Abs(nodeA.gameObject.transform.position.z - nodeB.transform.position.z);

			if (dstX > dstY)
				return 14 * dstY + 10 * (dstX - dstY);
			return 14 * dstX + 10 * (dstY - dstX);
		}

	}
}
