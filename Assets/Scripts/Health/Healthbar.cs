using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private Image totalhealthBar;
    [SerializeField] private Image currentHealthBar;

    private void Start()
    {
        totalhealthBar.fillAmount = playerHealth.currentHealth / 10;
    }
    private void Update()
    {
        currentHealthBar.fillAmount = playerHealth.currentHealth / 10;
    }
}
