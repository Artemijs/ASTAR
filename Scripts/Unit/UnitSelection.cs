using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UnitSelection {
	List<Unit> _selectedUnits;
	RectTransform _boxSelect;
	Vector2 _bSlctStart;
	//UnitControlUI _ui;
	bool _newSelection;
	bool _isSelected;
	public UnitSelection(ref List<Unit> slectedList, RectTransform rect) {
		_bSlctStart = new Vector2(-100, 100);
		_boxSelect = rect;
		_selectedUnits = slectedList;
		_isSelected = false;
		_newSelection = false;
	}
	public void HandleInput()
	{

		//box select
		if (Input.GetMouseButtonDown(0))
		{
			if (_bSlctStart.x == -100)
				_bSlctStart = Input.mousePosition;
			else UpdateSelectionBox(Input.mousePosition);
		}
		if (Input.GetMouseButtonUp(0))
		{
			if (_bSlctStart.x == Input.mousePosition.x && _bSlctStart.y == Input.mousePosition.y)
			{
				ClickSelect();
			}
			else
			{
				int nrFound = ReleaseSelectionBox();
				if (nrFound == 0)
					DeselectAll();
			}
			_bSlctStart.x = -100;
		}
		if (Input.GetMouseButton(0))
		{
			UpdateSelectionBox(Input.mousePosition);
		}
	}
	void Add2Selected(Transform t)
	{
		_selectedUnits.Add(t.gameObject.GetComponent<Unit>());
		_isSelected = true;
		_newSelection = true;
	}
	void DeselectAll()
	{
		_isSelected = false;
	}
	private void UpdateSelectionBox(Vector2 mpos)
	{
		if (!_boxSelect.gameObject.activeInHierarchy)
			_boxSelect.gameObject.SetActive(true);
		float width = _bSlctStart.x - mpos.x;
		float height = _bSlctStart.y - mpos.y;
		_boxSelect.anchoredPosition = new Vector2(_bSlctStart.x - width * 0.5f, _bSlctStart.y - height * 0.5f);
		_boxSelect.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
	}
	/// <summary>
	/// returns how many units were found to be within the selection
	/// and uts them in array
	/// </summary>
	/// <returns></returns>
	int ReleaseSelectionBox()
	{
		_boxSelect.gameObject.SetActive(false);
		//find screen rect
		Vector2 min = _boxSelect.anchoredPosition - (_boxSelect.sizeDelta / 2);
		Vector2 max = _boxSelect.anchoredPosition + (_boxSelect.sizeDelta / 2);
		GameObject[] allUnits = GameObject.FindGameObjectsWithTag("Unit");
		int count = 0;
		foreach (GameObject go in allUnits)
		{
			Transform t = go.transform;
			Vector3 spos = Camera.main.WorldToScreenPoint(t.position);
			if (spos.x >= min.x && spos.x <= max.x && spos.y >= min.y && spos.y <= max.y)
			{
				Add2Selected(t);
				count++;
			}
		}
		return count;
	}
	void ClickSelect()
	{
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hit, 1000))
		{

			if (hit.transform.gameObject.layer == 9)
			{
				Add2Selected(hit.transform);
			}
			else DeselectAll();
		}
		else
		{
			DeselectAll();
		}
	}

	public bool isSelected()
	{
		return (_isSelected);
	}
	public List<Unit> GetSelectedUnits()
	{
		return _selectedUnits;
	}
	public bool NewSelection
	{
		get { return _newSelection; }
		set { _newSelection = value; }
	}

}
/*
public class UnitSelection : MonoBehaviour
{
	public List<Unit> _selectedUnits;
	public RectTransform _boxSelect;
	Vector2 _bSlctStart;
	//UnitControlUI _ui;
	bool _newSelection;
	bool _isSelected;
	// Start is called before the first frame update
	void Start()
	{
		_bSlctStart = new Vector2(-100, 100);
		_selectedUnits = new List<Unit>();
		_isSelected = false;
		_newSelection = false;
	}
	public void SetUnitControlUI(ref UnitControlUI ui) {
		//_ui = ui;
	}
	// Update is called once per frame
	void Update()
	{
		HandleInput();
	}
	void HandleInput()
	{

		//box select
		if (Input.GetMouseButtonDown(0))
		{
			if (_bSlctStart.x == -100)
				_bSlctStart = Input.mousePosition;
			else UpdateSelectionBox(Input.mousePosition);
		}
		if (Input.GetMouseButtonUp(0))
		{
			if (_bSlctStart.x == Input.mousePosition.x && _bSlctStart.y == Input.mousePosition.y)
			{
				ClickSelect();
			}
			else
			{
				int nrFound = ReleaseSelectionBox();
				if (nrFound == 0)
					DeselectAll();
			}
			_bSlctStart.x = -100;
		}
		if (Input.GetMouseButton(0))
		{
			UpdateSelectionBox(Input.mousePosition);
		}
	}
	void Add2Selected(Transform t)
	{
		_selectedUnits.Add(t.gameObject.GetComponent<Unit>());
		_isSelected = true;
		_newSelection = true;
	}
	void DeselectAll() {
		//_ui.DeselectAll(ref _selectedUnits);
		_isSelected = false;
		//_selectedUnits.Clear();
	}
	private void UpdateSelectionBox(Vector2 mpos)
	{
		if (!_boxSelect.gameObject.activeInHierarchy)
			_boxSelect.gameObject.SetActive(true);
		float width = _bSlctStart.x - mpos.x;
		float height = _bSlctStart.y - mpos.y;
		_boxSelect.anchoredPosition = new Vector2(_bSlctStart.x - width * 0.5f, _bSlctStart.y - height * 0.5f);
		_boxSelect.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
	}
	/// <summary>
	/// returns how many units were found to be within the selection
	/// and uts them in array
	/// </summary>
	/// <returns></returns>
	int ReleaseSelectionBox()
	{
		_boxSelect.gameObject.SetActive(false);
		//find screen rect
		Vector2 min = _boxSelect.anchoredPosition - (_boxSelect.sizeDelta / 2);
		Vector2 max = _boxSelect.anchoredPosition + (_boxSelect.sizeDelta / 2);
		GameObject[] allUnits = GameObject.FindGameObjectsWithTag("Unit");
		int count = 0;
		foreach (GameObject go in allUnits)
		{
			Transform t = go.transform;
			Vector3 spos = Camera.main.WorldToScreenPoint(t.position);
			if (spos.x >= min.x && spos.x <= max.x && spos.y >= min.y && spos.y <= max.y)
			{
				Add2Selected(t);
				count++;
			}
		}
		//_ui.HighlightSelection(ref _selectedUnits);
		return count;
	}
	void ClickSelect()
	{
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hit, 1000))
		{

			if (hit.transform.gameObject.layer == 9)
			{
				Add2Selected(hit.transform);
				//_ui.HighlightSelection(ref _selectedUnits);
			}
			else DeselectAll();
		}
		else
		{
			DeselectAll();
		}
	}

	public bool isSelected()
	{
		return (_isSelected);
	}
	public List<Unit> GetSelectedUnits()
	{
		return _selectedUnits;
	}
	public bool NewSelection{
		get{ return _newSelection; }
		set { _newSelection = value; }
	}
}
*/
