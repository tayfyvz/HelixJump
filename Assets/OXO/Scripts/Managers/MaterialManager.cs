using System.Collections.Generic;
using UnityEngine;

public class MaterialManager : Singleton<MaterialManager>
{
    private Material _normalPlatformMaterial;
    private Material _badPlatformMaterial;
    private Material _goodPlatformMaterial;
    private Material _cylinderMaterial;
    private Material _playerMaterial;
    private ParticleSystem _splashPrefab;
    private TrailRenderer _trailPrefab;
    private ParticleSystem _smokePrefab;
    private Material _skyboxMaterial;

    
    [SerializeField] private List<LevelData> levelData = new List<LevelData>();

    private void Awake()
    {
        _normalPlatformMaterial = Resources.Load<Material>("Materials/Platform");
        _badPlatformMaterial = Resources.Load<Material>("Materials/PlatformBad");
        _goodPlatformMaterial = Resources.Load<Material>("Materials/PlatformGood");
        _cylinderMaterial = Resources.Load<Material>("Materials/Cylinder");
        _playerMaterial = Resources.Load<Material>("Materials/Player");
        _splashPrefab = Resources.Load<ParticleSystem>("Prefab/Splash");
        _trailPrefab = Resources.Load<TrailRenderer>("Prefab/Trail");
        _smokePrefab = Resources.Load<ParticleSystem>("Prefab/Smoke");
        _skyboxMaterial = Resources.Load<Material>("Skybox/Sky");
    }

    public void SetMaterials(int index)
    {
        _normalPlatformMaterial.color = levelData[index].normalPlatformColor;
        _badPlatformMaterial.color = levelData[index].badPlatformColor;
        _goodPlatformMaterial.color = levelData[index].goodPlatformColor;
        _cylinderMaterial.color = levelData[index].cylinderColor;
        _playerMaterial.color = levelData[index].playerColor;
        ParticleSystem.MainModule splashSettings = _splashPrefab.GetComponent<ParticleSystem>().main;
        splashSettings.startColor = levelData[index].splashColor;
        ParticleSystem.MainModule splashSecondSettings = _splashPrefab.transform.GetChild(0).GetComponent<ParticleSystem>().main;
        splashSecondSettings.startColor = levelData[index].splashColor;
        _trailPrefab.sharedMaterial.SetColor("_TintColor", levelData[index].trailTintColor);
        _trailPrefab.colorGradient = levelData[index].trailColor;
        ParticleSystem.MainModule smokeSettings = _smokePrefab.GetComponent<ParticleSystem>().main;
        smokeSettings.startColor = levelData[index].smokeColor;
        _skyboxMaterial.SetColor("_EquatorColor", levelData[index].skyboxEquatorColor);
        _skyboxMaterial.SetColor("_GroundColor", levelData[index].skyboxGroundColor);
    }
}