using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class Piece : MonoBehaviour
{
    [Header("Piece Settings")]
    public float number;
    protected Type platformType;
    public enum Type
    {
        Normal,
        Good,
        Bad
    }
    public void SetType(int type, int randomNum, Material mat)
    {
        ChangeMaterial(mat);
        switch (type)
        {
            case 0:
                platformType = Type.Normal;
                SetPieceAsNormal();
                SetText(0);
                break;
            case 1:
                platformType = Type.Bad;
                SetPieceAsBad();
                SetText(-randomNum);
                break;
            case 2:
                platformType = Type.Good;
                SetPieceAsGood();
                SetText(randomNum);
                break;
        }
    }

    public virtual void RemovePiece()
    {
        transform.DOJump(new Vector3(transform.position.x - transform.forward.x * 1, transform.position.y - transform.forward.y * 3, transform.position.z - transform.forward.z * 2), 1.5f, 1, .3f).SetEase(Ease.Linear).OnComplete((() =>
        {
            
        }));
        transform.DOScale(Vector3.zero, .5f).SetEase(Ease.Linear).OnComplete((() =>
        {
            gameObject.SetActive(false);
        }));
    }
    protected virtual void SetPieceAsNormal()
    {
        gameObject.tag = "Normal";
    }
    protected virtual void SetPieceAsGood()
    {
        gameObject.tag = "Good";
    }

    protected virtual void SetPieceAsBad()
    {
        gameObject.tag = "Bad";
    }
    protected virtual void SetText(float num)
    {
        TextMeshProUGUI numberText = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        number = num;
        if (num != 0)
        {
            numberText.text = num.ToString();
            return;
        }

        numberText.text = "";
    }
    protected void ChangeMaterial(Material mat)
    {
        GetComponent<MeshRenderer>().material = mat;
    }
}