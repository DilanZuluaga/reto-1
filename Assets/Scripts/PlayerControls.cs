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
        private float attackCooldown = 1f; // Duración estimada de la animación de ataque
        private float lastAttackTime; // Tiempo cuando se realizó el último ataque

        private Rigidbody2D rb;

        private void Start()
        {
            skeletonAnimation = GetComponent<SkeletonAnimation>();
            rb = GetComponent<Rigidbody2D>();

            // Asegúrate de que los componentes necesarios están presentes
            if (skeletonAnimation == null || rb == null)
            {
                Debug.LogError("Required component missing.");
            }

            // Establecer la orientación inicial del personaje hacia la derecha
            skeletonAnimation.transform.localScale = new Vector3(1f, 1f, 1f);
        }

        private void Update()
        {
            float horizontal = Input.GetAxis("Horizontal");
            bool isMovingHorizontally = Mathf.Abs(horizontal) > 0.1f;

            // Aplicar el movimiento horizontal
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

            // Si hay movimiento horizontal y el personaje está en el suelo
            if (isMovingHorizontally && isGrounded)
            {
                // Si no estamos atacando, permitir cambiar la animación y la dirección
                if (!isAttacking)
                {
                    SetAnimation(runWeapon, true);
                    FlipCharacter(horizontal);
                }
            }
            // Si no se está moviendo horizontalmente, está en el suelo y no está atacando, poner animación de idle
            else if (!isMovingHorizontally && isGrounded && !isAttacking)
            {
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

            // Permitir atacar de nuevo después de que la animación haya terminado
            if (isAttacking && Time.time - lastAttackTime >= attackCooldown)
            {
                isAttacking = false;
            }
        }

        private void FlipCharacter(float horizontal)
        {
            // Voltear el personaje para que mire en la dirección opuesta a la que se mueve
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
            SetAnimation(jumpAnimation, false); // Activar la animación de salto
            isGrounded = false;
        }

        private void Attack()
        {
            SetAnimation(attackAnimation, false); // Asumiendo que la animación de ataque no debe hacer loop
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

