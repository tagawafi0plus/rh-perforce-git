using System;
using SocialGame.Sound;

namespace Scripts.Sound.Unity
{
    public sealed class UnitySePlayer : IInnerSoundPlayer
    {
        private readonly SoundController controller;
        
        public UnitySePlayer(SoundController controller)
        {
            this.controller = controller;
        }

        public void Dispose()
        {
            
        }

        public void Play(string name)
        {
            controller.PlaySE((SE)Enum.Parse(typeof(SE), name));
        }

        public void Stop()
        {
            
        }

        public void AddCueSheet(CriAtomCueSheet cueSheet)
        {
            
        }
    }
}