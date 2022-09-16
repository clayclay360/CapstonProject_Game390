using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Reflection;

public class StartMenu : MonoBehaviour
{

    [Header("Overlay")]
    public float overlayDuration;
    public Image overlayImage;

    private enum Condition { add, subtract };
    private Condition condition;

    private float reference;

    // Start is called before the first frame update
    void Start()
    {
        overlayImage.color = new Color(0,0,0,1);
        StartCoroutine(ScreenOverlay(Condition.subtract,0,overlayDuration));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
}
