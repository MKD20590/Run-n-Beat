using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //[SerializeField] private GameObject start1;
    [SerializeField] private AudioSource bgm;
    [SerializeField] private Animator startScreen;
    [SerializeField] private Animator load;
    [SerializeField] private CanvasGroup loading;
    //[SerializeField] private Animator start1;
    //[SerializeField] private Animator options;
    [SerializeField] private Animator optionsWindow;
    [SerializeField] private Button str;
    [SerializeField] private Button opt;
    [SerializeField] private Button ext;
    //[SerializeField] private Animator exit;

    //[SerializeField] private AudioSource loop;
    //public Animator loading;
    //public Animator credit;

    //public static bool story = false, beats = false;

    //static bool startMenu = true;
    // Start is called before the first frame update
    [SerializeField] private GameObject strt;
    [SerializeField] private GameObject opts;
    [SerializeField] private GameObject exits;
    [SerializeField] private GameObject ctrl;
    //[SerializeField] private GameObject opts;
    //[SerializeField] private GameObject exit;
    static int loadingalpha = 0;
    //public static int volumeBG;
    //public static int volumeFX;
    bool started = false;
    bool selected = false;
    public bool option = false;
    private void Awake()
    {
        loading.alpha = loadingalpha;
        Time.timeScale = 1;
    }
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(strt);
        bgm.volume = 1;
        started = false;
        //EventSystem.current.SetSelectedGameObject(strt);
        startScreen.enabled = true;
        str.interactable = false;
        opt.interactable = false;
        ext.interactable = false;
        Invoke("interactableButtons", 7.5f);
        //strt.OnSelect();
    }
    void interactableButtons()
    {
        str.interactable = true;
        opt.interactable = true;
        ext.interactable = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !option && UnityEngine.EventSystems.EventSystem.current.alreadySelecting != true && !selected)
        {
            Debug.Log("mouse click");
            EventSystem.current.SetSelectedGameObject(strt);
        }

        //fading the background music out
        if(started && bgm.volume>0)
        {
            bgm.volume -= 0.01f;
        }
        //Debug.Log(selected);
    }
/*    public void startGame()
    {
        startScreen.SetTrigger("in");
        StartCoroutine(CountDown());
    }*/


    public void unSelected()
    {
        selected = false;
    }
    public void selectS()
    {
        selected = true;
        if (UnityEngine.EventSystems.EventSystem.current.alreadySelecting != true)
        {
            EventSystem.current.SetSelectedGameObject(strt);
        }
        startScreen.SetBool("selectStart",true);
        startScreen.SetBool("selectOptions", false);
        startScreen.SetBool("selectExit", false);
    }

   public void clickedS()
    {
        started = true;
        loading.alpha = 1;
        load.SetTrigger("in");
        //start1.SetTrigger("clicked");
        //startScreen.SetTrigger("in");
        StartCoroutine(CountDown());
    }




    public void selectO()
    {
        selected = true;
        if (UnityEngine.EventSystems.EventSystem.current.alreadySelecting != true)
        {
            EventSystem.current.SetSelectedGameObject(opts);
        }
        startScreen.SetBool("selectStart", false);
        startScreen.SetBool("selectOptions", true);
        startScreen.SetBool("selectExit", false);
    }

    public void clickedO()
    {
        option = true;
        str.interactable = false;
        opt.interactable = false;
        ext.interactable = false;
        startScreen.enabled = false;

        EventSystem.current.SetSelectedGameObject(ctrl);
        startScreen.SetBool("selectStart", false);
        startScreen.SetBool("selectOptions", true);
        startScreen.SetBool("selectExit", false);

        optionsWindow.SetTrigger("in");
        optionsWindow.ResetTrigger("out");
        optionsWindow.ResetTrigger("inCtrl");
        optionsWindow.ResetTrigger("outCtrl");
    }

    public void selectE()
    {
        selected = true;
        if (UnityEngine.EventSystems.EventSystem.current.alreadySelecting != true) 
        {
            EventSystem.current.SetSelectedGameObject(exits);
        }

        startScreen.SetBool("selectStart", false);
        startScreen.SetBool("selectOptions", false);
        startScreen.SetBool("selectExit", true);
    }

    public void clickedE()
    {
        startScreen.SetBool("selectStart", false);
        startScreen.SetBool("selectOptions", false);
        startScreen.SetBool("selectExit", true);
        //startScreen.SetTrigger("exited");
    }


    public void quit1()
    {

        Application.Quit();
        //Invoke("quit2", 2);
    }
/*    public void quit2()
    {
        Application.Quit();
    }
    public void credits()
    {
        credit.SetTrigger("in");
    }*/
    IEnumerator CountDown()
    {
        loadingalpha = 1;
        loading.alpha = loadingalpha;
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(2);
/*        if(story)
        {
            SceneManager.LoadScene("storymode");
        }
        else
        {
            SceneManager.LoadScene("beatSelect");
        }*/
    }
}
