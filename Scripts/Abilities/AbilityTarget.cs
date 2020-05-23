using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityTarget<T>
{
	T _target;
	public AbilityTarget(T target) {
		_target = target;
	}
	public T Target {
		get { return _target; }
	}
}
