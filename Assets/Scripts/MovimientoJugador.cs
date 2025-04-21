using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    private Rigidbody2D rb2D;
    private Animator animacionPlayer;

    [Header("Movimiento")]
    private float movimientoHorizontal = 0f;
    [SerializeField] private float velocidadDeMovimiento;
    [Range(0, 0.3f)][SerializeField] private float suavizadoMovimiento;

    private Vector3 velocidad = Vector3.zero;
    private bool mirandoDerecha = true;

    [Header("Salto")]
    [SerializeField] private float fuerzaDeSalto;
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
        // Control del movimiento horizontal
        movimientoHorizontal = Input.GetAxisRaw("Horizontal") * velocidadDeMovimiento;

        // Detectar acción de salto
        if (Input.GetButtonDown("Jump"))
        {
            salto = true;
        }

        // Actualizar animaciones
        ActualizarAnimaciones();
    }

    private void FixedUpdate()
    {
        // Verificar si el jugador está en el suelo
        enSuelo = Physics2D.OverlapBox(controladorSuelo.position, dimensionesCaja, 0f, queEsSuelo);

        // Manejar el movimiento
        Mover(movimientoHorizontal * Time.fixedDeltaTime, salto);

        salto = false;
    }

    private void Mover(float mover, bool saltar)
    {
        // Aplicar suavizado al movimiento horizontal
        Vector3 velocidadObjetivo = new Vector2(mover, rb2D.linearVelocity.y);
        rb2D.linearVelocity = Vector3.SmoothDamp(rb2D.linearVelocity, velocidadObjetivo, ref velocidad, suavizadoMovimiento);

        // Girar el personaje según la dirección del movimiento
        if (mover > 0 && !mirandoDerecha)
        {
            Girar();
        }
        else if (mover < 0 && mirandoDerecha)
        {
            Girar();
        }

        // Aplicar fuerza de salto si está en el suelo
        if (enSuelo && saltar)
        {
            enSuelo = false;
            rb2D.AddForce(new Vector2(0f, fuerzaDeSalto));
        }
    }

    private void Girar()
    {
        mirandoDerecha = !mirandoDerecha;
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }

    private void ActualizarAnimaciones()
    
    {
        // Detectar animaciones en función de linearVelocity y enSuelo
        animacionPlayer.SetBool("idle", Mathf.Abs(rb2D.linearVelocity.x) < 0.5f && enSuelo);
        animacionPlayer.SetBool("run", Mathf.Abs(rb2D.linearVelocity.x) > 0.5f && enSuelo);
        animacionPlayer.SetBool("jump", !enSuelo);

        // Depuración
        Debug.Log("linearVelocity.x: " + rb2D.linearVelocity.x);
        Debug.Log("Idle activado: " + animacionPlayer.GetBool("idle"));
    }


// Métodos personalizados (ajustar según la lógica del juego)

private bool EstaEscalando()
    {
        // Lógica para detectar escalada (puedes ajustarla según la mecánica del juego)
        return false;
    }

    private bool WallGrabDetectado()
    {
        // Lógica para detectar Wall Grab (agarrarse a la pared)
        return false;
    }

    private bool EstaMareado()
    {
        // Activar animación de mareo si es necesario
        return false; // Define tu lógica de mareo
    }

    private bool RecibiendoDano()
    {
        // Detectar daño (puedes usar una variable que se active cuando reciba daño)
        return false; // Cambia esto según tu lógica de recibir daño
    }

    private bool RecibiendoDanoSevero()
    {
        // Detectar daño más severo
        return false; // Ajusta según las mecánicas de tu juego
    }

    private bool Rodando()
    {
        // Detectar si está rodando
        return Input.GetKey(KeyCode.R); // Ejemplo: Usa la tecla "R" para rodar
    }

    private bool EsVictorioso()
    {
        // Detectar condición de victoria
        return false; // Ajusta según tu mecánica de victoria
    }

    private void OnDrawGizmos()
    {
        // Dibujar el área de detección del suelo en la escena (útil para depuración)
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(controladorSuelo.position, dimensionesCaja);
    }
}
