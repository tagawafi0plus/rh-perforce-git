using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using UniRx;

public interface IFooterMenuIntent
{
    IObservable<FooterState> OnClickAsObservable();
}

public class FooterMenuView : MonoBehaviour, IFooterMenuIntent
{
    [Inject] private IFooterMenuModel model = null;

    [SerializeField] private Button homeButton = null;

    [SerializeField] private Button characterButton = null;

    [SerializeField] private Button storyButton = null;

    [SerializeField] private Button liveButton = null;

    [SerializeField] private Button lotButton = null;

    [SerializeField] private Button menuButton = null;

    private readonly Subject<FooterState> onClick = new Subject<FooterState>();

    private void Start()
    {
        model
            .OnChangeStateAsObservable()
            .Subscribe(x => {
                homeButton.interactable = x != FooterState.Home;
                characterButton.interactable = x != FooterState.Character;
                storyButton.interactable = x != FooterState.Story;
                liveButton.interactable = x != FooterState.Live;
                lotButton.interactable = x != FooterState.Lot;
                menuButton.interactable = x != FooterState.Menu;
            })
            .AddTo(this);
        
        homeButton
            .OnClickAsObservable()
            .Subscribe(_ => onClick.OnNext(FooterState.Home))
            .AddTo(this);

        characterButton
            .OnClickAsObservable()
            .Subscribe(_ => onClick.OnNext(FooterState.Character))
            .AddTo(this);

        storyButton
            .OnClickAsObservable()
            .Subscribe(_ => onClick.OnNext(FooterState.Story))
            .AddTo(this);

        liveButton
            .OnClickAsObservable()
            .Subscribe(_ => onClick.OnNext(FooterState.Live))
            .AddTo(this);

        lotButton
            .OnClickAsObservable()
            .Subscribe(_ => onClick.OnNext(FooterState.Lot))
            .AddTo(this);
        
        menuButton
            .OnClickAsObservable()
            .Subscribe(_ => onClick.OnNext(FooterState.Menu))
            .AddTo(this);
    }

    public IObservable<FooterState> OnClickAsObservable()
    {
        return onClick;
    }
}
