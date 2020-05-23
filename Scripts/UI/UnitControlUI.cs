using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitControlUI
{
	GridManager _gridMgr;
	DebugScript db;
	public UnitControlUI() {
		db = GameObject.Find("Main").GetComponent<DebugScript>();
		_gridMgr = GameObject.Find("Main").GetComponent<GridManager>();
	}
	public void HighlightSelection(ref List<Unit> selected) {
		for (int i = 0; i < selected.Count; i++)
		{
			selected[i].UI.OnSelect(); 
		}
	}
	/*public void HighlightPath(Link node, Color color) {
		db.ShowParentNulls();
		Link n = node;
		while (n != null) {
			Transform tile = _gridMgr.GetTile(n.Index);
			tile.GetChild(0).GetComponent<Renderer>().material.color = color;
			n = n.Next;
		}
	}*/
	public void DeselectAll(ref List<Unit> selected)
	{
		for (int i = 0; i < selected.Count; i++)
		{
			//HighlightPath(selected[i].GetPath(), Color.white);
			//CancelAction(selected[i]);
			selected[i].UI.OnDeselect();
		}
	}
	public void CancelAction(Unit u) {
		u.UI.OnCancel();
		u.OnCancel();
	}
}
