using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Reflection;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [Header("Controls")]
    public const int PASBUTTNUM = 3; //Total number of menu buttons
    public Button howToPlayButt, menuButt, quitButt, howToPlayQuitButt;
    private int selectedButt;
    private Gamepad gamepad;
    private bool axisLocked, controlMenuIsUp;

    public GameObject pauseMenuUI;

    public static bool GameIsPaused = false;

    void Start()
    {
        //Controls
        gamepad = Gamepad.current; //Hope this works!
        selectedButt = 0;
        axisLocked = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (gamepad.startButton.isPressed)
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Paused();
                //CheckControls();
            }
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
            if (gamepad.buttonWest.wasPressedThisFrame)
            {
                howToPlayQuitButt.onClick.Invoke();
                controlMenuIsUp = false;
            }
            return;
        }

        //Navigate up or down depending on controller input
        //Gets y value of left controller stick
        float stickL = gamepad.leftStick.ReadValue().y;
        if (stickL > .25f && !axisLocked)
        {
            //Increase the number or loop back to 0
            selectedButt = selectedButt == 0 ? PASBUTTNUM - 1 : selectedButt - 1;
            axisLocked = true;
        }
        else if (stickL < -.25f && !axisLocked)
        {
            //Decrease the number or loop back to max
            selectedButt = selectedButt == PASBUTTNUM - 1 ? 0 : selectedButt + 1;
            axisLocked = true;
        }
        else if (axisLocked && stickL > -.25f && stickL < .25f)
        {
            axisLocked = false;
        }
  

        //Evoke the selected button if the controller button was pressed
        if (gamepad.buttonWest.wasPressedThisFrame)
        {
            Button[] buttons = { menuButt, howToPlayButt, quitButt };
            buttons[selectedButt].onClick.Invoke();
            if (buttons[selectedButt] == howToPlayButt) //Sets state for controls menu
            {
                controlMenuIsUp = true;
            }
        }

    }
    #endregion

    public void Paused()
    {
        GameIsPaused = true;
        Time.timeScale = 0f;
        CheckControls();
        pauseMenuUI.SetActive(true);
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void LoadMenu()
    {
        Debug.Log("Loading Menu...");
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Debug.Log("Qutting game...");
        Application.Quit();
    }
}
