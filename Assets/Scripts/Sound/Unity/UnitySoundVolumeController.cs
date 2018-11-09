using System;
using SocialGame.Sound;
using Scripts.Sound.Cri;
using Zenject;

namespace Scripts.Sound.Unity
{
    public sealed class UnitySoundVolumeController : IInitializable, IDisposable, ISoundVolumeController
    {
        [Inject] private SoundVolumeStorage storage;

        [Inject] private SoundVolumeController controller;
        
        public float Bgm {
            set
            {
                storage.Model.Bgm = value;
                var settings = controller.Settings;
                settings.BGM = value;
                controller.Settings = settings;
            }
            get
            {
                return storage.Model.Bgm;
            }
        }

        public float Se {
            set
            {
                storage.Model.Se = value;
                var settings = controller.Settings;
                settings.SE = value;
                controller.Settings = settings;
            }
            get
            {
                return storage.Model.Se;
            }
        }

        public float Voice {
            set
            {
                storage.Model.Voice = value;
                var settings = controller.Settings;
                settings.Voice = value;
                controller.Settings = settings;
            }
            get
            {
                return storage.Model.Voice;
            }
        }

        public void Initialize()
        {
            var settings = controller.Settings;
            settings.BGM = Bgm;
            settings.SE = Se;
            settings.Voice = Voice;
            controller.Settings = settings;
        }

        public void Dispose()
        {
            Save();
        }

        public void Save()
        {
            storage.Save();
        }
    }
}