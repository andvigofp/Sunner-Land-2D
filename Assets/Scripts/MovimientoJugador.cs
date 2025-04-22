using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    private Rigidbody2D rb2D;
    private Animator animacionPlayer;

    [Header("Movimiento")]
    private float movimientoHorizontal = 0f;
    [Range(1f, 10f)][SerializeField] private float velocidadDeMovimiento = 5f; // Velocidad ajustada con deslizador
    [Range(1f, 10f)][SerializeField] private float velocidadMaxima = 5f; // Limitar velocidad máxima con deslizador
    private bool mirandoDerecha = true;

    [Header("Salto")]
    [Range(5f, 20f)][SerializeField] private float fuerzaDeSalto = 10f; // Fuerza ajustada con deslizador
    [SerializeField] private LayerMask queEsSuelo;
    [SerializeField] private Transform controladorSuelo;
    [SerializeField] private Vector3 dimensionesCaja;
    private bool enSuelo;
    private bool salto = false;

    private void Start()
    {
        // Obtener componentes Rigidbody2D y Animator
        rb2D = GetComponent<Rigidbody2D>();
        animacionPlayer = GetComponent<Animator>();
    }

    private void Update()
    {
        // Detectar movimiento horizontal
        movimientoHorizontal = Input.GetAxisRaw("Horizontal");

        // Detectar salto solo si está en el suelo
        if (Input.GetButtonDown("Jump") && enSuelo)
        {
            salto = true;
        }

        // Actualizar animaciones según entrada del jugador
        ActualizarAnimaciones();
    }

    private void FixedUpdate()
    {
        // Verificar si el jugador está en el suelo
        enSuelo = Physics2D.OverlapBox(controladorSuelo.position, dimensionesCaja, 0f, queEsSuelo);

        // Manejar el movimiento y el salto
        Mover(movimientoHorizontal, salto);

        // Reiniciar el salto
        salto = false;
    }

    private void Mover(float mover, bool saltar)
    {
        // Calcular velocidad horizontal
        float nuevaVelocidadX = mover * velocidadDeMovimiento;

        // Limitar velocidad horizontal máxima
        nuevaVelocidadX = Mathf.Clamp(nuevaVelocidadX, -velocidadMaxima, velocidadMaxima);

        // Aplicar movimiento horizontal
        rb2D.linearVelocity = new Vector2(nuevaVelocidadX, rb2D.linearVelocity.y);

        // Cambiar dirección del personaje según el movimiento
        if (mover > 0 && !mirandoDerecha)
        {
            Girar();
        }
        else if (mover < 0 && mirandoDerecha)
        {
            Girar();
        }

        // Aplicar fuerza de salto si está en el suelo
        if (saltar)
        {
            rb2D.AddForce(new Vector2(0f, fuerzaDeSalto), ForceMode2D.Impulse);
        }
    }

    private void Girar()
    {
        // Cambiar la dirección del personaje
        mirandoDerecha = !mirandoDerecha;
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }

    private void ActualizarAnimaciones()
    {
        // Activar animación de correr solo si hay movimiento horizontal
        if (Mathf.Abs(movimientoHorizontal) > 0.01f && enSuelo)
        {
            animacionPlayer.SetBool("Correr", true);
        }
        else
        {
            animacionPlayer.SetBool("Correr", false);
        }

        // Activar animación de salto solo si no está en el suelo
        animacionPlayer.SetBool("Saltar", !enSuelo);
    }

    private void OnDrawGizmos()
    {
        // Dibujar el área de detección del suelo para depuración
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(controladorSuelo.position, dimensionesCaja);
    }
}