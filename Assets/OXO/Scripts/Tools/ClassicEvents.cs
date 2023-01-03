using UnityEngine;
using UnityEngine.Events;

public class ClassicEvents : MonoBehaviour
{
    #region Events
    public UnityEvent onAwakeEvent;
    
    public UnityEvent onEnableEvent;
    public UnityEvent onDisableEvent;
    #endregion
    private void Awake() => onAwakeEvent?.Invoke();
    private void OnEnable() => onEnableEvent?.Invoke();
    private void OnDisable() => onDisableEvent?.Invoke();
}
