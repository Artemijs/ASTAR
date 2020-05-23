using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitUI
{
	//selection highlight
	GameObject _selectHighlight;
	//action target
	public static GameObject _actionTargetTemplate;
	public static GameObject _pathTemplate;
	public static Transform _pathParent;
	GameObject _actionTarget;
	List<GameObject> _allPathLinks;
	public UnitUI(Transform unit) {
		_selectHighlight = unit.GetChild(0).gameObject;
		_allPathLinks = new List<GameObject>();
	}
	public void SetActionTarget(Vector3 pos)
	{
		//pos.x += _actionTargetTemplate.transform.localScale.x * 0.5f;
		//pos.z -= _actionTargetTemplate.transform.localScale.z * 0.5f;
		pos.y = 0.1f;
		_actionTarget = GameObject.Instantiate(_actionTargetTemplate, pos, Quaternion.identity, GameObject.Find("ActionTargets").transform);
		_actionTarget.SetActive(true);
	}
	public void OnSelect() {
		_selectHighlight.SetActive(true);
	}
	public void OnDeselect()
	{
		_selectHighlight.SetActive(false);
		//OnCancel();
	}
	public void OnCancel()
	{
		GameObject.Destroy(_actionTarget);
		_actionTarget = null;
		DeletePath();
	}
	public void DeletePath( ) {
		foreach (GameObject go in _allPathLinks) {
			GameObject.Destroy(go);
		}
		_allPathLinks.Clear();
	}
	public void DeletePathLink() {
		GameObject.Destroy(_allPathLinks[0]);
		_allPathLinks.RemoveAt(0);
	}
	public void HighlightPath(Link path) {
		Link n = path.Next;
		while (n.Next != null)
		{
			//Transform tile = _gridMgr.GetTile(n.Index);
			//tile.GetChild(0).GetComponent<Renderer>().material.color = color;
			
			GameObject link = GameObject.Instantiate(_pathTemplate, _pathParent);
			link.transform.position = new Vector3(n.position.x, 0.1f, n.position.z);
			_allPathLinks.Add(link);
			Roatate2Next(link, n);
			n = n.Next;
		}
	}
	void Roatate2Next(GameObject pathLink, Link node) {
		if (node.Next == null) return;
		
		NIndx dir = GetNeighbourDir(node.Index, node.Next.Index);
		float yAngle = 0;
		if (dir == NIndx.TOP) yAngle = 90;
		else if (dir == NIndx.TOPRIGHT) yAngle = 135;
		//if (dir == NIndx.RIGHT) { }
		else if(dir == NIndx.BOTRIGHT) yAngle = 45;
		else if(dir == NIndx.BOT) yAngle = 90;
		else if(dir == NIndx.BOTLEFT) yAngle = 135;
		//if (dir == NIndx.LEFT) { }
		else if(dir == NIndx.TOPLEFT) yAngle = 45;
		pathLink.transform.localRotation = Quaternion.Euler(new Vector3(90, yAngle, 0));
	}
	NIndx GetNeighbourDir(arr_ptr t1, arr_ptr t2) {
		NIndx dir = NIndx.BOT;
		if (t2.i > t1.i) {
			if (t2.j > t1.j) dir = NIndx.TOPRIGHT;
			else if(t2.j == t1.j) dir = NIndx.RIGHT;
			else if(t2.j < t1.j) dir = NIndx.BOTRIGHT;
		}
		else if(t2.i < t1.i) {
			if (t2.j > t1.j) dir = NIndx.TOPLEFT;
			else if(t2.j == t1.j) dir = NIndx.LEFT;
			else if(t2.j < t1.j) dir = NIndx.BOTLEFT;
		}
		else if(t2.i == t1.i) {
			if (t2.j > t1.j) dir = NIndx.TOP;
			else if(t2.j < t1.j) dir = NIndx.BOT;
		}
		return dir;
	}
}
