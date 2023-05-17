using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private AudioClip checkpointSound; //sound for it
    private Transform currentCheckpoint;    //last checkpoint
    private Health playerHealth;
    private UIManager uiManager;

    private void Awake()
    {
        playerHealth = GetComponent<Health>();
        uiManager = FindObjectOfType<UIManager>();
    }

    public void CheckRespawn()
    {
        //check if any checkpoints are available
        if(currentCheckpoint == null)
        {
            //show game over screen
            uiManager.GameOver();

            return; //dont execute rest
        }

        transform.position = currentCheckpoint.position;
        playerHealth.Respawn();



        Camera.main.GetComponent<CameraController>().MoveToNewRoom(currentCheckpoint.parent);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Checkpoint")
        {
            currentCheckpoint = collision.transform;
            SoundManager.instance.PlaySound(checkpointSound);
            collision.GetComponent<Collider2D>().enabled = false;
            collision.GetComponent<Animator>().SetTrigger("appear");
        }
    }
}
