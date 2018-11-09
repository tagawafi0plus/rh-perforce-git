using System.Collections;
using System.Collections.Generic;
using SocialGame.Scene;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using UniRx;

public class CharacterView : MonoBehaviour
{
    [SerializeField] private Button backButton = null;

    [Inject] private ISceneManager sceneManager = null;

    private void Start()
    {
        backButton
            .OnClickAsObservable()
            .Subscribe(_ => sceneManager.Back())
            .AddTo(this);
    }
}
