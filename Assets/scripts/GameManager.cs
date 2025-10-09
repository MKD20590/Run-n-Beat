using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private Player player;
    [SerializeField] private Animator boss;
    [SerializeField] private Animator gameOverScreen;
    [SerializeField] private Animator loading;
    [SerializeField] private Animator pause;

    [SerializeField] private Animator clear;
    [SerializeField] private Animator full;
    [SerializeField] private Animator doubleJumpLess;

    [SerializeField] private AudioSource bg;

    public bool win = false;
    bool winCount = false;

    bool fullHP = true;
    bool doubleJumpLessThan10 = true;

    public bool isPaused = false;
    bool selected = false;

    public bool isDied = false;
    bool dieCount = false;
    bool isRestarting = false;
    bool isBackToMenu = false;
    // Start is called before the first frame update
    private void Awake()
    {
        player = FindObjectOfType<Player>();
        Time.timeScale = 1;
    }


    // Update is called once per frame
    void Update()
    {
        //pause
        if (Input.GetKeyDown(KeyCode.Escape) && player.hp > 0 && !win && !isDied)
        {
            Pause();
        }

        //lose, player died
        if (player.hp <= 0)
        {
            if(!dieCount)
            {
                dieCount = true;
            }
            isDied = true;
            gameOverScreen.SetTrigger("in_lose");
            gameOverScreen.ResetTrigger("in_win");
        }

        //fading the background music out / in
        if (isRestarting || isBackToMenu || isDied)
        {
            if(AudioListener.volume > 0)
            {
                AudioListener.volume -= 0.01f;
            }
        }
        else
        {
            if (AudioListener.volume < 1)
            {
                AudioListener.volume += 0.01f;
            }
        }

        //winning and grading
        if(player.hp < 3)
        {
            fullHP = false;
        }
        if(player.totalDoubleJumps >= 10)
        {
            doubleJumpLessThan10 = false;
        }
        if(win && winCount && gameOverScreen.GetCurrentAnimatorStateInfo(0).IsName("stay_in_win"))
        {
            clear.SetTrigger("beatCleared");
            StartCoroutine(Countdown());
        }
        if (bg.time>=bg.clip.length-2 || boss.GetCurrentAnimatorStateInfo(0).IsName("stayDefeat"))
        {
            win = true;
            winCount = true;
            gameOverScreen.SetTrigger("in_win");
        }
    }
    public void Pause()
    {
        if(!isPaused)
        {
            isPaused = true;
            bg.Pause();
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

    public void Restart()
    {
        loading.SetTrigger("in");
        isRestarting = true;
        StartCoroutine(Countdown());
    }

    public void BackToMenu()
    {
        loading.SetTrigger("in");
        isBackToMenu = true;
        StartCoroutine(Countdown());
    }
    IEnumerator Countdown()
    {
        if(winCount)
        {
            if (fullHP)
            {
                yield return new WaitForSeconds(1f);
                full.SetTrigger("fullHP");
            }
            if (doubleJumpLessThan10)
            {
                yield return new WaitForSeconds(1f);
                doubleJumpLess.SetTrigger("doubleJLess10");
            }
            winCount = false;
        }
        yield return new WaitForSecondsRealtime(1.5f);
        if(isRestarting)
        {
            isRestarting = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else if(isBackToMenu)
        {
            isBackToMenu = false;
            SceneManager.LoadScene("MainMenu");
        }
    }
}
