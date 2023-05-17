using System.Collections;
using UnityEngine;

public class Firetrap : MonoBehaviour
{
    [SerializeField] private float damage;

    [Header("Firetrap Timers")]
    [SerializeField] private float activationDelay;
    [SerializeField] private float activeTime;
    private Animator anim;
    private SpriteRenderer spriteRend;

    [Header("SFX")]
    [SerializeField] private AudioClip firetrapSound;

    private bool triggered; // when it gets triggered
    private bool active; // active and can hurt

    private Health playerHealth;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(playerHealth != null && active)
            playerHealth.TakeDamage(damage);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerHealth = collision.GetComponent<Health>();

            if (!triggered)
                StartCoroutine(ActivateFiretrap());

            if (active)
                collision.GetComponent<Health>().TakeDamage(damage);
        }  
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            playerHealth = null;
    }

    private IEnumerator ActivateFiretrap()
    {
        //notify the player that it will activate in "activationDelay" seconds by turning the spire "red".
        triggered = true;
        spriteRend.color = Color.red;

        //wait for delay, activate trap, turn on the animation, return color back to normal
        yield return new WaitForSeconds(activationDelay);
        SoundManager.instance.PlaySound(firetrapSound);
        spriteRend.color = Color.white;
        active = true;
        anim.SetBool("activated", true);

        //resetting
        yield return new WaitForSeconds(activeTime);
        active = false;
        triggered = false;
        anim.SetBool("activated", false);
    }
}