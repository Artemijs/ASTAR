using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Deals with ui of selecting units and returns a list of units in a selection box
/// </summary>
namespace temp
{
	public class UnitSelection : MonoBehaviour
	{
		public List<Transform> _selectedUnits;
		public RectTransform _boxSelect;
		Vector2 _bSlctStart;
		// Start is called before the first frame update
		void Start()
		{
			_bSlctStart = new Vector2(-100, 100);
			_selectedUnits = new List<Transform>();
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
			_selectedUnits.Add(t);
		}
		void DeselectAll()
		{
			for (int i = 0; i < _selectedUnits.Count; i++)
			{
				_selectedUnits[i].GetChild(0).gameObject.SetActive(false);
			}
			_selectedUnits.Clear();
		}
		void HighLightSelection()
		{
			for (int i = 0; i < _selectedUnits.Count; i++)
			{
				_selectedUnits[i].transform.GetChild(0).gameObject.SetActive(true);
			}
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
			GameObject[] allUnits = GameObject.FindGameObjectsWithTag("selectable");
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
			HighLightSelection();
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
					HighLightSelection();
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
			return (_selectedUnits.Count > 0);
		}
		public List<Transform> GetSelectedUnits()
		{
			return _selectedUnits;
		}
	}

}