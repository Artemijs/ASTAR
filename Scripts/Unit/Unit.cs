using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//GONNA NEED A STAT FACTORY or A UNIT FACTORY

public enum S_Unit {
	IDLE = 0,
	MOVE
};
public class Unit : MonoBehaviour
{
	S_Unit _state;
	Stats _stats;
	UnitUI _unitUI;
	//path
	Link _path;
	//tile index
	arr_ptr _tile;
	//list of status effects buffs/debuffs
	List<StatusEffect> _allStatEffects;
	//all abilities
	List<Ability> _allAbilities;
	//Start is called before the first frame update
	void Start() {
		Init();
	}
	public void Init() {
		_state = S_Unit.IDLE;
		_stats = new Stats();
		//_stats._speed = 10;
		_allAbilities = new List<Ability>();
		_allStatEffects = new List<StatusEffect>();
		_path = null;
		_unitUI = new UnitUI(transform);
		_tile = GameObject.Find("Main").GetComponent<GridManager>().GetNodeAP(transform.position);
	}
    void Update() {
		UpdateStatusEffects();
		UpdateAbilities();
		if (_state == S_Unit.IDLE) return;
		else if (_state == S_Unit.MOVE) Move();
    }
	public void Move()
	{
		//move from path to path.next
		transform.position = Vector3.MoveTowards(transform.position, _path.Next.position, Time.deltaTime * GetStat(StatIndex.MOVE_SPEED));
		float distance = Vector3.Distance(_path.Next.position, transform.position);

		//if arrived at path . next
		if (distance < 0.5) {
			Link temp = _path;
			_path = null;
			_path = temp.Next;
			//if arrived at the end
			if (_path.Next == null)
			{
				transform.position = _path.position;
				_path = null;
				SetState(S_Unit.IDLE);
				_unitUI.OnCancel();
				return;
			}
			_unitUI.DeletePathLink();
		}
	}
	public void SetPath(Link n) {
		_path = n;
	}
	public Link GetPath() {
		return _path;
	}
	public void OnCancel() {
		SetState(S_Unit.IDLE);
		//clear path
		while (_path != null) {
			Link n = _path.Next;
			_path.Next = null;
			_path = n;
		}
	}

	public void SetState(S_Unit newState) { _state = newState; }
	public S_Unit GetState() { return _state; }
	public UnitUI UI {
		get { return _unitUI; }
		set { _unitUI = value; }
	}
	void UpdateAbilities() {
		foreach (Ability a in _allAbilities) {
			a.Update();
		}
	}
	void UpdateStatusEffects() {
		foreach (StatusEffect sf in _allStatEffects)
		{
			sf.Update();
		}
	}
	float GetStat(StatIndex si) {
		return _stats.GetStat(si) + GetStatMod(si);
	}
	float GetStatMod(StatIndex si) {
		float total = 0;
		foreach (StatusEffect sf in _allStatEffects)
		{
			if (sf.Index == si)
				total += sf.GetValue();
		}
		return total;
	}
	public void SetStats(Stats s) {
		_stats = s;
	}
	public void AddAbility(Ability a) {
		_allAbilities.Add(a);
	}
	public void UseAbility(int abId) {
		_allAbilities[abId].BeginCasting( null );
	}
	public Ability GetAbility(int id) {
		return _allAbilities[id];
	}
}
