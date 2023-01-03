using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FinishPlatformController : MonoBehaviour
{
    [SerializeField] private ChainPlatformController chainPrefab;
    [SerializeField] private GameObject finishPrefab;
    [SerializeField] private TextMeshPro finishMultiplier;

    public void CreateChains(float verDistance)
    {
        for (int i = 0; i < 10; i++)
        {
            ChainPlatformController cpc = Instantiate(chainPrefab, new Vector3(0, (i + 2) * -verDistance, 0),
                Quaternion.identity, transform);
            cpc.SetText(100 * (i + 1));
            cpc.transform.eulerAngles = new Vector3(0, -90, 0);

            if (i != 9)
            {
                TextMeshPro multiplierText = Instantiate(finishMultiplier,
                    new Vector3(0, ((i + 2) * -verDistance) + verDistance / 2, -1.6f),
                    Quaternion.identity, transform);
                multiplierText.text += (i + 1);
            }
            else
            {
                TextMeshPro multiplierText = Instantiate(finishMultiplier,
                    new Vector3(0, ((i + 2) * -verDistance) - verDistance / 4, -1.6f),
                    Quaternion.identity, transform);
                multiplierText.text += (i + 1);
            }
        }

        Instantiate(finishPrefab, new Vector3(0, (10 + 2) * -verDistance, 0),
            Quaternion.identity, transform);
    }
}