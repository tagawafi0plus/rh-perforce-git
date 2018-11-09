using System.Collections;
using System.Collections.Generic;
using SocialGame.Scene;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using UniRx;

public class LiveFinalizer : MonoBehaviour
{
    [SerializeField] private Button homeButton;

    [Inject] private ISceneManager sceneManager;

    private void Start()
    {
        homeButton
            .OnClickAsObservable()
            .Subscribe(_ => Transition())
            .AddTo(this);
    }

    public void Transition()
    {
        // TODO 仮でHOMEに遷移
        sceneManager.Next(Scene.Home);
    }
}
