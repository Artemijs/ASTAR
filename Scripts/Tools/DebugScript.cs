using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class DebugScript : MonoBehaviour
{
	GridManager _grid;
	void Start()
    {
		_grid = gameObject.GetComponent<GridManager>();
	}

    void Update()
    {
		
    }
	public void ShowParentNulls() {
		for (int i = 0; i < _grid.GridSize; i++) {
			for (int j = 0; j < _grid.GridSize; j++)
			{
				arr_ptr ptr = new arr_ptr(i, j);
				Transform t = _grid.GetTile(ptr);
				Node n = _grid.GetNode(ptr);
				if (n.Next == null)
					t.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = "null";
				else {
					t.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = "obj";
				}
			}
		}
	}
}
