using System;
using System.Collections.Generic;
using DG.Tweening;
using Lean.Touch;
using Managers.Vibration;
using MoreMountains.NiceVibrations;
using MText;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
	public bool move;
	public float startNumber;
	public float circlePassGain = 1;
	public float money;
	[SerializeField] private ParticleSystem multiplierEffect;
	[SerializeField] private ParticleSystem splashEffect;
	[SerializeField] float jumpStrength = 100;
	[SerializeField] float gravityForce = 10;

	[SerializeField] private Modular3DText modular3DText;
	[SerializeField] private CameraController cam;
	
	[SerializeField] private GameObject feverTrail;
	
	private ParticleSystem _multiplierEffect;
	
	private List<float> _angleList = new List<float>();
	private Queue<ParticleSystem> _splashQueue = new Queue<ParticleSystem>();

	private int _skippedCounter;
	private float _nextBallPosToJump;
	private float _verticalDistance;
	private float _circleAmount;
	private float _topPoint;
	private float _radiusOfNumText;
	private float _vel;
	private float _maxDistance;

	private bool _isPlayerDead;
	private bool _isFever;

	private void Awake()
	{
		//_angleList.Add(0);
		_angleList.Add(90);
		//_angleList.Add(180);
		_angleList.Add(270);
		money = PlayerPrefs.GetFloat("money");
	}

	private void Start()
	{
		DOTween.KillAll();
		_isFever = false;
		_isPlayerDead = false;
		cam.target = transform;
		_verticalDistance = CircleManager.Instance.verticalDistance;
		_circleAmount = CircleManager.Instance.circleAmount;
		_topPoint = _verticalDistance * (_circleAmount - 1);
		_nextBallPosToJump = _topPoint + GetComponent<SphereCollider>().bounds.size.y / 2 ;
		_radiusOfNumText = modular3DText.circularAlignmentRadius;
		_maxDistance = _verticalDistance / 2;

		cam.SetCam(_topPoint);
		for (int i = 0; i < 4; i++)
		{
			var splash = Instantiate(splashEffect);
			splash.gameObject.SetActive(false);
			_splashQueue.Enqueue(splash);
		}
		move = true;
		modular3DText.text = startNumber.ToString();
	}

	public void ResetPlayerController()
	{
		DOTween.KillAll();
		_verticalDistance = CircleManager.Instance.verticalDistance;
		_circleAmount = CircleManager.Instance.circleAmount;
		_topPoint = _verticalDistance * (_circleAmount - 1);
		_isFever = false;
		feverTrail.SetActive(false);
		_isPlayerDead = false;
		cam.isTapToPlay = false;
		//cam.SetCam();
		cam.offset.y = 4;
		
		transform.position = new Vector3(0, transform.position.y, 0);
		transform.localScale = Vector3.one;
		for (int i = 0; i < 4; i++)
		{
					ParticleSystem splashH = _splashQueue.Dequeue();
					if (splashH != null)
					{
						Destroy(splashH.gameObject);
					}
		}
			
		_splashQueue.Clear();

		for (int i = 0; i < 4; i++)
		{
			var splash = Instantiate(splashEffect);
			splash.gameObject.SetActive(false);
			_splashQueue.Enqueue(splash);
		}
		
		_nextBallPosToJump = _topPoint + GetComponent<SphereCollider>().bounds.size.y / 2 ;
		_radiusOfNumText = modular3DText.circularAlignmentRadius;
		_maxDistance = _verticalDistance / 2;
		move = true;
		startNumber = 1;
		LeanTouch.Instance.UseMouse = true;
		LeanTouch.Instance.UseTouch = true;
		
		// CircleManager.Instance.cylinder.transform.eulerAngles = Vector3.zero;
		cam.SetCam(_topPoint);

		modular3DText.text = startNumber.ToString();
		Debug.Log(CircleManager.Instance.name);
	}

	public void TapToPlay()
	{
		cam.SetCam(_topPoint);
		cam.isTapToPlay = true;
		_multiplierEffect = Instantiate(multiplierEffect);
		_multiplierEffect.gameObject.SetActive(false);

		if (_splashQueue.Count == 0)
		{
			for (int i = 0; i < 4; i++)
			{
				var splash = Instantiate(splashEffect);
				splash.gameObject.SetActive(false);
				_splashQueue.Enqueue(splash);
			}
		}
		
		move = true;
		
		modular3DText.text = startNumber.ToString();
	}
	// void Update() {
	// 	Debug.DrawLine(new Vector3(transform.position.x,transform.position.y, transform.position.z - `f), new Vector3(transform.position.x,transform.position.y, transform.position.z - 1.8f) + Vector3.down * 3 / 2, Color.blue);
	// }

	void FixedUpdate() 
	{
		if (!move)
		{
			//SplashEffect();
			return;
		}

		if (_vel > -.25f)
		{
			_vel -= gravityForce * Time.fixedDeltaTime;
		}
		else
		{
			_vel = -.25f;
		}

		for (int i = 0; i < _angleList.Count; i++)
		{
			CheckCollectableCollision(_angleList[i]);
		}

		float overlap = _nextBallPosToJump - (transform.position.y + _vel);
		if (overlap >= 0) 
		{
			transform.Translate(Vector3.up * (_vel + overlap));
			
			CheckCollision();
		}
		transform.Translate(Vector3.up * _vel);
	}

	void ChainJump(Vector3 pos)
	{
		transform.DOMove(
			new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z),
			.5f).SetEase(Ease.OutCirc).OnComplete((() =>
		{
			transform.DOMove(pos, .5f).SetEase(Ease.InCirc).OnComplete((() =>
			{
				ChainJump(pos);
			}));
		}));
	}
	void CheckCollectableCollision(float angle)
	{
		Transform transform1;
		var direction = Quaternion.AngleAxis(angle, transform.up) * (transform1 = transform).forward;
		
		Debug.DrawRay(new Vector3(transform.position.x, transform.position.y, transform.position.z - _radiusOfNumText), direction, Color.red, 5);
		
		if (Physics.Raycast(
			    new Vector3(transform.position.x, transform.position.y, transform.position.z - _radiusOfNumText),
			    Vector3.down, out var hit, 
			    .35f,
			    LayerMask.GetMask("Collectable")))
		{
			//Jump();
			float num = hit.collider.transform.parent.GetComponent<CollectableController>().Hit();
			//hit.collider.gameObject.SetActive(false);
			CalculatePlayerNum(num);
			_skippedCounter = 0;
			//SplashEffect(hit);
		}
	}
	void CheckCollision() {
		if (Physics.Raycast(new Vector3(transform.position.x,transform.position.y, transform.position.z - _radiusOfNumText), Vector3.down, out var hit, _maxDistance,
			    LayerMask.GetMask("Platform"))) 
		{
			if (hit.collider.CompareTag("FinishPlatform")) 
			{
				Stop();
				transform.DOMove(
					new Vector3(0, hit.collider.transform.position.y + .5f,
						0), .2f).SetEase(Ease.Linear);
				Destroy(_multiplierEffect.gameObject);
				GameManager.Instance.LevelComplete();
				VibrationManager.PlayVibration(HapticTypes.Success);
				CanvasManager.Instance.MoneyAnim(_skippedCounter);
				_skippedCounter = 0;
				SplashEffect(hit);
			}
			else if (hit.collider.CompareTag("Finish")) 
			{
				//MoveToNextChainJump();
				//cam.MoveCam(-_verticalDistance - 1.5f);
				// cam.offset.y = 4;
				DOTween.To(()=> cam.offset.y, x=> cam.offset.y = x, 4, .3f);
				
				feverTrail.SetActive(false);
				// transform.position = new Vector3(0, transform.position.y, 0);
				_nextBallPosToJump -= _verticalDistance;
				_isFever = false;
			}
			else if (_isFever)
			{
				hit.collider.GetComponentInParent<CircleController>().RemoveCircle();
			}
			else if (hit.collider.CompareTag("Chain")) 
			{
				if (hit.collider.GetComponentInParent<ChainPlatformController>().HitToChain(startNumber))
				{
					//MoveToNextChainJump();
					//cam.MoveCam(-_verticalDistance);
					VibrationManager.PlayVibration(HapticTypes.Selection);
					_nextBallPosToJump -= _verticalDistance;
					++_skippedCounter;
					money += 100;
					PlayerPrefs.SetFloat("money", money);
					CanvasManager.Instance.SetMoney();
					
					/*_multiplierEffect.transform.position = new Vector3(transform.position.x, transform.position.y - (_verticalDistance / 2),
						transform.position.z - _radiusOfNumText);
					_multiplierEffect.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "X" + (_skippedCounter);
					_multiplierEffect.gameObject.SetActive(true);
					_multiplierEffect.Play();*/
				}
				else
				{
					Stop();
					money += 100;
					PlayerPrefs.SetFloat("money", money);
					CanvasManager.Instance.SetMoney();
					CanvasManager.Instance.MoneyAnim(_skippedCounter);
					Transform transform1;
					Vector3 pos = new Vector3(0, (transform1 = hit.collider.transform).position.y + .5f,
						transform1.position.z + _radiusOfNumText);
					transform.DOMove(
						pos, .2f).SetEase(Ease.Linear).OnComplete((() =>
					{

						transform.DOMove(
							new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z),
							.5f).SetEase(Ease.OutCirc).OnComplete((() =>
						{
							transform.DOMove(pos, .5f).SetEase(Ease.InCirc).OnComplete((() =>
							{
								ChainJump(pos);
							}));
						}));
						
					}));
					

					/*Transform transform1;
					Vector3 pos = new Vector3(0, (transform1 = hit.collider.transform).position.y + .5f,
						transform1.position.z + _radiusOfNumText);
					transform.DOMove(
						pos, .2f).SetEase(Ease.Linear).OnComplete((() =>
					{
						DOTween.Sequence()
							.Append(
								transform.DOMove(
									new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z),
									1)
								.OnComplete((() =>
								{
									transform.DOMove(new Vector3(0, (transform1 = hit.collider.transform).position.y,
										transform1.position.z + _radiusOfNumText), .2f).SetEase(Ease.Linear);
								}))).SetLoops(-1);

					}));*/

					//transform.DOPunchScale(new Vector3(.5f, -.5f, 0), .5f);
					//Destroy(_multiplierEffect.gameObject);
					/*_vel = jumpStrength;
					transform.DOPunchScale(new Vector3(.5f, -.5f, 0), .5f);
					_nextBallPosToJump -= .5f;*/
					GameManager.Instance.LevelComplete();
					VibrationManager.PlayVibration(HapticTypes.Success);

					_skippedCounter = 0;
					//hit.collider.gameObject.tag = "Normal";
					//SplashEffect(hit);
					//Jump();
					SplashEffectChain(hit);
				}

			}
			else if (_skippedCounter >= 3) 
			{
				
				feverTrail.SetActive(false);
				hit.collider.GetComponentInParent<CircleController>().RemoveCircle();
				_skippedCounter = 0;
				Jump();
				SplashEffect(hit);
			}
			else if (hit.collider.CompareTag("Normal")) 
			{
				/*if (skippedCounter >= 2) {
					//Apply good-looking break force.
					Destroy(hit.collider.gameObject);
				}*/

				_skippedCounter = 0;
				Jump();
				SplashEffect(hit);
			}
			else if (hit.collider.CompareTag("Bad")) 
			{
				Jump();
				float num = hit.collider.GetComponent<Piece>().number;
				CalculatePlayerNum(num);
				VibrationManager.PlayVibration(HapticTypes.MediumImpact);

				if (!_isPlayerDead)
				{
					hit.collider.gameObject.GetComponent<PieceController>().Hit();
				}

				_skippedCounter = 0;
				SplashEffect(hit);
			}
			else if (hit.collider.CompareTag("Good")) 
			{
				Jump();
				float num = hit.collider.GetComponent<Piece>().number;
				hit.collider.gameObject.GetComponent<PieceController>().Hit();
				CalculatePlayerNum(num);
				

				_skippedCounter = 0;
				SplashEffect(hit);
			}
			else if (hit.collider.CompareTag("Collectable")) 
			{
				Jump();
				float num = hit.collider.GetComponent<CollectableController>().Hit();
				VibrationManager.PlayVibration(HapticTypes.Selection);

				//hit.collider.gameObject.SetActive(false);
				CalculatePlayerNum(num);
				_skippedCounter = 0;
				//SplashEffect(hit);
			}
		}
		else
		{
			CircleManager.Instance.CirclePassed();
			VibrationManager.PlayVibration(HapticTypes.Selection);
			
			cam.MoveCam(-_verticalDistance);
			
			_nextBallPosToJump -= _verticalDistance;
			++_skippedCounter;
			if (_skippedCounter >= 3)
			{
				feverTrail.SetActive(true);
			}
			// _multiplierEffect.transform.position = new Vector3(transform.position.x, transform.position.y /*- (_verticalDistance / 2)*/,
			// 	transform.position.z - _radiusOfNumText);
			// _multiplierEffect.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "X" + (_skippedCounter);
			// _multiplierEffect.gameObject.SetActive(true);
			// _multiplierEffect.Play();
			
			
			CalculatePlayerNum();
		}
	}

	void Jump() 
	{
		_vel = jumpStrength;
		transform.DOPunchScale(new Vector3(.5f, -.5f, 0), .5f);
	}

	/*private void MoveToNextChainJump() 
	{
		_vel = jumpStrength / 2;
		transform.DOPunchScale(new Vector3(.5f, -.5f, 0), .5f);
	}*/
	void Stop() 
	{
		move = false;
		_vel = 0;
	}

	private void SplashEffect(RaycastHit hit)
	{
		foreach (Transform child in transform)
		{
			ParticleSystem splash = _splashQueue.Dequeue();
			
			splash.Clear();
			splash.gameObject.SetActive(true);
			Transform transform1;
			(transform1 = splash.transform).SetParent(hit.transform);
			var position = child.position;
			transform1.position = new Vector3(position.x, hit.point.y + .01f, position.z);
			transform1.localScale = Vector3.one * .18f;
			splash.Play();
			_splashQueue.Enqueue(splash);
		}
	}
	private void SplashEffectChain(RaycastHit hit)
	{
		foreach (Transform child in transform)
		{
			ParticleSystem splash = _splashQueue.Dequeue();
			
			splash.Clear();
			splash.gameObject.SetActive(true);
			Transform transform1;
			(transform1 = splash.transform).SetParent(hit.transform);
			transform1.localScale = Vector3.one * .18f;
			transform1.localPosition = new Vector3(0, .1f, 0);
			splash.Play();
			_splashQueue.Enqueue(splash);
		}
	}
	/*private void SplashEffect()
	{
		if (_splashQueue.Count > 0)
		{
			ParticleSystem splash = _splashQueue.Dequeue();
			splash.gameObject.SetActive(true);
			splash.transform.position = new Vector3(transform.position.x, transform.position.y + .01f,
				transform.position.z - _radiusOfNumText);
			splash.transform.SetParent(transform);
			splash.Play();
			_splashQueue.Enqueue(splash);
		}
	}*/
	private void CalculatePlayerNum()
	{
		startNumber += circlePassGain * _skippedCounter;
		PopUpController.Instance.AnimatePopUp(new Vector3(transform.position.x, transform.position.y - .5f, transform.position.z - _radiusOfNumText), circlePassGain * _skippedCounter);
		SetDigitSpread();
	}
	private void CalculatePlayerNum(float num)
	{
		if (_skippedCounter == 0)
		{
			_skippedCounter = 1;
		}
		float amount = num;
		startNumber += amount;
		var transform1 = transform;
		var position = transform1.position;
		PopUpController.Instance.AnimatePopUp(new Vector3(position.x, position.y + 2f, position.z - _radiusOfNumText), amount);
		
		if (startNumber <= 0)
		{
			_isPlayerDead = true;
			modular3DText.text = "";
			Stop();
			Destroy(_multiplierEffect.gameObject);
			GameManager.Instance.LevelFail();
			VibrationManager.PlayVibration(HapticTypes.Failure);

			return;
		}

		SetDigitSpread();
	}

	private void SetDigitSpread()
	{
		int number = (int)startNumber;
		int counter = 0;
		while (number > 0)
		{
			number /= 10;
			counter++;
		}

		
		if (counter == 2 || counter == 1)
		{
			modular3DText.circularAlignmentSpreadAmount = 20;
		}
		else
		{
			modular3DText.circularAlignmentSpreadAmount = 30;
		}
		modular3DText.text = startNumber.ToString();

		if (startNumber >= 1000)
		{
			_isFever = true;
			_maxDistance = _verticalDistance;
		}
	}
}