using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("startGame", 7f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void startGame()
    {
        SceneManager.LoadScene(1);
    }
}
