using System;
using UnityEngine;
using Zenject;

namespace Scripts.Sound.Cri
{
    [Serializable]
    public enum Environment
    {
        Cri,
        Unity,
    }

    [Serializable]
    public sealed class CriSoundGeneralSettings
    {
        public Environment Environment;
        public string AcfFile;
    }

    [Serializable]
    public sealed class CriSoundBgmSettings
    {
        public float DefaultVolume;

        public CriAtomCueSheet CueSheet;
    }

    [Serializable]
    public sealed class CriSoundSeSettings
    {
        public float DefaultVolume;

        public CriAtomCueSheet CueSheet;
    }
    
    [Serializable]
    public sealed class CriSoundVoiceSettings
    {
        public float DefaultVolume;

        public CriAtomCueSheet CueSheet;
    }

    [Serializable]
    public sealed class CriSoundLiveSettings
    {
        public string PerfectCueName;
        public string NormalCueName;
        public string UnperfectCueName;
    }
    
    [CreateAssetMenu(fileName = "CriSoundSettings", menuName = "Installers/CriSoundSettings")]
    public sealed class CriSoundSettingsInstaller : ScriptableObjectInstaller<CriSoundSettingsInstaller>
    {
        [SerializeField] private CriSoundGeneralSettings general;

        [SerializeField] private CriSoundBgmSettings bgm;
        
        [SerializeField] private CriSoundSeSettings se;

        [SerializeField] private CriSoundVoiceSettings voice;

        [SerializeField] private CriSoundLiveSettings live;

        public override void InstallBindings()
        {
            Container.BindInstance(general).AsSingle();
            Container.BindInstance(bgm).AsSingle();
            Container.BindInstance(se).AsSingle();
            Container.BindInstance(voice).AsSingle();
            Container.BindInstance(live).AsSingle();
        }
    }
}
