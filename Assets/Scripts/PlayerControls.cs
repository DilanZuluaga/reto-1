using UnityEngine;
using Spine.Unity;

public class CharacterController : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset runWeapon;
    public AnimationReferenceAsset jumpAnimation;
    public AnimationReferenceAsset idleAnimation;
    public AnimationReferenceAsset attackAnimation; // Asignar en el Inspector

    public float speed = 10f;
    public float jumpForce = 10f;
    private bool isGrounded;
    private bool isAttacking;
    private float attackCooldown = 1f; // Duraci�n estimada de la animaci�n de ataque
    private float lastAttackTime; // Tiempo cuando se realiz� el �ltimo ataque

    private Rigidbody2D rb;

    private void Start()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        rb = GetComponent<Rigidbody2D>();

        // Aseg�rate de que los componentes necesarios est�n presentes.
        if (skeletonAnimation == null || rb == null)
        {
            Debug.LogError("Required component missing.");
        }

        // Establecer la orientaci�n inicial del personaje hacia la derecha
        skeletonAnimation.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");

        // Aplicar el movimiento horizontal
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

        // Si hay movimiento horizontal significativo
        if (Mathf.Abs(horizontal) > 0.1f)
        {
            // Si no estamos atacando, permitir cambiar la animaci�n y la direcci�n
            if (!isAttacking)
            {
                SetAnimation(runWeapon, true);
                FlipCharacter(horizontal);
            }
        }
        else if (Mathf.Abs(horizontal) < 0.1f && isGrounded && !isAttacking)
        {
            // Si no hay movimiento horizontal y el personaje est� en el suelo
            SetAnimation(idleAnimation, true);
        }

        // Salto
        if (Input.GetButtonDown("Jump") && isGrounded && !isAttacking)
        {
            Jump();
        }

        // Ataque
        if (Input.GetKeyDown(KeyCode.J) && !isAttacking && Time.time - lastAttackTime >= attackCooldown)
        {
            Attack();
        }

        // Permitir atacar de nuevo despu�s de que la animaci�n haya terminado
        if (isAttacking && Time.time - lastAttackTime >= attackCooldown)
        {
            isAttacking = false;
        }
    }

    private void FlipCharacter(float horizontal)
    {
        // Voltear el personaje para que mire en la direcci�n opuesta a la que se mueve
        if (horizontal > 0)
        {
            skeletonAnimation.transform.localScale = new Vector3(-1f, 1f, 1f); // Mover hacia la derecha, mirar hacia la izquierda
        }
        else if (horizontal < 0)
        {
            skeletonAnimation.transform.localScale = new Vector3(1f, 1f, 1f); // Mover hacia la izquierda, mirar hacia la derecha
        }
    }

    private void SetAnimation(AnimationReferenceAsset animation, bool loop)
    {
        if (skeletonAnimation.AnimationName != animation.name)
        {
            skeletonAnimation.state.SetAnimation(0, animation, loop);
        }
    }

    private void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        SetAnimation(jumpAnimation, false); // Asumiendo que la animaci�n de salto no debe hacer loop
        isGrounded = false; // Podr�as querer mejorar la detecci�n de si est� en el suelo
    }

    private void Attack()
    {
        SetAnimation(attackAnimation, false); // Asumiendo que la animaci�n de ataque no debe hacer loop
        isAttacking = true;
        lastAttackTime = Time.time;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}