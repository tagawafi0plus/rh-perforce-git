using System;
using SocialGame.Sound;
using Scripts.Sound.Cri;
using Scripts.Sound.Unity;
using UnityEngine;
using Zenject;
using UniRx;
using Environment = Scripts.Sound.Cri.Environment;

namespace Scripts.Sound
{
    public interface IInnerSoundPlayer : ISoundPlayer, IDisposable
    {
        void AddCueSheet(CriAtomCueSheet cueSheet);
    }

    public sealed class SoundManager : MonoBehaviour, ISoundController
    {
        [Inject] private CriSoundGeneralSettings generalSettings;

        [Inject] private CriSoundBgmSettings bgmSettings;

        [Inject] private CriSoundSeSettings seSettings;

        [Inject] private CriSoundVoiceSettings voiceSettings;

        [Inject] private CriSoundLiveSettings liveSettings;

        [Inject] private SoundController unitySoundController;

        private IInnerSoundPlayer bgmPlayer;

        private IInnerSoundPlayer sePlayer;

        private IInnerSoundPlayer voicePlayer;

        public bool Initialized {
            private set;
            get;
        }

        public ISoundPlayer Bgm
        {
            get { return bgmPlayer; }
        }

        public ISoundPlayer Se
        {
            get { return sePlayer; }
        }

        public ISoundPlayer Voice
        {
            get { return voicePlayer; }
        }

        public ILiveSoundPlayer Live
        {
            private set;
            get;
        }

        private void Start()
        {
            CriSoundUtility
                .LoadAcfFile(generalSettings.AcfFile)
                .Subscribe(x => {
                    // initialize
                    if (generalSettings.Environment == Environment.Cri)
                    {
                        bgmPlayer = new CriSingleSoundPlayer(this);
                        sePlayer = new CriMultiSoundPlayer(this);
                        voicePlayer = new CriMultiSoundPlayer(this);
                        Live = new CriLiveSoundPlayer(this, liveSettings);
                    }
                    else
                    {
                        bgmPlayer = new UnityBgmPlayer(unitySoundController);
                        sePlayer = new UnitySePlayer(unitySoundController);
                        voicePlayer = new UnityVoicePlayer(unitySoundController);
                        Live = new UnityLiveSoundPlayer(this, unitySoundController, liveSettings);
                    }

                    CriAtomEx.RegisterAcf(x);

                    // add cue sheet
                    bgmPlayer.AddCueSheet(bgmSettings.CueSheet);
                    sePlayer.AddCueSheet(seSettings.CueSheet);
                    voicePlayer.AddCueSheet(voiceSettings.CueSheet);

                    Initialized = true;
                })
                .AddTo(this);
        }
        
        private void OnDestroy()
        {
            CriAtomEx.UnregisterAcf();
        }
    }
}
