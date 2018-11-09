using System;
using System.Linq;
using SocialGame.Sound;
using SocialGame.Internal.Sound;
using Scripts.Sound.Cri;
using UnityEngine;
using UniRx;

namespace Scripts.Sound.Unity
{
    public sealed class UnityLiveSoundPlayer : ILiveSoundPlayer
    {
        private readonly MonoBehaviour owner;

        private readonly SoundController controller;

        private readonly BGMSettings bgmSettings;

        private readonly CriSoundLiveSettings liveSettings;

        private AudioSource bgmSource;
        
        public uint EstimatedLatency
        {
            get { return 0; }
        }

        public long Time
        {
            get { return (long)(bgmSource.time * 1000.0f); }
        }

        public long Length
        {
            get { return (long)(bgmSource.clip.length * 1000.0f); }
        }

        public UnityLiveSoundPlayer(MonoBehaviour owner, SoundController controller, CriSoundLiveSettings settings)
        {
            this.owner = owner;
            this.controller = controller;
            this.bgmSettings = Resources.Load<SoundSettingsInstaller>("SoundSettings")._bgm;
            this.liveSettings = settings;
        }

        public IObservable<Unit> Initialize(string name)
        {
            if (bgmSource == null)
                bgmSource = owner.gameObject.AddComponent<AudioSource>();
            bgmSource.clip = bgmSettings.Clips.FirstOrDefault(x => x.name.Replace(' ', '_') == name);
            return Observable.ReturnUnit();
        }
        
        public void Dispose()
        {
            if (bgmSource == null)
                return;
            
            bgmSource.Stop();
            bgmSource.time = 0.0f;
            bgmSource.clip = null;
            UnityEngine.Object.DestroyImmediate(bgmSource);
            bgmSource = null;
        }

        public void PlayBgm()
        {
            if (bgmSource != null)
                bgmSource.Play();
        }

        public void PlayBgm(long value)
        {
            if (bgmSource != null)
                bgmSource.time = value / 1000.0f;
        }

        public void PauseBgm()
        {
            if (bgmSource != null)
                bgmSource.Pause();
        }

        public void PlayPerfect()
        {
            controller.PlaySE((SE)Enum.Parse(typeof(SE), liveSettings.PerfectCueName));
        }

        public void PlayNormal()
        {
            controller.PlaySE((SE)Enum.Parse(typeof(SE), liveSettings.NormalCueName));   
        }

        public void PlayUnperfect()
        {
            controller.PlaySE((SE)Enum.Parse(typeof(SE), liveSettings.UnperfectCueName));
        }
    }
}