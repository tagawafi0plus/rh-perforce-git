using System;
using SocialGame.Scene;
using Zenject;
using UniRx;

public enum FooterState
{
    Home,
    Character,
    Story,
    Live,
    Lot,
    Menu,
}

public interface IFooterMenuModel
{
    IObservable<FooterState> OnChangeStateAsObservable();
}

public class FooterMenuModel : IInitializable, IDisposable, IFooterMenuModel
{
    [Inject] private IFooterMenuIntent intent = null;

    [Inject] private ISceneManager sceneManager = null;

    private readonly ReactiveProperty<FooterState> state = new ReactiveProperty<FooterState>();

    private readonly CompositeDisposable disposables = new CompositeDisposable();

    void IInitializable.Initialize()
    {
        intent
            .OnClickAsObservable()
            .Do(x => state.Value = x)
            .Select(x => {
                switch(x) {
                case FooterState.Home: return Scene.Home;
                case FooterState.Character: return Scene.Character;
                case FooterState.Live: return Scene.LiveSelect;
                case FooterState.Lot: return Scene.Lot;
                default: return Scene.Home;
                }
            })
            .Subscribe(x => sceneManager.Next(x))
            .AddTo(disposables);
    }

    void IDisposable.Dispose()
    {
        disposables.Dispose();
    }

    public IObservable<FooterState> OnChangeStateAsObservable()
    {
        return state;
    }
}
