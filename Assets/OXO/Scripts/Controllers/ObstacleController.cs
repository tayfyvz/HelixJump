using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    [SerializeField] private float amount;
    [SerializeField] private TextMeshPro text1, text2;

    public void SetText(float num)
    {
        amount = num;
        text1.text = amount.ToString();
        text2.text = amount.ToString();
        
        // List<Material> materials = new List<Material>(Resources.LoadAll<Material>("Materials"));
        List<Material> materials = new List<Material>
        {
            Resources.Load<Material>("Materials/Platform"),
            Resources.Load<Material>("Materials/PlatformBad"),
            Resources.Load<Material>("Materials/PlatformGood")
        };
        transform.GetComponent<MeshRenderer>().material = amount > 0 ? materials[2] : materials[1];
    }
}