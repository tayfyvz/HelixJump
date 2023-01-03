using System.Collections.Generic;
using MText;
using UnityEngine;

public class CollectableController : MonoBehaviour
{
    public float amount;
    [SerializeField] private ParticleSystem splashEffect;
    private Material _mat;

    private void Start()
    {
        transform.GetChild(0).GetComponent<Modular3DText>().text = amount.ToString();
    }

    public void SetText(float num)
    {
        // List<Material> materials = new List<Material>(Resources.LoadAll<Material>("Materials"));
        List<Material> materials = new List<Material>();
        materials.Add(Resources.Load<Material>("Materials/Platform")); 
        materials.Add(Resources.Load<Material>("Materials/PlatformBad")); 
        materials.Add(Resources.Load<Material>("Materials/PlatformGood")); 
        
        if (num > 0)
        {
            _mat = materials[2];
            transform.GetChild(0).GetComponent<Modular3DText>().material = _mat;
        }
        else
        {
            _mat = materials[1];
            transform.GetChild(0).GetComponent<Modular3DText>().material = _mat;
        }
        transform.GetChild(0).GetComponent<Modular3DText>().text = num.ToString();
        transform.GetChild(0).GetComponent<Modular3DText>().UpdateText();
        this.amount = num;
        //transform.GetChild(0).GetComponent<CollectableController>().amount = num;
    }

    public float Hit()
    {
        Transform transform1;
        ParticleSystem splash = Instantiate(splashEffect, (transform1 = transform).GetChild(0).GetChild(0).position, Quaternion.identity, transform1.parent);
        ParticleSystem.MainModule settings = splash.GetComponent<ParticleSystem>().main;
        settings.startColor = amount > 0 ? Color.green : Color.red;
        gameObject.SetActive(false);
        return amount;
    }
}