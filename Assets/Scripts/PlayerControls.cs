using UnityEngine;
using Spine.Unity;

public class CharacterController : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset runWeapon;
    public AnimationReferenceAsset jumpAnimation; // Referencia para la animaci�n de salto

    public float speed = 10f;
    public float jumpForce = 10f;
    private bool isGrounded; // Asumimos que hay una manera de verificar si el personaje est� en el suelo

    private Rigidbody2D rb; // Referencia al componente Rigidbody2D

    private void Start()
    {
        // Aseg�rate de que el componente SkeletonAnimation est� asignado
        if (skeletonAnimation == null)
            skeletonAnimation = GetComponent<SkeletonAnimation>();

        // Obtener el componente Rigidbody2D
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D no est� presente en el GameObject.");
        }

    }

    private void Update()
    {
        // Movimiento horizontal
        float horizontal = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(horizontal, 0);
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

        // Reproducir animaci�n de correr con arma
        if (Mathf.Abs(horizontal) > 0.1f) // Verifica si hay movimiento horizontal significativo
        {
            SetAnimation(runWeapon, true);
        }

        // Verificar si se presiona la tecla de salto y el personaje est� en el suelo
        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log("Intento de salto detectado");
            if (isGrounded)
            {
                Debug.Log("Salto ejecutado");
                Jump();
            }
            else
            {
                Debug.Log("El personaje no est� en el suelo");
            }
        }
    }

    private void SetAnimation(AnimationReferenceAsset animation, bool loop)
    {
        // Cambia la animaci�n solo si es diferente a la actual
        if (skeletonAnimation.AnimationName != animation.name)
        {
            skeletonAnimation.state.SetAnimation(0, animation, loop);
        }
    }

    private void Jump()
    {
        Debug.Log($"Saltando con fuerza de: {jumpForce}");
        // A�adir una fuerza hacia arriba para simular el salto
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        // Reproducir la animaci�n de salto
        SetAnimation(jumpAnimation, false); // El salto no se repite en bucle

        isGrounded = false; // El personaje ya no est� en el suelo
    }

    // M�todo para verificar si el personaje ha vuelto al suelo
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Aseg�rate de que el objeto con el que colisiona es el terreno
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }


}