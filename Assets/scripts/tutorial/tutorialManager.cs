using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Audio;

public class tutorialManager : MonoBehaviour
{
    //public optionsWindow ow;
    [SerializeField] private AudioMixer bgmFX;
    //[SerializeField] private Cubeat cb;
    //[SerializeField] private AudioMixer fx;

    [SerializeField] private Character chara;
    [SerializeField] private Animator boss;
    [SerializeField] private Animator wl;
    [SerializeField] private Animator loading;
    [SerializeField] private Animator pause;

    [SerializeField] private Animator clear;
    [SerializeField] private Animator full;
    [SerializeField] private Animator doubleJLess;

    [SerializeField] private GameObject Continue;
    [SerializeField] private GameObject Restart;
    [SerializeField] private GameObject Title;

    [SerializeField] private GameObject next;
    [SerializeField] private GameObject title1;

    [SerializeField] private GameObject retry;
    [SerializeField] private GameObject title2;
    [SerializeField] private AudioSource bg;

    public bool win = false;
    bool winCount = false;
    public bool load = true;

    bool fullHP = true;
    bool doubleJLessThan10 = true;

    bool paused = false;
    bool selected = false;

    public bool died = false;
    bool dieCount = false;
    bool res = false;
    bool menu = false;
    bool bgPlay = false;
    public static float volumeBG = 1;
    public static float volumeFX = 1;
    // Start is called before the first frame update
    private void Awake()
    {

        volumeBG = optionsWindow.volumeBG;
        volumeFX = optionsWindow.volumeFX;
        //Debug.Log(volumeBG);
        Time.timeScale = 1;
        bg.volume = 1;
    }
    void Start()
    {
        bgmFX.SetFloat("bgm", volumeBG);
        bgmFX.SetFloat("fx", volumeFX);
    }

    // Update is called once per frame
    void Update()
    {
        bgmFX.SetFloat("bgm", volumeBG);
        bgmFX.SetFloat("fx", volumeFX);
        if (loading.GetCurrentAnimatorStateInfo(0).IsName("stay_out") && !bgPlay) //check if the loading animation named "stay_out" is playing
        {
            bg.Play();
            load = false;
            bgPlay = true;
        }
        if (pause.GetCurrentAnimatorStateInfo(0).IsName("stay_out")) //check if the pause animation named "stay_out" is playing
        {
            bg.UnPause();
        }

        //pause
        if (Input.GetKeyDown(KeyCode.Escape) && chara.hp > 0 && !win && !died && loading.GetCurrentAnimatorStateInfo(0).IsName("stay_out"))
        {
            Pause();
        }

        //UI buttons navigation with keyboard
        if (Input.GetMouseButtonDown(0) && paused && !selected && chara.hp > 0)
        {
            EventSystem.current.SetSelectedGameObject(Continue);
        }
        else if (Input.GetMouseButtonDown(0) && !selected && win)
        {
            EventSystem.current.SetSelectedGameObject(next);
        }
        else if (Input.GetMouseButtonDown(0) && !selected && died)
        {
            EventSystem.current.SetSelectedGameObject(retry);
        }

        //Debug.Log(bg.clip.length);
        //Debug.Log(bg.time);


        //fading the background music out
        if (res || menu && bg.volume > 0)
        {
            bg.volume -= 0.02f;
        }

        //winning and grading
        if (chara.hp < 3)
        {
            fullHP = false;
        }
        if (chara.totalDoubleJ >= 10)
        {
            doubleJLessThan10 = false;
        }
        if (win && winCount && wl.GetCurrentAnimatorStateInfo(0).IsName("stay_in_win"))
        {
            clear.SetTrigger("beatCleared");
            StartCoroutine(countdown());
        }
        if (bg.time >= bg.clip.length - 2 || boss.GetCurrentAnimatorStateInfo(0).IsName("stayDefeat"))
        {
            win = true;
            winCount = true;
            wl.SetTrigger("in_win");
            //StartCoroutine(countdown());

            Debug.Log("win");
        }
        //Debug.Log(bg.clip.length);
    }
    public void Pause()
    {
        if (!paused && pause.GetCurrentAnimatorStateInfo(0).IsName("stay_out"))
        {
            bg.Pause();
            if (UnityEngine.EventSystems.EventSystem.current.alreadySelecting != true)
            {
                EventSystem.current.SetSelectedGameObject(Continue);
            }
            pause.SetTrigger("paused");
            pause.ResetTrigger("unpaused");
            paused = true;
            bg.Pause();
            Time.timeScale = 0;
        }

        else
        {
            pause.SetTrigger("unpaused");
            pause.ResetTrigger("paused");
            paused = false;
        }
    }

    public void selectC()
    {
        selected = true;
        EventSystem.current.SetSelectedGameObject(Continue);
    }
    public void selectR()
    {
        selected = true;
        EventSystem.current.SetSelectedGameObject(Restart);
    }
    public void selectT()
    {
        selected = true;
        EventSystem.current.SetSelectedGameObject(Title);
    }
    public void Unselect()
    {
        selected = false;
    }

    public void restart()
    {
        loading.SetTrigger("in");
        res = true;
        StartCoroutine(countdown());
    }

    public void BackToMenu()
    {
        loading.SetTrigger("in");
        menu = true;
        StartCoroutine(countdown());
    }
    IEnumerator countdown()
    {
        if (winCount)
        {
            EventSystem.current.SetSelectedGameObject(next);
            if (fullHP)
            {
                yield return new WaitForSeconds(1f);
                full.SetTrigger("fullHP");
            }
            if (doubleJLessThan10)
            {
                yield return new WaitForSeconds(1f);
                doubleJLess.SetTrigger("doubleJLess10");
            }
            winCount = false;
        }
        yield return new WaitForSecondsRealtime(1.5f);
        //Debug.Log("k");
        if (res)
        {
            res = false;
            SceneManager.LoadScene("gameplay");
        }
        else if (menu)
        {
            Debug.Log("menu");
            menu = false;
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void selectNext()
    {
        selected = true;
        EventSystem.current.SetSelectedGameObject(next);
    }
    public void selectTitle1()
    {
        selected = true;
        EventSystem.current.SetSelectedGameObject(title1);
    }

    public void selectRet()
    {
        selected = true;
        EventSystem.current.SetSelectedGameObject(retry);
    }
    public void selectTitle2()
    {
        selected = true;
        EventSystem.current.SetSelectedGameObject(title2);
    }
}
