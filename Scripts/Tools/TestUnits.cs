using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUnits : MonoBehaviour
{
	UnitFactory _uFactory;
	// Start is called before the first frame update
	Unit _testUnit;
    void Start() {
		_uFactory = GetComponent<UnitFactory>();
		_testUnit = _uFactory.BasicUnit();
	}
    // Update is called once per frame
    void Update()
    {
        
    }
}
