using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public GameObject[] SystemPrefabs; //Liste d'objets système (d'autres managers) à instancier dès le début
    public Events.EventGameState OnGameStateChanged; //Event qui se lance quand le State change
    public Events.EventReputState OnReputStateChanged;
    private List<AsyncOperation> _loadOperations; //Liste d'opérations de chargement
    private List<GameObject> _instanciedSystemPrefabs; //Liste des systemPrefabs qui ont été instanciés
    private string _currentLevelName = string.Empty; //Nom du niveau actuel
    private GameState _currentGameState = GameState.PREGAME; //State actuel
    private ReputState _currentReputState = ReputState.SAFE;

    public enum GameState //Ensemble des State
    {
        PREGAME, //Menu essentiellement
        RUNNING, //En train de jouer, en plein niveau
        PAUSED, //Jeu en pause (pendant un RUNNING)
        WIN, //Le joueur a gagné
    }

    public enum ReputState
    {
        SAFE,
        AVERAGE,
        TOUGH
    }

    public ReputState CurrentReputState
    {
        get { return _currentReputState; }
    }
    public GameState CurrentGameState //Renvoie le State actuel
    {
        get { return _currentGameState; }
    }

    public string LevelName //Renvoie le nom du niveau actuel
    {
        get { return _currentLevelName; }
    }


    public void RestartGame() //Retour au menu
    {
        if (_currentGameState != GameState.PREGAME)
        {
            UpdateState(GameState.PREGAME);
        }
    }
    public void TogglePause() //Passage du jeu en pause ou retour au jeu depuis une pause
    {
        if (_currentGameState == GameState.RUNNING)
        {
            UpdateState(GameState.PAUSED);
        }
        else if (_currentGameState == GameState.PAUSED)
        {
            UpdateState(GameState.RUNNING);
        }
    }

    public void TriggerWin() //Déclenchement de la victoire du joueur
    {
        if (_currentGameState != GameState.WIN)
        {
            UpdateState(GameState.WIN);
        }
    }

    public void UpdateReput(ReputState state) //Mise à jour du State actuel
    {
        ReputState previousReputState = _currentReputState;
        switch (state)
        {
            case ReputState.SAFE:
                {
                    _currentReputState = ReputState.SAFE;
                    Time.timeScale = 1f;
                    Debug.Log("SAFE");
                }
                break;
            case ReputState.AVERAGE:
                {
                    _currentReputState = ReputState.AVERAGE;
                    Time.timeScale = 1f;
                    Debug.Log("Average");
                }
                break;
            case ReputState.TOUGH:
                {
                    _currentReputState = ReputState.TOUGH;
                    Time.timeScale = 1f;
                    Debug.Log("Tough");
                }
                break;
            default:
                {
                    _currentReputState = ReputState.SAFE;
                    Time.timeScale = 1f;
                    Debug.Log("Safe");
                }
                break;
        }
        OnReputStateChanged.Invoke(_currentReputState, previousReputState);
    }

    private void UpdateState(GameState state) //Mise à jour du State actuel
    {
        GameState previousGameState = _currentGameState;
        switch (state)
        {
            case GameState.PREGAME:
                {
                    _currentGameState = GameState.PREGAME;
                    Time.timeScale = 1f;
                    Debug.Log("Pregame");
                }
                break;
            case GameState.RUNNING:
                {
                    _currentGameState = GameState.RUNNING;
                    Time.timeScale = 1f;
                    Debug.Log("Running");
                }
                break;
            case GameState.PAUSED:
                {
                    _currentGameState = GameState.PAUSED;
                    Time.timeScale = 0;
                    Debug.Log("Pause");
                }
                break;
            case GameState.WIN:
                {
                    _currentGameState = GameState.WIN;
                    Time.timeScale = 1f;
                    Debug.Log("Win");
                }
                break;
            default:
                {
                    _currentGameState = GameState.PREGAME;
                    Time.timeScale = 1f;
                    Debug.Log("Pregame");
                }
                break;
        }
        OnGameStateChanged.Invoke(_currentGameState, previousGameState);
    }

    public void LoadLevel(string levelName) //Chargement d'un niveau
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
        if (ao == null)
        {
            Debug.LogError("[GameManager] Unable to load level " + levelName);
            return;
        }
        ao.completed += OnLoadOperationComplete;
        _loadOperations.Add(ao);
        _currentLevelName = levelName;
        _currentReputState = ReputState.SAFE;
    }

    public void UnloadLevel(string levelName) //Déchargement d'un niveau
    {
        AsyncOperation ao = SceneManager.UnloadSceneAsync(levelName);
        if (ao == null)
        {
            Debug.LogError("[GameManager] Unable to unload level " + levelName);
            return;
        }
        ao.completed += OnUnloadOperationComplete;
    }

    private void OnLoadOperationComplete(AsyncOperation ao) //Fin de chargement de niveau
    {
        if (_loadOperations.Contains(ao))
        {
            _loadOperations.Remove(ao);
        }
        if (_loadOperations.Count == 0)
        {
            UpdateState(GameState.RUNNING);
        }
        Debug.Log("Load Complete");
    }

    private void OnUnloadOperationComplete(AsyncOperation ao) //Fin de déchargement de niveau
    {
        Debug.Log("Unload Complete");
    }

    private void Start()
    {
        _loadOperations = new List<AsyncOperation>();
        _instanciedSystemPrefabs = new List<GameObject>();
        InstantiateSystemPrefabs();
    }

    void InstantiateSystemPrefabs() //On instancie tous les éléments nécessaires au système de jeu (Les managers comme UIManager)
    {
        GameObject prefabInstance;
        for (int i = 0; i < SystemPrefabs.Length; i++)
        {
            prefabInstance = Instantiate(SystemPrefabs[i]);
            _instanciedSystemPrefabs.Add(prefabInstance);
        }
    }

    protected override void OnDestroy()
    {
        for (int i = 0; i < _instanciedSystemPrefabs.Count; i++)
        {
            Destroy(_instanciedSystemPrefabs[i]);
        }
        _instanciedSystemPrefabs.Clear();
        base.OnDestroy();
    }

    public void StartGame(string levelname) //Lance un niveau
    {
        LoadLevel(levelname);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) //Si on appuie sur echap...
        {

            if (_currentGameState != GameState.RUNNING && _currentGameState != GameState.PAUSED) //... et que le State actuel est Pause ou Running
            {
                return;
            }
            TogglePause(); //On met le jeu en pause ou on le reprend depuis une pause
        }

    }


}