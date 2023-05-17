using UnityEngine;

public class Spikehead : EnemyDamage
{
    [Header ("Spikehead Attributes")]
    [SerializeField] private float speed;
    [SerializeField] private float range;
    [SerializeField] private float checkDelay;
    [SerializeField] private LayerMask playerLayer;
    private Vector3[] directions = new Vector3[4];
    private Vector3 destination;
    private float checkTimer;
    private bool attacking;

    [Header("SFX")]
    [SerializeField] private AudioClip impactSound;

    private void OnEnable()
    {
        Stop();
    }
    private void Update()
    {
        //move spikehead to dest. only if attacking
        if (attacking)
            transform.Translate(destination * Time.deltaTime * speed);
        else
        {
            checkTimer += Time.deltaTime;
            if (checkTimer > checkDelay)
                CheckForPlayer();
        }
    }
    private void CheckForPlayer()
    {
        CalcualteDirections();

        //check if the spikehead sees the player in all 4 directions  or not
        for (int i = 0; i < directions.Length; i++)
        {
            Debug.DrawRay(transform.position, directions[i], Color.red);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directions[i], range, playerLayer);

            if(hit.collider != null && !attacking)
            {
                attacking = true;
                destination = directions[i];
                checkTimer = 0;
            }
        }
    }
    private void CalcualteDirections()
    {
        directions[0] = transform.right * range;     //right
        directions[1] = -transform.right * range;    //left
        directions[2] = transform.up * range;        //up
        directions[3] = -transform.up * range;       //down
    }
    private void Stop()
    {
        destination = transform.position;
        attacking = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SoundManager.instance.PlaySound(impactSound);
        base.OnTriggerEnter2D (collision);
        Stop();  //stop once it hits smth
    }
}