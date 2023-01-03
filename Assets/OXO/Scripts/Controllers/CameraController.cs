using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    public Transform target;
    public Vector3 offset;
    public bool isTapToPlay;
    
    [SerializeField] private float distanceToCurrentCircle;

    [SerializeField] private Vector3 _startPos;
    private void Start()
    {
        _startPos = transform.position;
    }

    public void SetCam(float height)
    {
        //transform.position = new Vector3(0, 46, -7.3f);
        transform.position = new Vector3(transform.position.x, height + distanceToCurrentCircle, transform.position.z);
    }
    public void SetCam()
    {
        transform.position = _startPos;
        //transform.position = new Vector3(transform.position.x, height + distanceToCurrentCircle, transform.position.z);
    }
    public void MoveCam(float distance)
    {
        //transform.DOMove(new Vector3(transform.position.x, transform.position.y + distance, transform.position.z), .2f).SetEase(Ease.Linear);
    }

    private void LateUpdate()
    {
        if (!isTapToPlay)
        {
            return;
        }
        var current = offset + target.position;
        if (current.y/* + .2f */> transform.position.y)
        {
            return;
        }

        current.x = 0;
        current.z = -7.3f;
        transform.position = current;
        
    }
}
