using System;
using System.Collections;
using UnityEngine;

public class PlayerObject : MonoBehaviour
{
    [SerializeField] private Transform _playerObj;

    private static float SPEED = 6.0f;
    private Vector3 _target;

    public void StartMove(Vector3 vector, float delay = 0.0f, Action onMoveCompleteCallback = null)
    {
        _target = vector;
        StartCoroutine(Move(delay, onMoveCompleteCallback));
    }

    private IEnumerator Move(float delay, Action onMoveCompleteCallback = null)
    {
        yield return new WaitForSeconds(delay);
        
        while (_playerObj.position != _target)
        {
            _playerObj.position = Vector3.MoveTowards(_playerObj.position, _target, Time.deltaTime * SPEED);
            yield return null;
        }

        onMoveCompleteCallback?.Invoke();
    }
}