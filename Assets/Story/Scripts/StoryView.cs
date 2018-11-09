using System.Collections;
using System.Collections.Generic;
using SocialGame.Scene;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using UniRx;

public class StoryView : MonoBehaviour
{
    [SerializeField] private Button menuButton = null;

    [Inject] private ISceneManager sceneManager = null;

    private void Start()
    {
        menuButton
            .OnClickAsObservable()
            .Subscribe(_ => sceneManager.Next(Scene.Home))
            .AddTo(this);
    }

}
