using SocialGame.Scene;
using Scripts.Sound;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using UniRx;

public class HomeView : MonoBehaviour
{
    [SerializeField] private Button liveButton;

    [Inject] private ISceneManager sceneManager;

    [Inject] private ISoundController soundController;
    
    private void Start()
    {
        liveButton
            .OnClickAsObservable()
            .Subscribe(_ => sceneManager.Next(Scene.Live))
            .AddTo(this);
    }
}
