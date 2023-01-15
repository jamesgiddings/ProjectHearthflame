using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class LightFlicker : MonoBehaviour
{
	/*[Header("Intensity Flicker")] 

	[SerializeField] float minimumIntensityModifier;
	[SerializeField] float maximumIntensityModifier;

	[Space(10)]

	[SerializeField] float minimumIntensityDuration;
	[SerializeField] float maximumIntensityDuration;

	[Header("Inner Radius Flicker")]

	[SerializeField] float minimumInnerRadiusModifier;
	[SerializeField] float maximumInnerRadiusModifier;

	[Space(10)]

	[SerializeField] float minimumInnerRadiusDuration;
	[SerializeField] float maximumInnerRadiusDuration;

	[Header("Outer Radius Flicker")]

	[SerializeField] float minimumOuterRadiusModifier;
	[SerializeField] float maximumOuterRadiusModifier;

	[Space(10)]

	[SerializeField] float minimumOuterRadiusDuration;
	[SerializeField] float maximumOuterRadiusDuration;

	private UnityEngine.Rendering.Universal.Light2D light2D;
	private float intensityCache;
	private float innerRadiusCache;
	private float outerRadiusCache;

	private Tweener intensityFlicker;
	private Tweener innerRadiusFlicker;
	private Tweener outerRadiusFlicker;

	private void Start()
	{
		light2D = GetComponent<UnityEngine.Rendering.Universal.Light2D>();

		intensityCache = light2D.intensity;
		innerRadiusCache = light2D.pointLightInnerRadius;
		outerRadiusCache = light2D.pointLightOuterRadius;


		intensityFlicker = light2D.DOIntensity(intensityCache * RandomizeIntensity(), RandomizeIntensityFlickerDuration()).SetEase(Ease.InOutQuad).OnStepComplete(() => ChangeIntensityEndValue()).SetLoops(-1, LoopType.Yoyo).Play();
		innerRadiusFlicker = light2D.DOInnerRadius((innerRadiusCache * RandomizeInnerRadius()), RandomizeInnerRadiusFlickerDuration()).SetEase(Ease.InOutQuad).OnStepComplete(() => ChangeInnerRadiusEndValue()).SetLoops(-1, LoopType.Yoyo).Play();
		outerRadiusFlicker = light2D.DOOuterRadius((outerRadiusCache * RandomizeOuterRadius()), RandomizeOuterRadiusFlickerDuration()).SetEase(Ease.InOutQuad).OnStepComplete(() => ChangeOuterRadiusEndValue()).SetLoops(-1, LoopType.Yoyo).Play();


	}

	private void ChangeIntensityEndValue()
	{
		intensityFlicker.ChangeEndValue((intensityCache * RandomizeIntensity()), RandomizeIntensityFlickerDuration(), true);
	}

	private void ChangeInnerRadiusEndValue()
	{
		innerRadiusFlicker.ChangeEndValue((innerRadiusCache * RandomizeInnerRadius()), RandomizeInnerRadiusFlickerDuration(), true);
	}

	private void ChangeOuterRadiusEndValue()
	{
		outerRadiusFlicker.ChangeEndValue((outerRadiusCache * RandomizeOuterRadius()), RandomizeOuterRadiusFlickerDuration(), true);
	}


	private float RandomizeIntensity()
	{
		return Random.Range(minimumIntensityModifier, maximumIntensityModifier);
	}		
	
	private float RandomizeInnerRadius()
	{
		return Random.Range(minimumInnerRadiusModifier, maximumInnerRadiusModifier);
	}		
	
	private float RandomizeOuterRadius()
	{
		return Random.Range(minimumOuterRadiusModifier, maximumOuterRadiusModifier);
	}	
	
	private float RandomizeIntensityFlickerDuration()
	{
		return Random.Range(minimumIntensityDuration, maximumIntensityDuration);
	}

	private float RandomizeInnerRadiusFlickerDuration()
	{
		return Random.Range(minimumInnerRadiusDuration, maximumInnerRadiusDuration);
	}

	private float RandomizeOuterRadiusFlickerDuration()
	{
		return Random.Range(minimumOuterRadiusDuration, maximumOuterRadiusDuration);
	}
}

public static class FlickerTweeners
{
	public static Tweener DOColor(this UnityEngine.Rendering.Universal.Light2D target, Color endValue, float duration)
	{
		return DOTween.To(() => target.color, x => target.color = x, endValue, duration).SetTarget(target);
	}	
	
	public static Tweener DOInnerRadius(this UnityEngine.Rendering.Universal.Light2D target, float endValue, float duration)
	{
		return DOTween.To(() => target.pointLightInnerRadius, x => target.pointLightInnerRadius = x, endValue, duration).SetTarget(target);
	}

	public static Tweener DOOuterRadius(this UnityEngine.Rendering.Universal.Light2D target, float endValue, float duration)
	{
		return DOTween.To(() => target.pointLightOuterRadius, x => target.pointLightOuterRadius = x, endValue, duration).SetTarget(target);
	}

	public static Tweener DOIntensity(this UnityEngine.Rendering.Universal.Light2D target, float endValue, float duration)
	{
		return DOTween.To(() => target.intensity, x => target.intensity = x, endValue, duration).SetTarget(target);
	}*/
}
