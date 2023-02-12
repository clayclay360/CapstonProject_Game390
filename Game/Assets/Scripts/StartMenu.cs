using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Reflection;
using UnityEngine.InputSystem;

public class StartMenu : MonoBehaviour
{
    [Header("Controls")]
    public const int MENBUTTNUM = 4; //Total number of menu buttons
    public Button playBtn, controlsBtn, quitBtn, howToPlayBtn, controlBackBtn, howToBackBtn;
    private int selectedButt;
    private Gamepad gamepad;
    private bool axisLocked, controlMenuIsUp, howToMenuIsUp;

    [Header("Overlay")]
    public float overlayDuration;
    public Image overlayImage;

    private HowTo howTo;
    private enum Condition { add, subtract };
    private Condition condition;

    private float reference;

    // Start is called before the first frame update
    void Start()
    {
        //Controls
        gamepad = Gamepad.current; //Hope this works!
        selectedButt = 0;
        axisLocked = false;
        ShadeButtons();


        //Overlay
        overlayImage.color = new Color(0,0,0,1);
        StartCoroutine(ScreenOverlay(Condition.subtract,0,overlayDuration));

        //HowTo
        howTo = FindObjectOfType<HowTo>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.frameCount > 30) //Controller input starts with positive value in the first few frames
        {
            CheckControls();
        } 
    }

    #region Controls
    //Check Controller input for the button selection
    private void CheckControls()
    {
        if (gamepad == null) { return; }
        //Close the controls menu
        if (controlMenuIsUp)
        {
            if (gamepad.buttonEast.wasPressedThisFrame)
            {
                controlBackBtn.onClick.Invoke();
                controlMenuIsUp = false;
            }
            return;
        }

        if (howToMenuIsUp)
        {
            howTo = FindObjectOfType<HowTo>();

            if (gamepad.buttonEast.wasPressedThisFrame)
            {
                howToMenuIsUp = false;
                howToBackBtn.onClick.Invoke();
            }

            //change the pages
            if (gamepad.rightTrigger.wasPressedThisFrame)
            {
                ++howTo.pageNumber;
            }
            if (gamepad.leftTrigger.wasPressedThisFrame)
            {
                --howTo.pageNumber;
            }
        }

        //Navigate up or down depending on controller input
        //Gets y value of left controller stick
        float stickL = gamepad.leftStick.ReadValue().y;
        if (stickL > .25f && !axisLocked)
        {
            //Increase the number or loop back to 0
            selectedButt = selectedButt == 0 ? MENBUTTNUM - 1 : selectedButt - 1;
            axisLocked = true;
        }
        else if (stickL < -.25f && !axisLocked)
        {
            //Decrease the number or loop back to max
            selectedButt = selectedButt == MENBUTTNUM - 1 ? 0 : selectedButt + 1;
            axisLocked = true;
        }
        else if (axisLocked && stickL > -.25f && stickL < .25f)
        {
            axisLocked = false;
        }
        ShadeButtons();

        //Evoke the selected button if the controller button was pressed
        if (gamepad.buttonWest.wasPressedThisFrame)
        {
            Button[] buttons = { playBtn, controlsBtn, howToPlayBtn, quitBtn };
            buttons[selectedButt].onClick.Invoke();
            if (buttons[selectedButt] == controlsBtn) //Sets state for controls menu
            {
                controlMenuIsUp = true;
            }

            if (buttons[selectedButt] == howToPlayBtn) //Sets state for controls menu
            {
                howToMenuIsUp = true;
            }
        }

    }

    //Sets the color for the buttons
    private void ShadeButtons()
    {
        Button[] buttons = { playBtn, controlsBtn, howToPlayBtn, quitBtn};
        for (int i = 0; i < buttons.Length; i++)
        {
            //The selected button will be shaded, the others will not
            if (i == selectedButt)
            {
                buttons[i].image.color = new Color32(255, 255, 255, 255);
            }
            else
            {
                buttons[i].image.color = new Color32(200, 200, 200, 255);
            }
        }
    }
    #endregion

    private IEnumerator ScreenOverlay(Condition cond,float a, float duration, int sceneNumber = -1)
    {
        Debug.Log("Overlaying");
        float length = 0;
        float alpha = overlayImage.color.a;
        bool myBool = true;

        while(myBool)
        {
            if (cond == Condition.subtract)
            {
                length = 0.05f;
                myBool = alpha > length;
            }
            else
            {
                length = 0.98f;
                myBool = alpha < length;
            }

            alpha = Mathf.Lerp(alpha, a, duration);
            overlayImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        overlayImage.color = new Color(0, 0, 0, a);

        if(sceneNumber != -1)
        {
            SceneManager.LoadScene(sceneNumber, LoadSceneMode.Single);
        }
    }

    public void LoadLevel(int index)
    {
        StartCoroutine(ScreenOverlay(Condition.add,1, overlayDuration, index));
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
