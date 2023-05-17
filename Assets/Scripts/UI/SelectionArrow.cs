using UnityEngine;
using UnityEngine.UI;

public class SelectionArrow : MonoBehaviour
{
    [SerializeField] private RectTransform[] options;
    [SerializeField] private AudioClip changeSound;
    [SerializeField] private AudioClip interractSound;
    private RectTransform rect;
    private int currentPosition;


    private void Awake()
    {
            rect = GetComponent<RectTransform>();
    }
    private void Update()
    {
        //change position of the arrow
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            ChangePosition(-1);
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            ChangePosition(1);

        //interract with options
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.E))
            Interract();
    }

    private void ChangePosition(int _change)
    {
        currentPosition += _change;
        
        if (_change != 0)
            SoundManager.instance.PlaySound(changeSound);

        if (currentPosition < 0) 
            currentPosition = options.Length - 1;
        else if (currentPosition > options.Length - 1)
            currentPosition =0;

        // assign the Y position of selected option
        rect.position = new Vector3(rect.position.x, options[currentPosition].position.y, 0);
    }

    public void Interract()
    {
        SoundManager.instance.PlaySound(interractSound);

        //access the button component and call the function of it
        options[currentPosition].GetComponent<Button>().onClick.Invoke();
    }
}