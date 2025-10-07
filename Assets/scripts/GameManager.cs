using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    //public optionsWindow ow;
    [SerializeField] private AudioMixer bgmFX;
    //[SerializeField] private Cubeat cb;
    //[SerializeField] private AudioMixer fx;

    [SerializeField] private Player player;
    [SerializeField] private Animator boss;
    [SerializeField] private Animator wl;
    [SerializeField] private Animator loading;
    [SerializeField] private Animator pause;

    [SerializeField] private Animator clear;
    [SerializeField] private Animator full;
    [SerializeField] private Animator doubleJLess;

    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject restartButton;
    [SerializeField] private GameObject titleButton;

    [SerializeField] private GameObject next;
    [SerializeField] private GameObject title1;

    [SerializeField] private GameObject retry;
    [SerializeField] private GameObject title2;
    [SerializeField] private AudioSource bg;

    public bool win = false;
    bool winCount = false;

    bool fullHP = true;
    bool doubleJumpLessThan10 = true;

    public bool isPaused = false;
    bool selected = false;

    public bool died = false;
    bool dieCount = false;
    bool res = false;
    bool menu = false;
    public static float volumeBG = 1;
    public static float volumeFX = 1;
    // Start is called before the first frame update
    private void Awake()
    {
        player = FindObjectOfType<Player>();
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

        //pause
        if (Input.GetKeyDown(KeyCode.Escape) && player.hp > 0 && !win && !died)
        {
            Pause();
        }

        //UI buttons navigation with keyboard
        if (Input.GetMouseButtonDown(0) && isPaused && !selected && player.hp > 0)
        {
            EventSystem.current.SetSelectedGameObject(continueButton);
        }
        else if (Input.GetMouseButtonDown(0) && !selected && win)
        {
            EventSystem.current.SetSelectedGameObject(next);
        }
        else if (Input.GetMouseButtonDown(0) && !selected && died)
        {
            EventSystem.current.SetSelectedGameObject(retry);
        }
        //lose, playercter died
        if (player.hp <= 0)
        {
            if(!dieCount)
            {
                EventSystem.current.SetSelectedGameObject(retry);
                dieCount = true;
            }
            died = true;
            wl.SetTrigger("in_lose");
            wl.ResetTrigger("in_win");
        }


        //fading the background music out
        if (res || menu || died && bg.volume>0)
        {
            bg.volume -= 0.02f;
        }

        //winning and grading
        if(player.hp<3)
        {
            fullHP = false;
        }
        if(player.totalDoubleJ>=10)
        {
            doubleJumpLessThan10 = false;
        }
        if(win && winCount && wl.GetCurrentAnimatorStateInfo(0).IsName("stay_in_win"))
        {
            clear.SetTrigger("beatCleared");
            StartCoroutine(Countdown());
        }
        if (bg.time>=bg.clip.length-2 || boss.GetCurrentAnimatorStateInfo(0).IsName("stayDefeat"))
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
        if(!isPaused)
        {
            isPaused = true;
            bg.Pause();
            if (EventSystem.current.alreadySelecting != true)
            {
                EventSystem.current.SetSelectedGameObject(continueButton);
            }
            pause.SetTrigger("paused");
            pause.ResetTrigger("unPaused");
            Time.timeScale = 0;
        }

        else
        {
            pause.SetTrigger("unPaused");
            pause.ResetTrigger("isPpaused");
            isPaused = false;
        }
    }

    public void SelectC()
    {
        selected = true;
        EventSystem.current.SetSelectedGameObject(continueButton);
    }
    public void SelectR()
    {
        selected = true;
        EventSystem.current.SetSelectedGameObject(restartButton);
    }
    public void SelectT()
    {
        selected = true;
        EventSystem.current.SetSelectedGameObject(titleButton);
    }
    public void Unselect()
    {
        selected = false;
    }

    public void Restart()
    {
        loading.SetTrigger("in");
        res = true;
        StartCoroutine(Countdown());
    }

    public void BackToMenu()
    {
        loading.SetTrigger("in");
        menu = true;
        StartCoroutine(Countdown());
    }
    IEnumerator Countdown()
    {
        if(winCount)
        {
            EventSystem.current.SetSelectedGameObject(next);
            if (fullHP)
            {
                yield return new WaitForSeconds(1f);
                full.SetTrigger("fullHP");
            }
            if (doubleJumpLessThan10)
            {
                yield return new WaitForSeconds(1f);
                doubleJLess.SetTrigger("doubleJLess10");
            }
            winCount = false;
        }
        yield return new WaitForSecondsRealtime(1.5f);
        //Debug.Log("k");
        if(res)
        {
            res = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else if(menu)
        {
            //Debug.Log("menu");
            menu = false;
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void SelectNext()
    {
        selected = true;
        EventSystem.current.SetSelectedGameObject(next);
    }
    public void SelectTitle1()
    {
        selected = true;
        EventSystem.current.SetSelectedGameObject(title1);
    }

    public void SelectRet()
    {
        selected = true;
        EventSystem.current.SetSelectedGameObject(retry);
    }
    public void SelectTitle2()
    {
        selected = true;
        EventSystem.current.SetSelectedGameObject(title2);
    }
}
