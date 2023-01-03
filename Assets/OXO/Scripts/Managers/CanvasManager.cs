using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Managers.Sound;
using Managers.Vibration;
using TMPro;

public class CanvasManager : Singleton<CanvasManager>
{

    [Header("COIN")]
    public Transform TargetPos;
    [SerializeField] private int _maxCoins;
    [SerializeField] private GameObject _coinPrefab;
    private Queue<GameObject> coinsQueue = new Queue<GameObject>();
    private float minAnimDuration = .7f;
    private float maxAnimDuration = 1.2f;
    private float spread = .5f;
    
    
    public GameObject tapToPlayButton;
    public GameObject nextLevelButton;
    public GameObject retryLevelButton;

    public GameObject tutorialRect;
    public GameObject mainMenuRect;
    public GameObject inGameRect;
    public GameObject finishRect;

    public Image levelSliderImage;

    public TextMeshProUGUI levelText;
    public TextMeshProUGUI coinText;

    public GameObject winPanel;
    public GameObject failPanel;

    [Header("Control Panel")] 
    [SerializeField] private CanvasGroup controlPanel;
    [SerializeField] private Button soundOpenButton;
    [SerializeField] private Button soundCloseButton;
    [SerializeField] private Button vibrationOpenButton;
    [SerializeField] private Button vibrationCloseButton;

    private bool _isControlPanelOpened = false;
    private void OnEnable()
    {
        ControlPanelsOnLevelChanged();
    }

    private void OnDisable()
    {
        ControlPanelsOnLevelChanged();
    }

    private void Awake()
    {
        mainMenuRect.SetActive(true);
        
        GameObject coin;

        for (int i = 0; i < _maxCoins; i++)
        {
            coin = Instantiate(_coinPrefab, transform.position, Quaternion.identity, transform);
            coin.SetActive(false);
            coinsQueue.Enqueue(coin);
        }
    }

    public void TapToPlayButtonClick()
    {
        GameManager.Instance.StartGame();
    }

    public void SetMoney()
    {
        coinText.text = "$" + PlayerPrefs.GetFloat("money");
    }
    public void MoneyAnim(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (coinsQueue.Count > 0)
            {
                GameObject coin = coinsQueue.Dequeue();
                coin.SetActive(true);

                // coin.transform.position = PlayerController.Instance.transform.GetChild(0).position
                //                          + new Vector3(
                //                              UnityEngine.Random.Range(-spread,
                //                                  spread),
                //                              UnityEngine.Random.Range(-spread,
                //                                  spread), 0);
                coin.transform.position = Camera.main.WorldToScreenPoint(PlayerController.Instance.transform.GetChild(0).position
                                                                         + new Vector3(
                                                                             UnityEngine.Random.Range(-spread,
                                                                                 spread),
                                                                             UnityEngine.Random.Range(-spread,
                                                                                 spread), 0));
                float duration = UnityEngine.Random.Range(minAnimDuration, maxAnimDuration);
                coin.transform.DOMove(TargetPos.position, duration).SetEase(Ease.InOutBack).SetUpdate(true).OnComplete(() =>
                {
                    coin.SetActive(false);
                    coinsQueue.Enqueue(coin);
                });
            }
        }
    }
    public void NextLevel()
    {
        StartCoroutine(NL());
        

    }

    IEnumerator NL()
    {
        LevelManager.Instance.IncreaseLevel();
        LevelManager.Instance.SetLevel();
        //SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        yield return new WaitForFixedUpdate();
        PlayerController.Instance.ResetPlayerController();
    }

    public void RestartGame()
    {
        LevelManager.Instance.SetLevel();
        PlayerController.Instance.ResetPlayerController();
        //SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);

        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void OpenFinishRect(bool isSuccess)
    {
        if (isSuccess)
        {
            failPanel.SetActive(false);
            winPanel.SetActive(true);
            nextLevelButton.SetActive(true);
        }
        else
        {
            failPanel.SetActive(true);
            winPanel.SetActive(false);
            retryLevelButton.SetActive(true);
        }

        inGameRect.SetActive(false);

        if (finishRect.TryGetComponent(out CanvasGroup finishCanvasGroup))
        {
            finishCanvasGroup.alpha = 0f;
            finishCanvasGroup.DOFade(1, 1.5f);    
        }
        

        finishRect.SetActive(true);
    }
    private void ControlPanelsOnLevelChanged()
    {
        mainMenuRect.SetActive(true);
        inGameRect.SetActive(false);
        finishRect.SetActive(false);
        
        winPanel.SetActive(false);
        failPanel.SetActive(false);
        
        retryLevelButton.SetActive(false);
        nextLevelButton.SetActive(false);
    }
    
    public void ControlPanelController(int index)
    {
        if (index == 1)
        {
            _isControlPanelOpened = true;
            controlPanel.gameObject.SetActive(true);
            controlPanel.DOFade(1, 0.5f);
        }
        else
        {
            controlPanel.DOFade(0, 0.5f).OnComplete(DisablePanel);
        }
    }

    private void DisablePanel()
    {
        controlPanel.alpha = 0;
        controlPanel.gameObject.SetActive(false);
        _isControlPanelOpened = false;
    }

    public void ControlSoundButtons(int buttonIndex)
    {
        if (buttonIndex == 1)
        {
            soundOpenButton.gameObject.SetActive(false);
            soundCloseButton.gameObject.SetActive(true);
            SoundManager.ControlSound();
        }
        else
        {
            soundCloseButton.gameObject.SetActive(false);
            soundOpenButton.gameObject.SetActive(true);
            SoundManager.ControlSound();
        }
    }
    public void ControlVibrationButtons(int buttonIndex)
    {
        if (buttonIndex == 1)
        {
            vibrationOpenButton.gameObject.SetActive(false);
            vibrationCloseButton.gameObject.SetActive(true);
            VibrationManager.ControlVibration();
        }
        else
        {
            vibrationCloseButton.gameObject.SetActive(false);
            vibrationOpenButton.gameObject.SetActive(true);
            VibrationManager.ControlVibration();
        }
    }
    public void ControlReplayButton()
    {
        DisablePanel();
        RestartGame();
        mainMenuRect.SetActive(true);
    }

    public void OpenPrivacyPolicyMenu()
    {
       // Elephant.ShowSettingsView();   
    }

}