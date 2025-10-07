using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Audio;

public class optionsWindow : MonoBehaviour
{
    //[SerializeField] private AudioMixer fx;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private Animator options;
    [SerializeField] private Animator startScreen;
    [SerializeField] private GameObject opts;
    [SerializeField] private Button str;
    [SerializeField] private Button opt;
    [SerializeField] private Button ext;

    [SerializeField] private GameObject ctrl;
    [SerializeField] private GameObject reset;
    public MainMenuManager mm;
    bool control = false;
    bool selected = false;

    public static bool appOpened = false;
    public static float volumeBG;
    public static float volumeFX;
    // Start is called before the first frame update
    private void Awake()
    {
        mm = FindObjectOfType<MainMenuManager>();
        if (appOpened)
        {
            if(bgmSlider.value == 1)
            {
                bgmSlider.value = volumeBG + 1;
            }
            if (sfxSlider.value == 1)
            {
                sfxSlider.value = volumeFX + 1;
            }
        }
        else
        {
            appOpened = true;
            if (!PlayerPrefs.HasKey("bgm") || !PlayerPrefs.HasKey("sfx"))
            {
                PlayerPrefs.SetFloat("bgm", 1f);
                PlayerPrefs.SetFloat("sfx", 1f);
                bgmSlider.value = 1;
                sfxSlider.value = 1;
                audioMixer.SetFloat("bgm", PlayerPrefs.GetFloat("bgm"));
                audioMixer.SetFloat("sfx", PlayerPrefs.GetFloat("sfx"));
            }
            else
            {
                bgmSlider.value = PlayerPrefs.GetFloat("bgm");
                sfxSlider.value = PlayerPrefs.GetFloat("sfx");
                audioMixer.SetFloat("bgm", PlayerPrefs.GetFloat("bgm"));
                audioMixer.SetFloat("sfx", PlayerPrefs.GetFloat("sfx"));
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
    }

    
    // Update is called once per frame
    void Update()
    {
        //audio controls
        if (PlayerPrefs.HasKey("bgm") && PlayerPrefs.HasKey("sfx"))
        {
            PlayerPrefs.SetFloat("bgm", bgmSlider.value);
            PlayerPrefs.SetFloat("sfx", sfxSlider.value);
            audioMixer.SetFloat("bgm", Mathf.Log10(PlayerPrefs.GetFloat("bgm")) * 20);
            audioMixer.SetFloat("sfx", Mathf.Log10(PlayerPrefs.GetFloat("sfx")) * 20);
        }
        else
        {
            audioMixer.SetFloat("bgm", Mathf.Log10(bgmSlider.value) * 20);
            audioMixer.SetFloat("sfx", Mathf.Log10(sfxSlider.value) * 20);
        }

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
        else if(Input.GetKeyDown(KeyCode.Escape) && mm.option)
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
