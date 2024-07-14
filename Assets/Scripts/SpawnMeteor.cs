using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMeteor : MonoBehaviour
{

    [SerializeField]
    private GameObject _meteor;
    [SerializeField]
    private List<Transform> _spawnPositions = new();

    void Start()
    {
        JogaNaCabecaDele();
    }
    private void JogaNaCabecaDele()
    {
        int aleatorio = Random.Range(0, 4);
        Instantiate(_meteor, _spawnPositions[aleatorio].position , Quaternion.identity);
        Invoke("JogaNaCabecaDele", 1f);
    }
}
