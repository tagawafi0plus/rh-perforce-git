using System;
using SocialGame.Internal.Sound;
using Zenject;
using UniRx;
using UnityEngine;

namespace SocialGame.Sound
{
    public sealed class SoundVolumeController : IInitializable, IDisposable, ISoundVolumeIntent
    {
        private readonly ReactiveProperty<VolumeSettings> _settings = new ReactiveProperty<VolumeSettings>();

        public VolumeSettings Settings {
            get { return _settings.Value; }
            set { _settings.Value = value; }
        }

        void IInitializable.Initialize()
        {
            var settings = new VolumeSettings() {
                Master = 1.0f,
                BGM = 1.0f,
                SE = 1.0f,
                Voice = 1.0f,
            };
             _settings.Value = settings;
        }

        void IDisposable.Dispose()
        {
            
        }

        #region ISoundVolumeIntent implementation
        IObservable<float> ISoundVolumeIntent.OnMasterVolumeAsObservable()
        {
            return _settings
                .Select(x => x.Master);
        }

        IObservable<float> ISoundVolumeIntent.OnBGMVolumeAsObservable()
        {
            return _settings
                .Select(x => x.BGM);
        }

        IObservable<float> ISoundVolumeIntent.OnSEVolumeAsObservable()
        {
            return _settings
                .Select(x => x.SE);
        }

        IObservable<float> ISoundVolumeIntent.OnVoiceVolumeAsObservable()
        {
            return _settings
                .Select(x => x.Voice);
        }
        #endregion
    }
}
