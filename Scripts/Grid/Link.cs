using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link
{
	Link _next;
	arr_ptr _index;
	Vector3 _position;
	public Link()
	{
		_next = null;
	}
	public Link(Vector3 pos, arr_ptr index, Link next) {
		_next = next;
		_index = index;
		_position = pos;
	}
	public Vector3 position
	{
		get { return _position; }
		set { _position = value; }
	}
	public arr_ptr Index
	{
		get { return _index; }
		set { _index = value; }
	}
	public Link Next
	{
		get { return _next; }
		set { _next = value; }
	}
}
