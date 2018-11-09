using System;
using SocialGame.Data;
using Scripts.Sound.Cri;
using Zenject;
using MessagePack;

namespace Scripts.Sound
{
    [Serializable]
    [MessagePackObject]
    public class SoundVolume
    {
        [Key(0)] public float Bgm;
        [Key(1)] public float Se;
        [Key(2)] public float Voice;
    }

    public class SoundVolumeStorage : LocalStorageBase<SoundVolume>
    {
        [Inject] private CriSoundBgmSettings bgmSettings;
        
        [Inject] private CriSoundSeSettings seSettings;

        [Inject] private CriSoundVoiceSettings voiceSettings;
        
        protected override string FileName {
            get {
                return "VolumeConfig";
            }
        }

        protected override SoundVolume OnCreate()
        {
            return new SoundVolume()
            {
                Bgm = bgmSettings.DefaultVolume,
                Se = seSettings.DefaultVolume,
                Voice = voiceSettings.DefaultVolume,
            };
        }

        protected override void OnInitialize()
        {
        }

        #if UNITY_EDITOR
        [UnityEditor.CustomEditor(typeof(SoundVolumeStorage))] 
        protected class CustomInspector : CustomInspectorBase<SoundVolumeStorage, SoundVolume>
        {
        }
        #endif
    }
}