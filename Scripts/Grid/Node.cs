using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
	int _h;
	int _g;
	int _f;
	bool _free;
	Node _parent;
	Node _next;//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
	Vector3 _position;
	arr_ptr _index;
	public Node() {
		_parent = null;
		_next = null;
	}
	
	public Node(Vector3 position, arr_ptr index)
	{
		_index = index;
		_position = position;
		_parent = null;
		_free = true;
		_next = null;
	}
	public Node parent {
		get { return _parent; }
		set { _parent = value; }
	}
	public int fCost
	{
		get
		{
			return _g + _h;
		}
	}
	public int gCost
	{
		get
		{
			return _g;
		}
		set { _g = value; }
	}
	public int hCost
	{
		get
		{
			return _h;
		}
		set
		{
			_h = value;
		}
	}
	public bool free {
		get { return _free; }
		set { _free = value; }
	}
	public Vector3 position
	{
		get { return _position; }
		set { _position = value; }
	}
	public arr_ptr Index {
		get { return _index; }
		set { _index = value; }
	}
	public Node Next {
		get { return _next; }
		set { _next = value; }
	}
}
