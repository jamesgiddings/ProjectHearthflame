using System;

[Flags]
[Serializable]
public enum TargetFlags // create an editor tool which populates this through a series of bools with toggle groups so that you don't have things which should be mutually exclusive
{
	Single = 1,
	All = 2,
	AllExceptUser = 4,

	AllyFront = 8,
	AllyRear = 16,
	
	EnemyFront = 32,
	EnemyRear = 64,
}

