using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitFactory : MonoBehaviour
{
	public GameObject _basicUnit;
	AbilityFactory _abFactory;
	private void Awake()
	{
		_abFactory = new AbilityFactory();
	}
	public Unit BasicUnit() {
		Stats s = new Stats();
		GameObject unitGo =
			GameObject.Instantiate(
				_basicUnit, new Vector3(20, 6, 20), Quaternion.identity);
		Unit u = unitGo.AddComponent<Unit>();
		u.Init();
		s.SetStatValue(StatIndex.MOVE_SPEED, 10);
		u.SetStats(s);
		u.AddAbility(_abFactory.BasicAbility(unitGo.transform));
		return u;
	}
}
