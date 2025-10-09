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
    bool isControlPanelOpened = false;
    bool selected = false;

    // Start is called before the first frame update
    private void Awake()
    {
        mm = FindObjectOfType<MainMenuManager>();
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

        if(isControlPanelOpened)
        {
            if(Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(0))
            {
                options.SetTrigger("outCtrl");
                options.ResetTrigger("inCtrl");
                options.ResetTrigger("in");
                options.ResetTrigger("out");
                isControlPanelOpened = false;
            }
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && mm.isOptionPanelOpened)
        {
            mm.isOptionPanelOpened = false;
            startScreen.enabled = true;
            str.interactable = true;
            opt.interactable = true;
            ext.interactable = true;
            options.SetTrigger("out");
            options.ResetTrigger("in");
            options.ResetTrigger("outCtrl");
            options.ResetTrigger("inCtrl");
        }
    }
    public void Controls()
    {
        options.SetTrigger("inCtrl");
        options.ResetTrigger("outCtrl");
        options.ResetTrigger("in");
        options.ResetTrigger("out");
        isControlPanelOpened = true;
    }
    public void SelectCtrl()
    {
        selected = true;
        EventSystem.current.SetSelectedGameObject(ctrl);
    }
    public void Unselect()
    {
        selected = false;
    }
    public void SelectReset()
    {
        selected = true;
        EventSystem.current.SetSelectedGameObject(reset);
    }
}
