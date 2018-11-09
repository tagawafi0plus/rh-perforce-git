using SocialGame.Scene;
using UnityEngine;
using Zenject;

public class FooterMenuInstaller : SceneInstaller
{
    [SerializeField] private FooterMenuView view;

    protected override void OnInstallBindings()
    {
        Container.BindInstance<IFooterMenuIntent>(view).AsSingle();
        Container.BindInterfacesAndSelfTo<FooterMenuModel>().AsSingle();
    }
}