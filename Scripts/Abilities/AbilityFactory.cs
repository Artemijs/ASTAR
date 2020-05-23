using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityFactory 
{
	AbilityPrefabs _abPrefs;
	public AbilityFactory() {
		_abPrefs = GameObject.Find("Main").GetComponent<AbilityPrefabs>();
	}
	public Ability BasicAbility( Transform owner ) {
		AbilityData ad = new AbilityData();
		Ability a = new Ability(owner);
		ad._manaCost = 10;
		ad._selfCast = false;
		ad._hpDisplacement = 10;
		ad._castTime = 0.25f;
		ad._cooldown = 6;
		a.SetAbilityData(ad);
		return a;
	}
	public Ability BasicProjectileAbility(Transform owner) {
		AbilityData ad = new AbilityData();

		GameObject p = GameObject.Instantiate(_abPrefs._projectile, owner);
		p.transform.position = owner.position;
		AbilityProjectile a = new AbilityProjectile(p.GetComponent<Projectile>(), owner);
		ad._manaCost = 10;
		ad._selfCast = false;
		ad._hpDisplacement = 10;
		ad._castTime = 0.25f;
		ad._cooldown = 6;
		a.SetAbilityData(ad);
		return a;
	}
}
