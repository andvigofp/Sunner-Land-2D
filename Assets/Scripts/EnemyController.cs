using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Configuración del Enemigo")]
    public Transform player; // Referencia al Transform del jugador
    public float detectionRadius = 5.0f; // Radio de detección
    public float speed = 2.0f; // Velocidad de movimiento

    private Rigidbody2D rb;

    // Start se ejecuta una vez antes del primer Update
    void Start()
    {
        // Obtener el Rigidbody2D del enemigo
        rb = GetComponent<Rigidbody2D>();
    }

    // Update se ejecuta cada cuadro
    void Update()
    {
        // Calcular la distancia al jugador
        float distancePlayer = Vector2.Distance(transform.position, player.position);

        // Si el jugador está dentro del radio de detección, moverse hacia él
        if (distancePlayer < detectionRadius)
        {
            MoverHaciaJugador();
        }
    }

    private void MoverHaciaJugador()
    {
        // Mover el enemigo hacia el jugador suavemente
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Si el enemigo colisiona con el jugador, inflige daño
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 direccionDanio = new Vector2(transform.position.x, 0);

            // Llamar al método RecibeDanio en el jugador
            collision.gameObject.GetComponent<PlayerController>().RecibeDanio(direccionDanio, 1);
        }
    }

    // Dibujar el radio de detección en la escena
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
