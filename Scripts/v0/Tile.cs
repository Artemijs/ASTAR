
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace temp
{
	public enum NIndx
	{
		TOP = 0,
		TOPRIGHT,
		RIGHT,
		BOTRIGHT,
		BOT,
		BOTLEFT,
		LEFT,
		TOPLEFT
	};
	public struct arr_ptr
	{
		public int i;
		public int j;
		public arr_ptr(int iv, int jv) { i = iv; j = jv; }
	};
	public class Tile : MonoBehaviour
	{
		int _h;
		int _g;
		int _f;
		public bool _walkable = true;
		arr_ptr[] _all_n_tiles;//all neighbouring nodes
		public Tile parent;
		public void Init()
		{
			_all_n_tiles = new arr_ptr[8];
			parent = null;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="n"> neighbour index enum</param>
		/// <param name="ptr"> index in all node array</param>
		public void SetNeighbor(NIndx n, arr_ptr ptr)
		{
			_all_n_tiles[(int)n] = ptr;
		}
		public int GetMaxNS()
		{
			return _all_n_tiles.Length;
		}
		/// <summary>
		/// get neighbouring tile
		/// </summary>
		/// <param name="n"></param>
		/// <returns></returns>
		public arr_ptr GetNTile(NIndx n)
		{
			return _all_n_tiles[(int)n];
		}
		// Update is called once per frame
		void Update()
		{

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
	}

}