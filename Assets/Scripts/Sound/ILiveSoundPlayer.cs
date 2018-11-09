using System;
using UniRx;

namespace Scripts.Sound
{
    public interface ILiveSoundPlayer : IDisposable
    {
        uint EstimatedLatency {
            get;
        }
        long Time {
            get;
        }
        long Length {
            get;
        }
        IObservable<Unit> Initialize(string name);
        void PlayBgm();
        void PlayBgm(long seek);
        void PauseBgm();
        void PlayPerfect();
        void PlayNormal();
        void PlayUnperfect();
    }
}
