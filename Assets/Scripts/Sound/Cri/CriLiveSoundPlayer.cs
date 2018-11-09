using System;
using UniRx;
using UnityEngine;

namespace Scripts.Sound.Cri
{
    public sealed class CriLiveSoundPlayer : ILiveSoundPlayer
    {
        private readonly CriAtomSource bgmSource;

        private readonly CriAtomSource seSource;

        private readonly CriAtomCueSheet cueSheet;

        private readonly CriSoundLiveSettings settings;

        public uint EstimatedLatency
        {
            private set;
            get;
        }

        public long Time
        {
            get {
                return bgmSource.time;
            }
        }

        public long Length
        {
            private set;
            get;
        }

        public CriLiveSoundPlayer(MonoBehaviour owner, CriSoundLiveSettings settings)
        {
            // intiailize
            this.cueSheet = new CriAtomCueSheet();
            this.bgmSource = owner.gameObject.AddComponent<CriAtomSource>();
            this.seSource = owner.gameObject.AddComponent<CriAtomSource>();
            this.settings = settings;
            // set low latency mode
            seSource.androidUseLowLatencyVoicePool = true;
        }

        public IObservable<Unit> Initialize(string name)
        {
            // set cue info
            cueSheet.name = name;
            cueSheet.acbFile = name + ".acb";
            cueSheet.awbFile = name + ".awb";

            if (!CriSoundUtility.AddCueSheet(cueSheet))
                throw new ArgumentException();

            CriAtomExAcb acb = CriAtom.GetAcb(name);
            CriAtomEx.CueInfo cueInfo;
            if (!acb.GetCueInfo(name, out cueInfo))
                throw new ArgumentException();
            Length = cueInfo.length;
            
            return Observable
                .ReturnUnit()
                .Do(_ => CriAtomExLatencyEstimator.InitializeModule())
                .SelectMany(_ => Observable
                    .EveryUpdate()
                    .Select(__ => CriAtomExLatencyEstimator.GetCurrentInfo())
                    .Where(x => x.status == CriAtomExLatencyEstimator.Status.Processing))
                .Do(x => EstimatedLatency = x.estimated_latency)
                .Do(_ => CriAtomExLatencyEstimator.FinalizeModule())
                .AsUnitObservable();
        }

        public void Dispose()
        {
            CriSoundUtility.RemoveCueSheet(cueSheet);
            UnityEngine.Object.Destroy(bgmSource);
            UnityEngine.Object.Destroy(seSource);
        }

        public void PlayBgm()
        {
            if (bgmSource.IsPaused())
                bgmSource.Pause(false);
            else
                bgmSource.Play(cueSheet.name);
        }

        public void PlayBgm(long value)
        {
            bgmSource.player.SetStartTime(value);
            bgmSource.Stop();
            PlayBgm();
        }

        public void PauseBgm()
        {
            bgmSource.Pause(true);
        }

        public void PlayPerfect()
        {
            seSource.Play(settings.PerfectCueName);
        }

        public void PlayNormal()
        {
            seSource.Play(settings.NormalCueName);
        }

        public void PlayUnperfect()
        {
            seSource.Play(settings.UnperfectCueName);
        }
    }
}
