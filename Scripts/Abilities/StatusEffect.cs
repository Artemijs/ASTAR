using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect
{
	StatIndex _index;
	float _duration;
	float _tickInterval;
	float _value;
	float _currDuration;
	float _currTickTime;
	public StatusEffect() {

	}

	public bool Update() {
		if (_duration != 0)
		{
			_currDuration += Time.deltaTime;
			if (_currDuration >= _duration)
				return true;
		}
		if(_tickInterval !=0)
			_currTickTime -= Time.deltaTime;
		return false;
	}
	public float GetValue() {
		if (_tickInterval != 0)
		{
			if (_currTickTime <= 0)
			{
				_currTickTime = _tickInterval;
				return _value;
			}
		}
		else return _value;
		return 0;
	}
	public StatIndex Index {
		get { return _index; }
	}
}
