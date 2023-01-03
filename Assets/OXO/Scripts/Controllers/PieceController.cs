using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class PieceController : Piece
{
    private List<Material> _materials = new List<Material>();
    
    [SerializeField] private int health;
    
    [SerializeField] private bool isBreakable;
    
    [SerializeField] private CollectableController collectableController;

    [SerializeField] private ObstacleController obstacleController;

    [Button]
    public void SetType()
    {
        _materials.Add(Resources.Load<Material>("Materials/Platform")); 
        _materials.Add(Resources.Load<Material>("Materials/PlatformBad")); 
        _materials.Add(Resources.Load<Material>("Materials/PlatformGood")); 

        // = new List<Material>(Resources.LoadAll<Material>("Materials"));
        switch (number)
        {
            case 0:
                ChangeMaterial(_materials[0]);
                SetPieceAsNormal();
                SetText(0);
                break;
            case < 0:
                ChangeMaterial(_materials[1]);
                SetPieceAsBad();
                SetText(number);
                break;
            case > 0:
                ChangeMaterial(_materials[2]);
                SetPieceAsGood();
                SetText(number);
                break;
        }
    }

    [Button]
    public void CreateCollectable(float amount, float height)
    {
        CollectableController cc = Instantiate(collectableController,transform);
        var transform1 = transform;
        var position = transform1.position;
        cc.transform.position = new Vector3(position.x, position.y + .3f, position.z);
        cc.transform.GetChild(0).position = new Vector3(position.x, position.y + .4f + height, position.z);
        cc.amount = amount;
        cc.SetText(amount);
    }
    
    [Button]
    public void CreateObstacle(float amount)
    {
        ObstacleController oc = Instantiate(obstacleController,transform);
        oc.transform.position = transform.position;

        oc.SetText(amount);
    }
    public void Hit()
    {
        if (health == -1)
        {
            return;
        }
        
        health--;
        if (isBreakable)
        {
            if (health == 0)
            {
                transform.DOScale(Vector3.zero, .2f).SetEase(Ease.InQuart).OnComplete((() =>
                {
                    gameObject.SetActive(false);
                }));
            }
        }
        else
        {
            if (health == 0)
            {
                Material mat = Resources.Load<Material>("Materials/Platform");
                    
                ChangeMaterial(mat);
                SetPieceAsNormal();
                SetText(0);
            }
        }
    }
    // [Button]
    // public void SetType(Type type)
    // {
    //     platformType = type;
    //     List<Material> materials = new List<Material>(Resources.LoadAll<Material>("Materials"));
    //     switch (platformType)
    //     {
    //         case Type.Normal:
    //             ChangeMaterial(materials[0]);
    //             SetPieceAsNormal();
    //             SetText(0);
    //             break;
    //         case Type.Bad:
    //             ChangeMaterial(materials[1]);
    //             SetPieceAsBad();
    //             SetText(number);
    //             break;
    //         case Type.Good:
    //             ChangeMaterial(materials[2]);
    //             SetPieceAsGood();
    //             SetText(number);
    //             break;
    //     }
    // }


    
}