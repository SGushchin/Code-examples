using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] private Transform _spawnPos;

    public void Awake()
    {
        if (_spawnPos is null) throw new System.NullReferenceException("[Teleport] _spawnPos is null");
    }

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.transform.position = _spawnPos.position;
    }
}
