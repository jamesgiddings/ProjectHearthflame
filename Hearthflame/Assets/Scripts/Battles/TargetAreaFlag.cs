using System;

[Flags]
[Serializable]
public enum TargetAreaFlag // create an editor tool which populates this through a series of bools with toggle groups so that you don't have things which should be mutually exclusive
{
	AllyFront = 1,
	AllyRear = 2,

	EnemyFront = 4,
	EnemyRear = 8,
}