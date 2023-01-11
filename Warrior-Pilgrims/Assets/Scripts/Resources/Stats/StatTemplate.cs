using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stat Template", menuName = "Stat System/Stat Template")]
public class StatTemplate : Data
{
	[SerializeField] List<BaseStat> stats;

	public List<BaseStat> Stats => stats;
}
