using System;

[Flags]
[Serializable]
public enum TargetNumberFlag // create an editor tool which populates this through a series of bools with toggle groups so that you don't have things which should be mutually exclusive
{
	Single = 1,
	All = 2,
	AllExceptUser = 4,
}