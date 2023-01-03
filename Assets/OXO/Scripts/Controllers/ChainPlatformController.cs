using TMPro;
using UnityEngine;

public class ChainPlatformController : MonoBehaviour
{
    public float breakNumber;

    [SerializeField] private TextMeshPro breakNumText;
    [SerializeField] private Animator animator;

    public void SetText(float num)
    {
        breakNumber = num;
        breakNumText.text = breakNumber.ToString();
    }
    public bool HitToChain(float num)
    {
        if (num >= breakNumber)
        {
            animator.SetTrigger("HitChain");
            return true;
        }

        return false;
    }
}