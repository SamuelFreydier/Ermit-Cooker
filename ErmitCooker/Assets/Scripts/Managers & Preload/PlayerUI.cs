using UnityEngine;
using UnityEngine.UI;
#pragma warning disable 0649

public class PlayerUI : MonoBehaviour
{
    [SerializeField] Animation _playerUIAnimator;
    [SerializeField] AnimationClip _fadeOutAnimationClip;
    [SerializeField] AnimationClip _fadeInAnimationClip;
    [SerializeField] private GameObject _timerPanel;
    [SerializeField] private GameObject _reputationPanel;
    [SerializeField] private TMPro.TextMeshProUGUI Timer;
    [SerializeField] private Image reputBar; //Barre de vie
    public Events.EventFadeComplete OnUIFadeComplete;

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);
    }

    private void Update()
    {
        UpdateTimerText(); //On update le timer de l'UI
    }

    public void UpdateReputBar(float fill) //On update la barre de vie
    {
        reputBar.fillAmount += fill;
    }


    void HandleGameStateChanged(GameManager.GameState currentstate, GameManager.GameState previousstate)
    {
        if (previousstate == GameManager.GameState.RUNNING && currentstate != GameManager.GameState.RUNNING) //Si le précédent state était WIN et le nouveau ne l'est pas
        {
            FadeOut(); //On sort de l'UI
        }
        if (previousstate != GameManager.GameState.RUNNING && previousstate != GameManager.GameState.PAUSED && currentstate == GameManager.GameState.RUNNING) //Quand on repasse en RUNNING depuis un menu
        {
            reputBar.fillAmount = 1.0f; //On réinitialise les barres de vie et d'armure
        }
    }

    private void UpdateTimerText() //Update du timer
    {
        if (GameManager.Instance.CurrentGameState == GameManager.GameState.RUNNING)
        {
            Timer.text = ((int)ScoreManager.Instance.TimeLvl1 / 60).ToString()
                + ":"
                + (ScoreManager.Instance.TimeLvl1 % 60).ToString("f0");
        }
    }

    public void OnFadeOutComplete() //Fin de sortie
    {
        OnUIFadeComplete.Invoke(true); //On envoie l'event avec le choix entre false -> prochain niveau et true -> menu
        gameObject.SetActive(false); //On désactive le menu
    }

    public void OnFadeInComplete() //Fin d'entrée
    {
        OnUIFadeComplete.Invoke(false); //On envoie l'event
    }

    public void FadeIn() //Début d'entrée
    {
        Debug.Log("FadeInUI");
        gameObject.SetActive(true); //On active l'objet
        CheckLevel();
        _playerUIAnimator.Stop(); //On active l'anim
        _playerUIAnimator.clip = _fadeInAnimationClip;
        _playerUIAnimator.Play();
    }

    public void FadeOut() //Début de sortie
    {
        _playerUIAnimator.Stop(); //On active l'anim
        _playerUIAnimator.clip = _fadeOutAnimationClip;
        _playerUIAnimator.Play();
    }

    private void CheckLevel()
    {
        switch (GameManager.Instance.LevelName) //On check en fonction du niveau les panels à activer ou non
        {
            case "Level1":
                {
                    _timerPanel.SetActive(true);
                }
                break;
        }
    }
}