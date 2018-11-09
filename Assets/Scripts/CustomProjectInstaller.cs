using System;
using Scripts.Sound;
using Scripts.Sound.Cri;
using Scripts.Sound.Unity;
using UnityEngine;
using Zenject;
using Environment = Scripts.Sound.Cri.Environment;

public sealed class CustomProjectInstaller : MonoInstaller
{
    [SerializeField] private SoundManager soundManager;

    [SerializeField] private SoundVolumeStorage soundVolumeStorage;

    public override void InstallBindings()
    {
        Container.BindInstance<ISoundController>(soundManager).AsSingle();
        Container.BindInstance(soundVolumeStorage).AsSingle();
        
        var soundSettings = Container.Resolve<CriSoundGeneralSettings>();
        if (soundSettings.Environment == Environment.Cri)
            Container.BindInterfacesAndSelfTo<CriSoundVolumeController>().AsCached();
        else
            Container.BindInterfacesAndSelfTo<UnitySoundVolumeController>().AsSingle();
    }
}
