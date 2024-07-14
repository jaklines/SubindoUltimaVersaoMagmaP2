using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float velocidade = 5;
    [SerializeField] private float alturaDoPulo = 10;
    public LayerMask groundLayer;
    public LayerMask wallLayer;

    private Rigidbody2D playerRb;
    private Animator anim;
    [SerializeField]
    private Transform castPoint;
    [SerializeField]
    private LayerMask layerPotion;

    private bool hasShield = false;
    private float shieldEndTime;

    public Material rainbowMaterial;
    private Material originalMaterial;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        if (spriteRenderer != null)
        {
            originalMaterial = spriteRenderer.material;
            Debug.Log("Start: Material original definido");
        }
        else
        {
            Debug.LogError("SpriteRenderer não encontrado no GameObject 'Player' ou seus filhos.");
        }
    }

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0 && transform.position.y > collision.transform.position.y)
        {
            Pular();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & wallLayer) != 0)
        {
            Vector2 collisionPoint = collision.contacts[0].point;
            Vector2 playerPosition = transform.position;
            Vector2 oppositeDirection = (playerPosition - collisionPoint).normalized;
            playerRb.AddForce(oppositeDirection * velocidade, ForceMode2D.Impulse);
        }
    }

    private void Pular()
    {
        playerRb.velocity = Vector2.up * alturaDoPulo;
    }

    public void Die()
    {
        if (hasShield)
        {
            Debug.Log("Usou a Pot");
            hasShield = false;
            if (spriteRenderer != null)
            {
                spriteRenderer.material = originalMaterial;
            }
        }
        else
        {
            Destroy(gameObject);
            SceneManager.LoadScene("GameOverScene");
        }
    }

    public void ActivateShield()
    {
        Debug.Log("ActivateShield chamado");
        if (spriteRenderer != null)
        {
            spriteRenderer.material = rainbowMaterial;
            Debug.Log("Material arco-íris aplicado");
        }
        hasShield = true;
        shieldEndTime = 20f;
        Debug.Log("Escudo ativado por 20 segundos");
    }

    private void Update()
    {
        if (hasShield)
        {
            shieldEndTime -= Time.deltaTime;
            if (shieldEndTime <= 0)
            {
                Debug.Log("Acabou a Pot");
                hasShield = false;
                if (spriteRenderer != null)
                {
                    spriteRenderer.material = originalMaterial;
                }
            }
        }

        float movimentoHorizontal = Input.GetAxis("Horizontal");
        playerRb.velocity = new Vector2(movimentoHorizontal * velocidade, playerRb.velocity.y);

        if (playerRb.velocity.y > alturaDoPulo)
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, alturaDoPulo);
        }

        RaycastHit2D hit = Physics2D.Raycast(castPoint.position, Vector2.down, 20, layerPotion);
        Debug.DrawRay(castPoint.position, Vector2.down * 20, Color.red);

        if (hit.collider != null)
        {
            Debug.Log("Poção detectada");
            hit.collider.gameObject.GetComponent<GarrafaPoderosa>().CrackB();
            ActivateShield(); // Ativa o escudo ao detectar a poção
            Destroy(hit.collider.gameObject); // Destroi a poção após o uso
        }

        // Força a aplicação do material no Update
        if (spriteRenderer != null)
        {
            if (hasShield)
            {
                spriteRenderer.material = rainbowMaterial;
            }
            else
            {
                spriteRenderer.material = originalMaterial;
            }
        }
    }
}