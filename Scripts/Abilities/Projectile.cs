using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Projectile : MonoBehaviour
{
	Unit _target;
	float _speed;
	Action<Unit> _onHit;
	public void Init(Unit target, float speed, Action<Unit> callback) {
		_target = target;
		_speed = speed;
		_onHit = callback;
	}
    void Update() {
		//move 2 target
		transform.position = Vector3.MoveTowards(transform.position, _target.gameObject.transform.position, Time.deltaTime * _speed);
    }
	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.GetComponent<Unit>() == _target) {
			_onHit(_target);
		}
	}
}
