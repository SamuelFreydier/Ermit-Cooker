using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    private float timelvl1; //Temps du niveau 1
    private float besttimelvl1 = 0; //Meilleur temps du niveau 1
    private float timelvl2;
    private float besttimelvl2;
    private float countdown;
    private float runtimer = 0;
    private float bestrun; //Meilleure run complète
    private bool startRun = false;


    //Fonction utile pour le débogage car il y a un système de sauvegarde avec les PlayerPrefs pour enregistrer les meilleurs temps entre chaque session

    /*private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            PlayerPrefs.SetFloat("BestTime1", 0);
            PlayerPrefs.SetFloat("BestTime2", 0);
            PlayerPrefs.SetFloat("BestTimeRun", 0);
            bestrun = PlayerPrefs.GetFloat("BestTimeRun");
            besttimelvl1 = PlayerPrefs.GetFloat("BestTime1");
            besttimelvl2 = PlayerPrefs.GetFloat("BestTime2");
            PlayerPrefs.SetFloat("BestTime1", besttimelvl1);
            PlayerPrefs.SetFloat("BestTime2", besttimelvl2);
            PlayerPrefs.SetFloat("BestTimeRun", bestrun);
        }
    }*/

    public void UpdateTime(float time) //On update le timer en fonction du niveau courant
    {
        timelvl1 = time;
    }

    public void UpdateCountdown(float time) //On update le countdown
    {
        countdown = time;
    }

    public float Countdown
    {
        get { return countdown; }
    }

    public void UpdateBestTime(float time) //Se déclenche à la fin d'un niveau quand le joueur a gagné : on compare le temps actuel et le meilleur temps et on change ou non
    {

        if (time > besttimelvl1)
        {
            besttimelvl1 = time;
        }
        if (startRun)
        {
            runtimer = time;
        }
        PlayerPrefs.SetFloat("BestTime", besttimelvl1);
    }

    public float BestTimeRun
    {
        get { return bestrun; }
    }
    public float BestTime
    {
        get { return besttimelvl1; }
    }
    public float TimeLvl1
    {
        get { return timelvl1; }
    }

    public float TimeLvl2
    {
        get { return timelvl2; }
    }

    public float BestTimeLvl2
    {
        get { return besttimelvl2; }
    }
    private void Start()
    {
        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);
        besttimelvl1 = PlayerPrefs.GetFloat("BestTime");
    }

    private void HandleGameStateChanged(GameManager.GameState currentstate, GameManager.GameState previousstate)
    {
        if (currentstate == GameManager.GameState.PREGAME) //Si on va sur le menu ou en state LOSE, on quitte la run en cours
        {
            startRun = false;
            runtimer = 0;
        }
        if (currentstate == GameManager.GameState.RUNNING && previousstate == GameManager.GameState.PREGAME) //Si on démarre au niveau 1, une run commence
        {
            if (GameManager.Instance.LevelName == "Level1")
            {
                startRun = true;
            }
        }
    }

}