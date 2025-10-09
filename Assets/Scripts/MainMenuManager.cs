using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private AudioSource bgm;
    [SerializeField] private Animator startScreen;
    [SerializeField] private Animator load;
    //[SerializeField] private CanvasGroup loading;
    [SerializeField] private Animator optionsWindow;
    [SerializeField] private Button str;
    [SerializeField] private Button opt;
    [SerializeField] private Button ext;

    [SerializeField] private GameObject strt;
    [SerializeField] private GameObject opts;
    [SerializeField] private GameObject exits;
    [SerializeField] private GameObject ctrl;

    static int loadingAlpha = 0;

    bool started = false;
    bool selected = false;
    public bool isOptionPanelOpened = false;
    private void Awake()
    {
        //loading.alpha = loadingAlpha;
        Time.timeScale = 1;
    }
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(strt);
        bgm.volume = 1;
        started = false;
        startScreen.enabled = true;
        str.interactable = false;
        opt.interactable = false;
        ext.interactable = false;
        Invoke("InteractableButtons", 7.5f);
    }
    void InteractableButtons()
    {
        str.interactable = true;
        opt.interactable = true;
        ext.interactable = true;
    }

    // Update is called once per frame
    void Update()
    {
        //fading the background music out / in
        if(started && AudioListener.volume > 0)
        {
            AudioListener.volume -= 0.01f;
        }
        else if(!started && AudioListener.volume < 1)
        {
            AudioListener.volume += 0.01f;
        }
    }

    public void UnSelected()
    {
        selected = false;
    }
    public void SelectStart()
    {
        selected = true;
        if (EventSystem.current.alreadySelecting != true)
        {
            EventSystem.current.SetSelectedGameObject(strt);
        }
        startScreen.SetBool("selectStart",true);
        startScreen.SetBool("selectOptions", false);
        startScreen.SetBool("selectExit", false);
    }

   public void StartGame()
    {
        started = true;
        //loading.alpha = 1;
        load.SetBool("in",true);
        StartCoroutine(CountDown());
    }


    public void SelectOptions()
    {
        selected = true;
        if (EventSystem.current.alreadySelecting != true)
        {
            EventSystem.current.SetSelectedGameObject(opts);
        }
        startScreen.SetBool("selectStart", false);
        startScreen.SetBool("selectOptions", true);
        startScreen.SetBool("selectExit", false);
    }

    public void Options()
    {
        isOptionPanelOpened = true;
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

    public void SelectExit()
    {
        selected = true;
        if (EventSystem.current.alreadySelecting != true) 
        {
            EventSystem.current.SetSelectedGameObject(exits);
        }

        startScreen.SetBool("selectStart", false);
        startScreen.SetBool("selectOptions", false);
        startScreen.SetBool("selectExit", true);
    }

    public void Quit()
    {
        Application.Quit();
    }
    IEnumerator CountDown()
    {
        loadingAlpha = 1;
        //loading.alpha = loadingAlpha;
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(2);
    }
}
