using SocialGame.Scene;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using UniRx;

public class LiveSelectView : MonoBehaviour
{
    [Inject] private ISceneManager sceneManager = null;

    [SerializeField] private Button nextButton = null;

    private void Start()
    {
        nextButton
            .OnClickAsObservable()
            .Subscribe(_ => sceneManager.Next(Scene.UnitSelect))
            .AddTo(this);
    }
}
