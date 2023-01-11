using System;

[Flags]
[Serializable]
public enum TargetAreaFlag1 // create an editor tool which populates this through a series of bools with toggle groups so that you don't have things which should be mutually exclusive
{
   OOOO_OOOO = 1,
   X = 2,

    OpponentFront = 4,
    OpponentRear = 8,
}