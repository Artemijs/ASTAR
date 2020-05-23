using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace temp
{
	public class UnitManager : MonoBehaviour
	{
		ActionFactory _afctry;
		//List<Pair<S_Unit, Pair<GameObject, Unit[]>>> _allActionGroups;
		List<Group> _allActionGroups;
		List<Action<int>> _actionCallbacks;
		// Start is called before the first frame update
		void Start()
		{
			_afctry = gameObject.GetComponent<ActionFactory>();
			_allActionGroups = new List<Group>();
			_actionCallbacks = new List<Action<int>>();
			_actionCallbacks.Add(MoveAction);
		}

		// Update is called once per frame
		void Update()
		{
			Debug.Log(_allActionGroups.Count);
			CheckNewGroup();
			UpdateActionGroups();

		}
		public void UpdateActionGroups()
		{
			if (_allActionGroups.Count == 0) return;

			for (int i = 0; i < _allActionGroups.Count; i++)
			{
				_actionCallbacks[(int)(_allActionGroups[i]._id)](i);
			}

		}
		public void CheckNewGroup()
		{
			Group newGroup = _afctry.IsActionRdy();
			if (newGroup == null) return;
			_allActionGroups.Add(newGroup);
			HighlightPath();

		}
		private void HighlightPath()
		{
			//for(int i =0; i < _allActionGroups[_allActionGroups.Count-1].)
			//get the tile for the last group added
			//Tile t = _allActionGroups[_allActionGroups.Count - 1]._paths;
			for (int i = 0; i < _allActionGroups[_allActionGroups.Count - 1]._paths.Count; i++)
			{
				Tile t = _allActionGroups[_allActionGroups.Count - 1]._paths[i];
				while (t != null)
				{
					t.gameObject.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color = Color.green;
					t = t.parent;
				}
			}
			/*for (int i = 0; i < _allActionGroups[_allActionGroups.Count-1]._paths.Count; i++) {
				for (int j = 0; j < _allActionGroups[_allActionGroups.Count-1]._paths[i].Count; j++)
				{
					_allActionGroups[_allActionGroups.Count-1]._paths[i][j].gameObject.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color = Color.green;
				}

			}*/
		}


		/// <summary>
		/// moves units to a target stored in group object
		/// </summary>
		/// <param name="id"> the index of the action group within the group array</param>
		void MoveAction(int id)
		{

			Unit u;
			List<Tile> path;
			Group group = _allActionGroups[id];
			Unit[] units = group._sUnits;
			for (int i = 0; i < units.Length; i++)
			{
				u = units[i];
				//path = group._paths[i];

			}




			//Pair<S_Unit, Pair<GameObject, Unit[]>> group = _allActionGroups[id];
			/*Group group = _allActionGroups[id];
			bool group_good = false;
			for (int i = 0; i < group._sUnits.Length; i++) {
				if (group._sUnits[i] != null) {
					group_good = true;
					if (group._sUnits[i].Move(group._targets[i]))
					{
						group._sUnits[i] = null;
						GameObject.Destroy(group._targets[i].gameObject);
					}
				}
			}
			if (!group_good)
			{
				//removee the group
				_allActionGroups.RemoveAt(id);
			}*/
			/*for (int i = 0; i < group.Second.Second.Length; i++) {
				if (group.Second.Second[i] != null)
				{
					group_good = true;
					if (group.Second.Second[i].Move(group.Second.First.transform.position))
					{
						group.Second.Second[i] = null;
					}
				}
			}
			//means there are only nulls in the group array
			if (!group_good) {
				//removee the group
				_allActionGroups.RemoveAt(id);
			}*/
		}

	}

}