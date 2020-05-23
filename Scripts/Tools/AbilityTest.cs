using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityTest : MonoBehaviour
{
	List<Ability> _allAbilities;
	public GameObject _testPlayer;
	public GameObject _testTarget;
	Unit _targetUnit;
	InputHandler _ih;
	// Start is called before the first frame update
	void Start()
    {
		_targetUnit = _testTarget.GetComponent<Unit>();

		AbilityPrefabs aps = GetComponent<AbilityPrefabs>();

		AbilityProjectile._abilityParent = GameObject.Find("AbilityParents").transform;

		_allAbilities = new List<Ability>();
		_allAbilities.Add(new Ability(_testPlayer.transform));
		_allAbilities.Add(new AbilityAoe(_testPlayer.transform));

		GameObject p = GameObject.Instantiate(aps._projectile, _testPlayer.transform);
		p.transform.position = _testPlayer.transform.position;
		_allAbilities.Add(new AbilityProjectile(p.GetComponent<Projectile>(), _testPlayer.transform));

		_ih = GetComponent<InputHandler>();
	}

	//TEST IF CAST CALLS THE RIGHT FUNCTION
	void Update()
    {
		if (Input.GetKeyDown(KeyCode.Q))
		{
			_allAbilities[0].BeginCasting(_targetUnit);
		}
		else if (Input.GetKeyDown(KeyCode.W))
		{
			_allAbilities[1].BeginCasting(_targetUnit);
		}
		else if (Input.GetKeyDown(KeyCode.E))
		{
			_allAbilities[2].BeginCasting(_targetUnit);
		}
		foreach (Ability a in _allAbilities)
		{
			a.Update();
		}
	}
}
