using UnityEngine;
using UnityEngine.Video;

public class Player : MonoBehaviour {

    /*public Vector2 Speed;
    public Vector2 JumpForce;*/
    [Range(1, 10)]
    public float speed = 3f;
    [Range(100, 600)]
    public float jumpForce = 333f;
    //    public float MaxSpeed = 2f;

    public Vector2 velocity;

    Rigidbody2D RB2;
    Animator animator;
    public VideoPlayer videoPlayer;
    public Transform resetPoint;
    public Animator ocu;

    float initScaleX;

    // Use this for initialization
    void Start() {
        RB2 = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        initScaleX = transform.localScale.x;
    }

    void FixedUpdate() {
        /**
            Burayı tamamen kaldırıp, biraz daha stabil hale aşağıda getirdim.
            AddForce yerine, tuşa basılı tutulduğu için doğrudan velocity ile
            hızı değiştirdim. Float kontrolleri için de Equals kullandım, ki
            daha doğru bir kontrol olsun. Animator'daki eksikleri de giderdim.
            Şu anda birbirleri arasında geçebiliyorlar. Has Exit Time'ları ve
            animasoynların kendilerine geçişlerini kapattım.
         */
        /*RB2.AddForce(Speed * Input.GetAxis("Horizontal"));
        RB2.velocity = Vector2.ClampMagnitude(RB2.velocity, MaxSpeed);
        velocity = RB2.velocity;

        if(Input.GetAxis("Horizontal") > 0f) {
            transform.localScale = new Vector3(
                    -transform.localScale.x,
                    transform.localScale.y,
                    transform.localScale.z
            );
        } else if(Input.GetAxis("Horizontal") < 0f) {
            transform.localScale = new Vector3(
                    transform.localScale.x,
                    transform.localScale.y,
                    transform.localScale.z
            );
        }

        if(RB2.velocity.x != 0 && RB2.velocity.y == 0) {
            animator.SetBool("run_state", true);
            animator.SetBool("idle_state", false);
            animator.SetBool("jump_state", false);
        } else if(RB2.velocity.y != 0) {
            animator.SetBool("run_state", false);
            animator.SetBool("idle_state", false);
            animator.SetBool("jump_state", true);
        } else {
            animator.SetBool("run_state", false);
            animator.SetBool("idle_state", true);
            animator.SetBool("jump_state", false);
        }*/

        //  Hızı belirleyelim
        RB2.velocity = new Vector2(speed * Input.GetAxis("Horizontal"), RB2.velocity.y);

        // Sadece yerdeyken zıplayabilelim
        if(Input.GetButtonDown("Jump") && RB2.velocity.y == 0f) {
            RB2.AddForce(Vector2.up * jumpForce);
        }

        //  Eğer karakterimiz yumruk atmıyorsa animasyon kontrolünü gerçekleştirelim
        if(!animator.GetCurrentAnimatorStateInfo(0).IsName("punch")) {

            if(!RB2.velocity.y.Equals(0f)) {
                //  Karakter Havada Değil
                animator.SetBool("run_state", false);
                animator.SetBool("idle_state", false);
                animator.SetBool("jump_state", true);
            } else if(RB2.velocity.y.Equals(0f) && RB2.velocity.x.Equals(0f)) {
                //  Karakter Duruyor
                animator.SetBool("run_state", false);
                animator.SetBool("idle_state", true);
                animator.SetBool("jump_state", false);
            } else if(RB2.velocity.y.Equals(0f) && !RB2.velocity.x.Equals(0f)) {
                //  Karakter yürüyor
                animator.SetBool("run_state", true);
                animator.SetBool("idle_state", false);
                animator.SetBool("jump_state", false);
            }
        } else {
            //  Karakter yumruk atmakta
            animator.SetBool("run_state", false);
            animator.SetBool("idle_state", false);
            animator.SetBool("jump_state", false);
        }

        if(Input.GetAxis("Horizontal") > 0f) {
            //  Sola gidiyoruz
            transform.localScale = new Vector3(
                    -initScaleX,
                    transform.localScale.y,
                    transform.localScale.z
            );
        } else if(Input.GetAxis("Horizontal") < 0f) {
            //  Sağa gidiyoruz
            transform.localScale = new Vector3(
                    initScaleX,
                    transform.localScale.y,
                    transform.localScale.z
            );
        }

        //  Yumruk tuşuna (F) mı basıldı?
        if(Input.GetButtonDown("Punch")) {
            animator.SetTrigger("punch_trigger");
        }

    }

    //  Trigger alanına girildi
    void OnTriggerEnter2D(Collider2D c2d) {
        if(c2d.name == "Kalp") {
            //  Kalp alanına girdik, videoyu oynatalım
            videoPlayer.Play();
        } else if(c2d.name == "Diken") {
            //  Dikenlere düştük, karakteri canlandıralım
            transform.position = resetPoint.position;
        }
    }

    //  Trigger alanı içindeyiz
    void OnTriggerStay2D(Collider2D c2d) {
        //  Trigger alanımızın adı öcü mü?
        if(c2d.name == "Ocu") {
            //  Karakter yumruk atıyor mu?
            if(animator.GetCurrentAnimatorStateInfo(0).IsName("punch")) {
                //  Öcüyü öldür
                ocu.SetTrigger("die_trigger");
            }
        }
    }
}



