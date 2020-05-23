using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace temp
{

	public class Unit : MonoBehaviour
	{
		//S_Unit _state;
		// Start is called before the first frame update
		Rigidbody _body;
		void Start()
		{
			//_state = S_Unit.IDLE;
			_body = gameObject.GetComponent<Rigidbody>();

		}

		// Update is called once per frame
		void Update()
		{
			//if (_state == S_Unit.IDLE) return;
		}
		public bool Move(Transform target)
		{
			float speed = 25;
			Vector3 trgt = target.position;
			trgt.y = 6;
			transform.position = Vector3.MoveTowards(transform.position, trgt, Time.deltaTime * speed);
			float distance = Vector3.Distance(trgt, transform.position);
			bool arrived = (distance < 0.5);
			if (arrived)
			{
				transform.position = trgt;
			}
			return arrived;
		}
		public void ChangeState(S_UNIT_ACTION_ID newState)
		{
			//_state = newState;
		}
	}
}
