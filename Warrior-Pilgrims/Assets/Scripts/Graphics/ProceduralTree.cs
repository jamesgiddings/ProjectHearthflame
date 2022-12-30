using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ProceduralTree : MonoBehaviour
{
    //private LineRenderer lineRenderer;
    private Vector3 position;
    private List<Vector3> branchPoints = new List<Vector3>();
    private float randomTerminalHeight;
    [SerializeField] private float minTerminalHeight = 10f;
    [SerializeField] private float maxTerminalHeight = 50f;

    [SerializeField] float chanceToBranch = 0.3f;
    [SerializeField] bool heightWeightToChanceToBranch = true;
    [SerializeField] float heightWeightToChanceToBranchValue = 0.1f; // increases chance to branch with each segment
    private int index;
    private int branchIndex;
    Vector3[] positionArray;


    void Start()
    {
        chanceToBranch = 0f;
        index = 0;
        LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
        position = new Vector3(0f, 0f, 0f);
        randomTerminalHeight = UnityEngine.Random.Range(minTerminalHeight, maxTerminalHeight);
        InitializeShape(lineRenderer);
        CreateSegment(lineRenderer);
        SetSegmentGirths(lineRenderer);
    }

	private void InitializeShape(LineRenderer lineRenderer)
	{
        lineRenderer.SetPosition(index, position);
	}

	private void CreateSegment(LineRenderer lineRenderer)
	{
        index++;
        SetHeight(lineRenderer);
        
        while (position.y < randomTerminalHeight)
        {
            lineRenderer.positionCount += 1;
            CreateSegment(lineRenderer);
            CreateNewBranch();
        }
    }

	private void CreateNewBranch()
	{
        bool branched = false;
        if (UnityEngine.Random.Range(0f, 1f) > chanceToBranch)
		{
            LineRenderer newLineRenderer = new LineRenderer();
            CreateSegment(newLineRenderer);
            branched = true;
        }
        if (branched == false && heightWeightToChanceToBranch)
		{
            chanceToBranch += heightWeightToChanceToBranchValue;
        }
	}

	private void SetSegmentGirths(LineRenderer lineRenderer)
	{
        AnimationCurve curve = new AnimationCurve();
        float curveDivision = 1f / lineRenderer.positionCount;
        //Debug.Log(curveDivision);
        //Debug.Log(lineRenderer.positionCount);
        float girthMultiplier = 1f;
        curve.AddKey(0.0f, 1.0f);
        for (int i = 0; i < lineRenderer.positionCount; i++)
		{
            girthMultiplier *= UnityEngine.Random.Range(0.8f, 0.96f);
            
            curve.AddKey(i * curveDivision, girthMultiplier);
            //Debug.Log(i * curveDivision);
            //Debug.Log(curve.Evaluate(i * curveDivision));
        }
        lineRenderer.widthCurve = curve;
    }

	private void SetHeight(LineRenderer lineRenderer)
	{
        position = GeneratePosition(lineRenderer);
        lineRenderer.SetPosition(index, position);
    }

	private Vector3 GeneratePosition(LineRenderer lineRenderer)
	{
        float randomSizeY = UnityEngine.Random.Range(0.2f, 2f) + lineRenderer.GetPosition(index - 1).y;
        float randomSizeX = UnityEngine.Random.Range(0.2f, 2f);
        position = new Vector3(randomSizeX, randomSizeY, 0);
        return position;
	}

	void Update()
    {
        
    }
}

