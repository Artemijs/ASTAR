using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class AbilityAoe : Ability
{
	float _radius;
	public AbilityAoe(Transform owner) : base(owner) {

	}
	public override void Cast() {
		Debug.Log("CASTING AN AOEEEEEEEEEEEEE EXUPUROSIOOOOOOOOOOOOOON");
	}
}
