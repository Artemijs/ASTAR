using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace temp
{
	public class Main : MonoBehaviour
	{
		public uint gridSize = 10;
		public GameObject tileTemplate;
		GridManager _gridMgr;
		public bool test = false;
		List<Tile> path;
		// Start is called before the first frame update
		void Awake()
		{
			Debug.Log(gridSize);
			path = new List<Tile>();
			///_gridMgr = new GridManager();
			//_gridMgr.SetNeighbours();
		}
		// Update is called once per frame
		void Update()
		{
			//if (test) Test();
			//_gridMgr.Update();
		}
		/*public GridManager GetGridMgr() {
			return _gridMgr;
		}
		public void Test() {
			test = false;
			//ClearPath();
			//_gridMgr.TestPath(GameObject.Find("A").transform.position, GameObject.Find("B").transform.position, ref path);
			//ColorPath();
		}
		public void ClearPath() {
			foreach (Tile p in path) {
				p.gameObject.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color = Color.white;
			}
			path.Clear();
		}
		public void ColorPath()
		{
			foreach (Tile p in path)
			{
				p.gameObject.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color = Color.red;
			}
		}*/
	}

}