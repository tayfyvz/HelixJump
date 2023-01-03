using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleManager : Singleton<CircleManager>
{
    public List<CircleController> circles = new List<CircleController>();
    
    [SerializeField] private PieceController piecePrefab;
    [SerializeField] private CircleController circlePrefab;
    [SerializeField] private FinishPlatformController finishPlatformPrefab;

    public float verticalDistance;
    public int circleAmount;

    public GameObject cylinder;

    [ContextMenu(nameof(EditorCreate))]
    public void EditorCreate()
    {
        Create(circleAmount);
    }
    private void Create(int amount)
    {
        circles.Clear();
        
        FinishPlatformController fpc = Instantiate(finishPlatformPrefab, new Vector3(0, -verticalDistance, 0), Quaternion.identity, transform);
        fpc.CreateChains(verticalDistance);
        
        for (int i = 0; i < amount; i++)
        {
            // GameObject circle = CreateCircle();
            CircleController circle = Instantiate(circlePrefab, transform);
            
            circle.transform.position = new Vector3(0, i * verticalDistance, 0);
            int random = UnityEngine.Random.Range(2, 5);
            circle.RandomPieceActivate(random);
            circle.transform.parent = cylinder.transform;
            circles.Add(circle);
        }
        float topPoint = verticalDistance * (circleAmount - 1);
        CameraController.Instance.SetCam(topPoint);
    }
    private GameObject CreateCircle()
    {
        GameObject circleParent = new GameObject()
        {
            transform = { name = "Circle"}
        };
        for (int i = 0; i < 10; i++)
        {
            PieceController piece = Instantiate(piecePrefab, circleParent.transform);
            Quaternion rotation = piece.transform.rotation;
            rotation.eulerAngles = new Vector3(0, 36 * i, 0);
            piece.transform.rotation = rotation;
        }

        return circleParent;
    }

    public void CirclePassed()
    {
        if (circles.Count > 0)
        {
            circles[^1].RemoveCircle();
            circles.Remove(circles[^1]);
        }
    }
}
