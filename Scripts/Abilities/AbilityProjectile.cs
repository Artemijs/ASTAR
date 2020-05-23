using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityProjectile : Ability
{
	public static Transform _abilityParent;
	Projectile _projectile;
	public AbilityProjectile(Projectile p, Transform owner) : base(owner) {
		_projectile = p;
	}
	public override void Cast() {
		_projectile.Init(_target, 20, ProjectileCollided);
		_projectile.gameObject.SetActive(true);
		Vector3 lScale = _projectile.transform.localScale;
		_projectile.transform.SetParent(_abilityParent);
		_projectile.transform.localScale = lScale;
	}
	public void ProjectileCollided(Unit target) {
		Debug.Log("COLLIDED WITH TARGET");
		Vector3 lScale = _projectile.transform.localScale;
		_projectile.transform.SetParent(base._owner);
		_projectile.transform.localScale = lScale;
		_projectile.transform.position = base._owner.position;
		_projectile.gameObject.SetActive(false);
		base.Cast();
	}

}
