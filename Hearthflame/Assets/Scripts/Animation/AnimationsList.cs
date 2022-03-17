using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Animations List", menuName = "Characters/Animations List")]
public class AnimationsList : ScriptableObject
{
	[SerializeField] List<AnimationClip> animationClipsList;

	public List<AnimationClip> AnimationClipsList => animationClipsList;
}
