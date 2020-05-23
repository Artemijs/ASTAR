using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
	List<KeyCode> _allAbilityKeys;
	int _activeAbility;
	bool _waiting4click;
	public GameObject _icoAbilityCast;
	Unit _selectedTarget;
	UnitManager _uMgr;
	Unit _target;
	UnitSelection _unitSlct;

	void Start()
    {
		_waiting4click = false;
		_allAbilityKeys = new List<KeyCode>();
		_allAbilityKeys.Add(KeyCode.Q);
		_allAbilityKeys.Add(KeyCode.W);
		_allAbilityKeys.Add(KeyCode.E);
		_allAbilityKeys.Add(KeyCode.R);
		_uMgr = GetComponent<UnitManager>();
	}
	void Update()
    {
		if (!_waiting4click)
			for (int i = 0; i < _allAbilityKeys.Count; i++)
			{
				if (Input.GetKeyDown(_allAbilityKeys[i]))
				{
					if (_selectedTarget.GetAbility(i).State == S_Ability.READY) { 
						_activeAbility = i;
						_waiting4click = true;
						_icoAbilityCast.SetActive(true);
					}
				}
			}
		else {
			_icoAbilityCast.transform.position = Input.mousePosition;
			Unit target = GetClickTarget();
			if (target == null) return;//play "u stoopid sound"0
			_selectedTarget.UseAbility(_activeAbility);
			_waiting4click = false;
			_icoAbilityCast.SetActive(false);
		}
    }
	public Unit GetClickTarget() {
		return _uMgr.GetClickedObject().Second.GetComponent<Unit>();
	}
	public Unit SelectedTarget { get => _selectedTarget; set => _selectedTarget = value; }
	public Unit Target { get => _target; set => _target = value; }
}
