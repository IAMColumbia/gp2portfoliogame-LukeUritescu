using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerOfGame : MonoBehaviour
{
    public GameObject PlayerSub;
    public GameObject EnemySub;

    private Enemy enemySubject;
    private Player playerSubject;

    protected Manager manager;

    ManagerState State;

    // Start is called before the first frame update
    void Start()
    {
        
        this.manager = new Manager();
        this.SetUp();
    }

    public void SetUp()
    {
        playerSubject = PlayerSub.GetComponent<Player>();
        enemySubject = EnemySub.GetComponent<Enemy>();
        this.manager.Attach(playerSubject.UnityPlayerSub);
        this.manager.Attach(enemySubject.UnityEnemySub);
        this.State = manager.State;

    }

    // Update is called once per frame
    void Update()
    {
        if(this.State != this.manager.State)
        {
            this.State = this.manager.State;
        }
        switch (this.manager.State)
        {
            case ManagerState.NoState:
                break;
            case ManagerState.ExitGame:
                ExitGame();
                break;
            case ManagerState.RestartGame:
                RestartGame();
                break;
            case ManagerState.BossDefeated:
                BossDefeated();
                break;
            case ManagerState.PlayerDefeated:
                PlayerDefeated();
                break;
        }   
    }

    private void ExitGame()
    {
        Application.Quit();
    }

    private void RestartGame()
    {       
        SceneManager.LoadScene("MVPTest", LoadSceneMode.Single);
    }

    private void BossDefeated()
    {
        SceneManager.LoadScene("MVPTest", LoadSceneMode.Single);
    }

    private void PlayerDefeated()
    {
        SceneManager.LoadScene("MVPTest", LoadSceneMode.Single);
    }
}
