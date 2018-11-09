using System;
using Zenject;

namespace Scripts.Sound.Cri
{
    public sealed class CriSoundVolumeController : IInitializable, IDisposable, ISoundVolumeController
    {
        [Inject] private SoundVolumeStorage storage;
        
        public float Bgm {
            set
            {
                storage.Model.Bgm = value;
                CriAtom.SetCategoryVolume("BGM", value);
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
                CriAtom.SetCategoryVolume("SE", value);
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
                CriAtom.SetCategoryVolume("Voice", value);
            }
            get
            {
                return storage.Model.Voice;
            }
        }

        public void Initialize()
        {
            CriAtom.SetCategoryVolume("BGM", Bgm);
            CriAtom.SetCategoryVolume("SE", Se);
            CriAtom.SetCategoryVolume("Voice", Voice);
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