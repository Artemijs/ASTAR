using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace temp
{

	//i shopuld call thios action factory instead
	/// <summary>
	/// im going to group units by action order such that if a group of units is ordered to move theres an array with just those units
	/// </summary>
	/// 
	public enum S_UNIT_ACTION_ID
	{
		MOVE = 0,
		IDLE,
	}
	public class Group
	{
		public S_UNIT_ACTION_ID _id;
		//public List<List<Tile>> _paths;
		public List<Tile> _paths;
		public Transform[] _targets;
		public Unit[] _sUnits;
		public Group(S_UNIT_ACTION_ID id, Transform[] targets, List<Tile> paths, Unit[] units)
		{
			_id = id;
			_sUnits = units;
			_paths = paths;
			_targets = targets;
		}
	};
	public class ActionFactory : MonoBehaviour
	{

		UnitSelection _usl;
		public GameObject _trgtParent;
		bool _targetUIactive;
		//Pair<S_Unit, Pair<GameObject[], Unit[]>> _newGroup;
		Group _newGroup;
		bool _isActionRdy;
		GridManager _gridMgr;
		void Start()
		{
			_targetUIactive = false;
			_usl = gameObject.GetComponent<UnitSelection>();
			_gridMgr = gameObject.GetComponent<GridManager>();
			_newGroup = null;
			_isActionRdy = false;
		}

		// Update is called once per frame
		void Update()
		{
			if (!_usl.isSelected())
			{
				if (_targetUIactive)
				{
					_targetUIactive = false;
					HideTargetUI();
				}
				return;
			}
			if (Input.GetMouseButtonDown(1))
			{
				_targetUIactive = true;
				MoveAction();
				_isActionRdy = true;
			}
		}
		public void MoveAction()
		{

			Vector3 target = GetMoveTarget();
			if (target.x == 0 && target.y == 0 && target.z == 0) return;

			Unit[] uarr;
			List<Tile> paths;
			Transform[] targets;
			ToUnitArray(_usl.GetSelectedUnits(), out uarr);

			//_newGroup = new Pair<S_Unit, Pair<GameObject, Unit[]>>(S_Unit.MOVE, new Pair<GameObject, Unit[]>(_mvTarget , arr));
			GetTargets(_usl.GetSelectedUnits(), out targets, out paths, target);
			_newGroup = new Group(S_UNIT_ACTION_ID.MOVE, targets, paths, uarr);

		}
		public Vector3 GetMoveTarget()
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit, 1000))
			{
				return hit.point;
			}
			else return new Vector3();
		}
		private void HideTargetUI()
		{
			//_mvTarget.SetActive(false);
			List<Transform> sluts = _usl.GetSelectedUnits();
			for (int i = 0; i < sluts.Count; i++)
			{
				sluts[i].GetChild(1).gameObject.SetActive(false);
			}
		}
		//public Pair<S_Unit, Pair<GameObject, Unit[]>> IsActionRdy()
		public Group IsActionRdy()
		{
			if (_isActionRdy)
			{
				_isActionRdy = false;
				return _newGroup;
			}
			else return null;
		}
		private void ToUnitArray(List<Transform> units, out Unit[] arr)
		{
			arr = new Unit[units.Count];
			for (int i = 0; i < units.Count; i++)
			{
				arr[i] = units[i].gameObject.GetComponent<Unit>();
			}

		}
		/// <summary>
		/// Creates a set of targets where u clicked 
		/// Number of targets are dependant on the number of slected units and
		/// Position of targets is offset by units reletive position to the first unit in the array
		/// </summary>
		/// <param name="sUnits"> selected units 1 or more</param>
		/// <param name="arr"> array where to put the target objects</param>
		/// <param name="trgt">mouse click position </param>
		private void GetTargets(List<Transform> sUnits, out Transform[] targets, out List<Tile> path, Vector3 trgt)
		{
			path = new List<Tile>();
			targets = new Transform[sUnits.Count];
			//_gridMgr.FindPath();
			//arr = new Transform[sUnits.Count];

			for (int i = 0; i < targets.Length; i++)
			{
				List<Tile> onePath = new List<Tile>();

				_gridMgr.FindPath(sUnits[i].position, trgt, ref onePath);
				Debug.Log(onePath.Count);
				path.Add(onePath[0]);//(sUnits[0].position - sUnits[i].position) + trgt
				GameObject tgo = GameObject.Instantiate(
					transform.GetChild(0).GetChild(0).gameObject,
					onePath[onePath.Count - 1].gameObject.transform.position,
					Quaternion.identity, _trgtParent.transform);
				tgo.SetActive(true);
				targets[i] = tgo.transform;
			}
		}
	}

}