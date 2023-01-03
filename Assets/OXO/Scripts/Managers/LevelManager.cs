using System.Collections.Generic;
using UnityEngine;


public class LevelManager : Singleton<LevelManager>
{
    public int randomStartLevel;
    [HideInInspector]
    public List<GameObject> currentLevelObjectsList;
    //[HideInInspector]
    public List<GameObject> levelPrefabList;

    [Header("Current Level")]
    public GameObject currentLevel;
    public GameObject player;

    [Header("Player Prefab")]
    public GameObject playerPrefab;

    public int level;
    public int index;

    private void Awake()
    {
        level = PlayerPrefs.GetInt("Level");
    }
    private void Start()
    {
        SetLevel();
    }
    public void SetLevel()
    {
        ResetLevel();
        ConfettiManager.Instance.Stop();
        Spawn();

    }
    public void Spawn()
    {
        if (levelPrefabList.Count == 0)
        {
            Debug.Log($"<color=orange><b>(!) Couldn't find level in the Level List.</b> </color>"); return;
        }

        if (level >= levelPrefabList.Count)
        { 
            index = UnityEngine.Random.Range(randomStartLevel, levelPrefabList.Count);
            currentLevel = Instantiate(levelPrefabList[index]);
            MaterialManager.Instance.SetMaterials(UnityEngine.Random.Range(randomStartLevel, levelPrefabList.Count));
        }
        else
        {
            index = level % levelPrefabList.Count;
            currentLevel = Instantiate(levelPrefabList[index]);
            MaterialManager.Instance.SetMaterials(index);
        }
        
        currentLevel.SetActive(true);
        currentLevelObjectsList.Add(currentLevel);

        CanvasManager.Instance.levelText.text = "Level " + (level + 1).ToString();

        if (playerPrefab)
        {
            player = Instantiate(playerPrefab);
            player.transform.position = Vector3.zero;
            player.SetActive(true);
            currentLevelObjectsList.Add(player);

        }
    }

    private void ResetLevel()
    {
        Destroy(currentLevel);
        //ResetList(currentLevelObjectsList);
    }
    public void IncreaseLevel()
    {
        level++;
        PlayerPrefs.SetInt("Level", level);
    }

    private void ResetList(List<GameObject> list)
    {
        list.ForEach(x => Destroy(x.gameObject));
        list.Clear();
    }
    public void ResetCamera()
    {
        //Camera.main.GetComponent<CameraDisplay>().ResetCamera();
    }
}