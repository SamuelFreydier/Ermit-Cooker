using UnityEngine;
using UnityEngine.UI;
#pragma warning disable 0649
public class MainMenu : MonoBehaviour
{
    [SerializeField] Animation _mainMenuAnimator;
    [SerializeField] AnimationClip _fadeOutAnimationClip;
    [SerializeField] AnimationClip _fadeInAnimationClip;
    [SerializeField] private Button levelButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private TMPro.TextMeshProUGUI timelvlText;
    public Events.EventFadeComplete OnMainMenuFadeComplete;
    public Events.EventLoadLevel OnMainMenuLevelLoad;
    Coroutine musicfade;


    private void Update()
    {
        //On met à jour les meilleurs temps dans le menu
        timelvlText.text = "Meilleur temps : " + ScoreManager.Instance.BestTime.ToString("f2") + " s";
    }
    private void Start()
    {

        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);
        levelButton.onClick.AddListener(HandleLoadLevel1Clicked);
        quitButton.onClick.AddListener(HandleQuitClicked);
    }

    //Selon le bouton, on envoie un event avec le nom du Level chargé (1 ou 2)
    private void HandleLoadLevel1Clicked()
    {
        if (!UIManager.Instance.AfterClick)
        {
            OnMainMenuLevelLoad.Invoke("Level1");
        }
    }

    private void HandleQuitClicked() //Le bouton Quit permet de quitter le jeu
    {
        Application.Quit();
    }

    void HandleGameStateChanged(GameManager.GameState currentstate, GameManager.GameState previousstate)
    {
        if (previousstate == GameManager.GameState.PREGAME && currentstate == GameManager.GameState.RUNNING) //Si l'ancien State était pregame et le nouveau Running
        {
            FadeOut(); //On part du menu
        }
        if (previousstate != GameManager.GameState.PREGAME && currentstate == GameManager.GameState.PREGAME) //Si l'ancien n'était pas pregame et le nouveau si
        {
            if (previousstate != GameManager.GameState.WIN)
            {
                GameManager.Instance.UnloadLevel(GameManager.Instance.LevelName); //On décharge le niveau
            }
            //FadeIn(); //On entre dans le menu
        }

    }

    public void OnFadeOutComplete() //Fin de sortie du menu
    {
        OnMainMenuFadeComplete.Invoke(true); //On envoie un event pour indiquer la fin de l'anim
        gameObject.SetActive(false); //On désactive le menu
        UIManager.Instance.SetDummyCameraActive(false); //On désactive la caméra UIManager
    }

    public void OnFadeInComplete() //Fin d'entrée dans le menu
    {
        OnMainMenuFadeComplete.Invoke(false); //On envoie un event pour indiquer la fin de l'anim
        //musicfade = StartCoroutine(AudioManager.Instance.Play("MenuMusic", 1f, 1f)); //On joue la musique de menu
    }

    public void FadeIn() //Début d'entrée
    {
        Debug.Log("FadIN");
        UIManager.Instance.SetDummyCameraActive(true); //Activation de la caméra
        _mainMenuAnimator.Stop(); //Activation de l'animation
        _mainMenuAnimator.clip = _fadeInAnimationClip;
        _mainMenuAnimator.Play();
    }

    public void FadeOut() //Début de sortie
    {
        //StopCoroutine(musicfade);
        //StartCoroutine(AudioManager.Instance.StopFadeOut("MenuMusic", .5f)); //On stoppe la musique du menu
        _mainMenuAnimator.Stop(); //Activation de l'animation
        _mainMenuAnimator.clip = _fadeOutAnimationClip;
        _mainMenuAnimator.Play();
    }
}