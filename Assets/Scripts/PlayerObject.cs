using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerObject : MonoBehaviour
{
    [SerializeField] private Transform _playerObj;

    private static float SPEED = 6.0f;
    private Vector3 _target;

    public void StartMove(Vector3 vector, bool shouldWait)
    {
        _target = vector;
        StartCoroutine(Move(shouldWait));
    }

    private IEnumerator Move(bool shouldWait)
    {
        if (shouldWait)
        {
            yield return new WaitForSeconds(0.75f);
        }
    
        while (_playerObj.position != _target)
        {
            _playerObj.position = Vector3.MoveTowards(_playerObj.position, _target, Time.deltaTime * SPEED);
            yield return null;
        }
    }
}