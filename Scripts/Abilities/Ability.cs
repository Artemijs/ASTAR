using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public enum S_Ability {
	READY = 0,
	CASTING,
	EXECUTING,
	COOLDOWN
};

public struct AbilityData {
	public float _manaCost;
	public float _cooldown;
	public float _castTime;
	public float _hpDisplacement;
	public bool _selfCast;

};

public class Ability
{
	AbilityData _aData;
	float _cCastTime = 0;
	S_Ability _state;
	protected Transform _owner;
	float _cdTime;
	protected Unit _target;

	public Ability(Transform owner) {
		_state = S_Ability.READY;
		_owner = owner;
		_cdTime = 0;
	}
	public void Update() {
		if (_state == S_Ability.READY) return;
		else if (_state == S_Ability.CASTING) {
			_cCastTime += Time.deltaTime;
			if (_cCastTime >= _aData._castTime) {
				//casting the ability
				_state = S_Ability.COOLDOWN;
				_cdTime = _aData._cooldown;
				Cast();
			}
		}
		else if (_state == S_Ability.COOLDOWN) {
			_cdTime -= Time.deltaTime;
			if (_cdTime <= 0) {
				_state = S_Ability.READY;
			}
		}
	}
	public virtual void Cast() {

		Debug.Log("CASTING ABILITY ");
	}
	public void BeginCasting(Unit target) {
		_state = S_Ability.CASTING;
		_cCastTime = 0;
		_target = target;
	}
	public void SetAbilityData(AbilityData ad) {
		_aData = ad;
	}
	public AbilityData AData { get => _aData;}
	public S_Ability State { get => _state; }
}
