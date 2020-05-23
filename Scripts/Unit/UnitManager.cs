using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
	UnitSelection _unitSlct;
	GridManager _gridMgr;
	UnitControlUI _unitSlctUI;
	public List<Unit> _allUnits;
	public List<Unit> _selectedUnits;
	
    void Start()
    {
		UIPrefabs uip = GetComponent<UIPrefabs>();
		UnitUI._actionTargetTemplate = uip._actionTargetTemplate;
		_unitSlct = new UnitSelection(ref _selectedUnits, uip._uiRect);
		_unitSlctUI = new UnitControlUI();
		_gridMgr = gameObject.GetComponent<GridManager>();
		GameObject plink = GetComponent<UIPrefabs>()._pathLinkTemplate;
		plink.transform.localScale = new Vector3(_gridMgr.GridSize, 1, _gridMgr.GridSize * 0.5f);
		UnitUI._pathTemplate = uip._pathLinkTemplate;
		UnitUI._pathParent = GameObject.Find("Paths").transform;
		FindUnits();
	}

	void Update()
    {
		HandleUnitSelection();
		HandleAction();
	}
	void HandleAction() {
		if (_selectedUnits.Count == 0) return;
		//target which if targetted on the ground results in a move action by default
		if (Input.GetMouseButtonDown(1)) {
			Pair<int, GameObject> clickedObj = GetClickedObject();
			if (clickedObj == null) return;
			//clicked on a tile
			if (clickedObj.First == 0) {
				//move action
				MoveAction(clickedObj.Second);

			}
			//else if right clicked an enemy unit results in a auto attack
		}
	}
	void MoveAction(GameObject target) {
		List<Vector3> list;
		GetTargets(out list, target.transform.position, _selectedUnits.Count);
		//foreach (Unit u in _selectedUnits) {
		for(int i =0; i < _selectedUnits.Count; i++) {
			Unit u = _selectedUnits[i];
			CancelAction(u);
			Pair<Link, Link> path = _gridMgr.FindPath(u.gameObject.transform.position, list[i]);
			if (path == null) {
				Debug.Log("THERE IS NO PATH");
				return;
			}
			u.SetPath(path.First);
			u.UI.SetActionTarget(path.Second.position);
			u.UI.HighlightPath(path.First);
			u.SetState(S_Unit.MOVE);
		}

	}
	void GetTargets(out List<Vector3> list, Vector3 target, int count) {
		list = new List<Vector3>();
		Node n =_gridMgr.GetNode(target);
		int xSize = (int)Mathf.Ceil(Mathf.Sqrt(count));
		arr_ptr start = _gridMgr.GetNodeAP(target);
		list.Add(n.position);
		for (int i = 1; i < xSize; i++) {
			for (int j = 0; j < xSize; j++) {
				n = _gridMgr.GetNode(new arr_ptr(start.i+i, start.j+j));
				if (n == null) continue;
				list.Add(n.position);
			}
		}
		
	}
	public Pair<int, GameObject> GetClickedObject() {
		//get tile/object clicked
		Pair<int, GameObject> result = null;
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hit, 100000))
		{
			if (hit.transform.gameObject.tag == "Tile")
			{
				result = new Pair<int, GameObject>(0, hit.transform.gameObject);
			}
		}
		return result;
	}
	void HandleUnitSelection() {
		_unitSlct.HandleInput();
		if (_unitSlct.NewSelection)
		{
			_selectedUnits = _unitSlct.GetSelectedUnits();
			_unitSlct.NewSelection = false;
			_unitSlctUI.HighlightSelection(ref _selectedUnits);
		}
		else if (!_unitSlct.isSelected() && _selectedUnits.Count != 0)
		{
			_unitSlctUI.DeselectAll(ref _selectedUnits);
			_selectedUnits.Clear();
		}
	}
	private void FindUnits() {
		_allUnits = new List<Unit>();
		GameObject[] allObjs = GameObject.FindGameObjectsWithTag("Unit");
		foreach (GameObject go in allObjs) {
			_allUnits.Add(go.GetComponent<Unit>());
		}
	}
	public void CancelAction(Unit u) {
		if (u.GetPath() == null) return;
		_unitSlctUI.CancelAction(u);
	}
}
