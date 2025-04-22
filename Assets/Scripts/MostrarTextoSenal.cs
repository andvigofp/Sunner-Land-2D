using UnityEngine;
using TMPro;

public class MostrarTextoConTransparencia : MonoBehaviour
{
    [Header("Configuraci�n")]
    public TextMeshProUGUI textoUI; // Texto de la interfaz
    public float retrasoSalida = 1.0f; // Retraso antes de ocultar el texto

    private bool enRangoDeSenal = false; // Verifica si el jugador est� cerca de la se�al

    private void Start()
    {
        // Asegurarse de que el texto sea completamente transparente al inicio
        if (textoUI != null)
        {
            Color color = textoUI.color;
            color.a = 0; // Alpha en 0 (totalmente transparente)
            textoUI.color = color;
        }
    }

    private void Update()
    {
        // Mostrar el texto cuando el jugador pulsa "E"
        if (enRangoDeSenal && Input.GetKeyDown(KeyCode.E))
        {
            MostrarTexto(); // Muestra el texto con opacidad
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Detectar cuando el jugador entra en el rango de la se�al
        if (collision.CompareTag("Player"))
        {
            enRangoDeSenal = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Detectar cuando el jugador sale del rango de la se�al
        if (collision.CompareTag("Player"))
        {
            enRangoDeSenal = false;
            OcultarTexto(); // Oculta el texto al salir del rango
        }
    }

    private void MostrarTexto()
    {
        if (textoUI != null)
        {
            // Hacer visible el texto (opacidad completa)
            Color color = textoUI.color;
            color.a = 1; // Alpha en 1 (totalmente visible)
            textoUI.color = color;

            // Programar la ocultaci�n con un retraso
            Invoke("OcultarTexto", retrasoSalida);
        }
    }

    private void OcultarTexto()
    {
        if (textoUI != null)
        {
            // Hacer transparente el texto
            Color color = textoUI.color;
            color.a = 0; // Alpha en 0 (totalmente transparente)
            textoUI.color = color;
        }
    }
}

