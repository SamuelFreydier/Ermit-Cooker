using UnityEngine;
using UnityEngine.UI;
#pragma warning disable 0649
public class WinMenu : MonoBehaviour
{
    [SerializeField] Animation _winMenuAnimator;
    [SerializeField] AnimationClip _fadeOutAnimationClip;
    [SerializeField] AnimationClip _fadeInAnimationClip;
    [SerializeField] private Button ReplayButton;
    [SerializeField] private Button MenuButton;
    [SerializeField] private Button QuitButton;
    [SerializeField] private TMPro.TextMeshProUGUI Timer;
    public Events.EventLoseWinFadeComplete OnWinMenuFadeComplete;
    private bool choice;
    Coroutine musicfade;

    private void Start()
    {
        MenuButton.onClick.AddListener(HandleMenuClicked);
        ReplayButton.onClick.AddListener(HandleNextLevelClicked);
        QuitButton.onClick.AddListener(HandleQuitClicked);
    }
    private void Update()
    {
        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);
        UpdateTimerText();
    }
    private void HandleNextLevelClicked() //On charge le prochain niveau (forcément niveau 2)
    {
        choice = false; //false pour le choix "nextlevel"
        FadeOut(); //début de sortie du menu win
    }

    private void HandleMenuClicked()
    {
        choice = true; //true pour le choix "menu"
        FadeOut(); //début de sortie du menu win
    }

    private void HandleQuitClicked()
    {
        Application.Quit(); //on quitte le jeu
    }

    void HandleGameStateChanged(GameManager.GameState currentstate, GameManager.GameState previousstate)
    {
        if (previousstate == GameManager.GameState.WIN && currentstate != GameManager.GameState.WIN) //Si le précédent state était WIN et le nouveau ne l'est pas
        {
            FadeOut(); //On sort du menu Win
        }
    }

    private void UpdateTimerText() //On update le timer affiché en fonction du niveau courant
    {
        Timer.text = "Temps : "
            + ((int)ScoreManager.Instance.TimeLvl1 / 60).ToString()
            + ":"
            + (ScoreManager.Instance.TimeLvl1 % 60).ToString("f2");
    }

    public void OnFadeOutComplete() //Fin de sortie
    {
        OnWinMenuFadeComplete.Invoke(true, choice); //On envoie l'event avec le choix entre false -> prochain niveau et true -> menu
        gameObject.SetActive(false); //On désactive le menu
    }

    public void OnFadeInComplete() //Fin d'entrée
    {
        OnWinMenuFadeComplete.Invoke(false, choice); //On envoie l'event
        musicfade = StartCoroutine(AudioManager.Instance.Play("WinMusic", .8f, 1f)); //On lance la musique de victoire
    }

    public void FadeIn() //Début d'entrée
    {
        gameObject.SetActive(true); //On active l'objet
        if (GameManager.Instance.LevelName == "Level2")
        {
            ReplayButton.gameObject.SetActive(false);
        }
        else
        {
            ReplayButton.gameObject.SetActive(true);
        }
        UIManager.Instance.SetDummyCameraActive(true); //On active la caméra
        UpdateTimerText();
        _winMenuAnimator.Stop(); //On active l'anim
        _winMenuAnimator.clip = _fadeInAnimationClip;
        _winMenuAnimator.Play();
    }

    public void FadeOut() //Début de sortie
    {
        StopCoroutine(musicfade);
        AudioManager.Instance.InstantStop("WinMusic");
        _winMenuAnimator.Stop(); //On active l'anim
        _winMenuAnimator.clip = _fadeOutAnimationClip;
        _winMenuAnimator.Play();
    }
}