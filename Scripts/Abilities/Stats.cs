using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatIndex {
	MOVE_SPEED = 0,
};
public class Stats
{
	List<float> _allStats;
	public Stats() {
		_allStats = new List<float>();
		for (int i = 0; i < (int)StatIndex.MOVE_SPEED + 1; i++) {
			_allStats.Add(0);
		}
	}
	public void SetStatValue(StatIndex index, float value) {
		_allStats[(int)index] = value;
	}
	public void ChangeStat(StatIndex index, float value) {
		_allStats[(int)index] += value;
	}
	public float GetStat(StatIndex index) {
		return _allStats[(int)index];
	}
}
