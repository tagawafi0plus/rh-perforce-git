using System;
using SocialGame.Sound;

namespace Scripts.Sound.Unity
{
    public sealed class UnityBgmPlayer : IInnerSoundPlayer
    {
        private readonly SoundController controller;
        
        public UnityBgmPlayer(SoundController controller)
        {
            this.controller = controller;
        }

        public void Dispose()
        {
            
        }

        public void Play(string name)
        {
            controller.PlayBGM((BGM)Enum.Parse(typeof(BGM), name));
        }

        public void Stop()
        {
            controller.StopBGM();
        }

        public void AddCueSheet(CriAtomCueSheet cueSheet)
        {
            
        }
    }
}