using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class PopUpController : Singleton<PopUpController>
{
    [SerializeField] private int maxPopUp;
    [SerializeField] private TextMeshPro popUpPrefab;

    private Queue<TextMeshPro> popUpsQueue = new Queue<TextMeshPro>();
        
    private float _minAnimDuration = 1f;
    private float _maxAnimDuration = 1.1f;

    private float _spread = .1f;
    public void Start()
    {
        TextMeshPro popUp;

        for (int i = 0; i < maxPopUp; i++)
        {
            popUp = Instantiate(popUpPrefab, transform.position, Quaternion.identity, transform);
            popUp.gameObject.SetActive(false);
            popUpsQueue.Enqueue(popUp);
        }
    }
    
    public void AnimatePopUp(Vector3 playerPosition, float number)
    {
        if (popUpsQueue.Count > 0)
        {
            TextMeshPro popUp = popUpsQueue.Dequeue();
            popUp.gameObject.SetActive(true);
            if (number > 0)
            {
                popUp.text = "+" + number;
                popUp.color = Color.green;
            }
            else
            {
                popUp.text = number.ToString();
                popUp.color = Color.red;
            }
            
            var position = playerPosition
                           + new Vector3(
                               UnityEngine.Random.Range(-_spread,
                                   _spread),
                               UnityEngine.Random.Range(-_spread,
                                   _spread), 0);
            popUp.transform.position = position;
            // coin.transform.position = Camera.main.WorldToScreenPoint(balloonPosition
            //                                                          + new Vector3(
            //                                                              UnityEngine.Random.Range(-spread,
            //                                                                  spread),
            //                                                              UnityEngine.Random.Range(-spread,
            //                                                                  spread), 0));
            float duration = UnityEngine.Random.Range(_minAnimDuration, _maxAnimDuration);
            popUp.transform.DOMove(new Vector3(position.x, position.y + .7f, position.z), duration).SetEase(Ease.InQuad).SetUpdate(true).OnComplete(() =>
            {
                popUp.gameObject.SetActive(false);
                popUpsQueue.Enqueue(popUp);
            });
        }
    }
}