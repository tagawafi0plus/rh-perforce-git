using SocialGame.Scene;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using UniRx;

public class UnitSelectView : MonoBehaviour
{
    [SerializeField] private Button nextButton = null;

    [SerializeField] private Button backButton = null;

    [Inject] private ISceneManager sceneManager = null;

    private void Start()
    {
        nextButton
            .OnClickAsObservable()
            .Subscribe(_ => sceneManager.Next(Scene.Live))
            .AddTo(this);

        backButton
            .OnClickAsObservable()
            .Subscribe(_ => sceneManager.Back())
            .AddTo(this);
    }
}
