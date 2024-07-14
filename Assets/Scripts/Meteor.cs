using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Meteor : MonoBehaviour
{
    public Rigidbody2D playerRb;

    public GameObject MeteorDown;
    public Transform[] spawnPoints;
    public int totalMeteors = 15;
    [SerializeField]
    private Animator _anim;
    [SerializeField]
    private Rigidbody2D _meteorRB;

    private void Start()
    {
        playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }

    private void SpawnCoins()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            // Obt�m a posi��o do ponto de spawn
            Vector3 spawnPosition = spawnPoint.position;
            // Define a profundidade do eixo Z
            spawnPosition.z = -0.2f;

            Instantiate(MeteorDown, spawnPosition, Quaternion.identity);
        }
    }

    public void OnTriggerEnter2D(Collider2D HitKill)
    {
        {
            if (HitKill.gameObject.CompareTag("Player"))
            {
                HitKill.GetComponent<PlayerMovement>().Die();
                _meteorRB.constraints = RigidbodyConstraints2D.FreezeAll;
                _anim.SetBool("DestruirPedrinha", true);
                Destroy(gameObject, .5f);
            }           

            if (HitKill.gameObject.CompareTag("Ground"))
            {
                _meteorRB.constraints = RigidbodyConstraints2D.FreezeAll;
                _anim.SetBool("DestruirPedrinha", true);
                Destroy(gameObject, .5f);
            }
        }
    }
}