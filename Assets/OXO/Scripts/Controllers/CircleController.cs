using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class CircleController : MonoBehaviour
{
    [Header("Random Interval")] 
    [SerializeField] private int minNum;
    [SerializeField] private int maxNum;
    
    [SerializeField] private List<PieceController> pieces = new List<PieceController>();

    public void RandomPieceActivate(int inactiveAmount)
    {
        for (int i = 0; i < inactiveAmount; i++)
        {
            int index = UnityEngine.Random.Range(0, pieces.Count);
            PieceController piece = pieces[index];
            piece.gameObject.SetActive(false);
        }
    }

    [Button]
    public void RandomPieceSetter()
    {
        List<Material> materials = new List<Material>
        {
            Resources.Load<Material>("Materials/Platform"),
            Resources.Load<Material>("Materials/PlatformBad"),
            Resources.Load<Material>("Materials/PlatformGood")
        };
        // new List<Material>(Resources.LoadAll<Material>("Materials"));
        foreach (var t in pieces)
        {
            int type = UnityEngine.Random.Range(0, 3);
            int randomNum = UnityEngine.Random.Range(minNum, maxNum);
            t.SetType(type, randomNum,materials[type]);
        }
    }

    public void RemoveCircle()
    {
        foreach (var t in pieces)
        {
            t.RemovePiece();
        }
    }
    
}