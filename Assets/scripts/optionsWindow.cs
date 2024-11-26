using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Audio;

public class optionsWindow : MonoBehaviour
{
    [SerializeField] private AudioMixer bgFX;
    //[SerializeField] private AudioMixer fx;
    [SerializeField] private Slider bg1;
    [SerializeField] private Slider fx1;
    //[SerializeField] private GameObject bg2;
    //[SerializeField] private GameObject fx2;

    [SerializeField] private Animator options;
    [SerializeField] private Animator startScreen;
    [SerializeField] private GameObject opts;
    [SerializeField] private Button str;
    [SerializeField] private Button opt;
    [SerializeField] private Button ext;

    [SerializeField] private GameObject ctrl;
    [SerializeField] private GameObject reset;
    public MainMenu mm;
    bool control = false;
    bool selected = false;

    public static bool appOpened = false;
    public static float volumeBG;
    public static float volumeFX;
    // Start is called before the first frame update
    private void Awake()
    {
        if(appOpened)
        {
            if(bg1.value == 1)
            {
                bg1.value = volumeBG + 1;
            }
            if (fx1.value == 1)
            {
                fx1.value = volumeFX + 1;
            }
        }
        else
        {
            appOpened = true;
            if (PlayerPrefs.HasKey("volumeBG") && PlayerPrefs.HasKey("volumeFX"))
            {
                bg1.value = PlayerPrefs.GetFloat("volumeBG")+1;
                fx1.value = PlayerPrefs.GetFloat("volumeFX")+1;
            }
        }


    }
    void Start()
    {
        if(GameManager.volumeBG!=volumeBG && GameManager.volumeFX!=volumeFX && GameManager.volumeBG<1 && GameManager.volumeFX<1)
        {
            volumeBG = GameManager.volumeBG;
            volumeFX = GameManager.volumeFX;
        }
        //Debug.Log(volumeBG);
        //DontDestroyOnLoad(bg1);
        //DontDestroyOnLoad(fx1);
    }

    
    // Update is called once per frame
    void Update()
    {
        volumeBG = bg1.value-1;
        volumeFX = fx1.value-1;
        
        bgFX.SetFloat("bgm", volumeBG);
        bgFX.SetFloat("fx", volumeFX);
        if (Input.GetMouseButtonDown(0) && mm.option==true && !selected)
        {
            EventSystem.current.SetSelectedGameObject(ctrl);
        }
        if(control)
        {
            if(Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(0))
            {
                options.SetTrigger("outCtrl");
                options.ResetTrigger("inCtrl");
                options.ResetTrigger("in");
                options.ResetTrigger("out");
                control = false;
            }

        }
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            mm.option = false;
            startScreen.enabled = true;
            str.interactable = true;
            opt.interactable = true;
            ext.interactable = true;
            EventSystem.current.SetSelectedGameObject(opts);
            options.SetTrigger("out");
            options.ResetTrigger("in");
            options.ResetTrigger("outCtrl");
            options.ResetTrigger("inCtrl");
        }
    }
    public void Ctrls()
    {
        options.SetTrigger("inCtrl");
        options.ResetTrigger("outCtrl");
        options.ResetTrigger("in");
        options.ResetTrigger("out");
        control = true;
    }
    public void selectCtrl()
    {
        selected = true;
        EventSystem.current.SetSelectedGameObject(ctrl);
    }
    public void Unselect()
    {
        selected = false;
    }
    public void selectReset()
    {
        selected = true;
        EventSystem.current.SetSelectedGameObject(reset);
    }
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetFloat("volumeBG", volumeBG);
        PlayerPrefs.SetFloat("volumeFX", volumeFX);
    }
}
