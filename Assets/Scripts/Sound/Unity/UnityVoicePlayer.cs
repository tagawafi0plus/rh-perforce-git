using System;
using SocialGame.Sound;

namespace Scripts.Sound.Unity
{
    public sealed class UnityVoicePlayer : IInnerSoundPlayer
    {
        private readonly SoundController controller;
        
        public UnityVoicePlayer(SoundController controller)
        {
            this.controller = controller;
        }

        public void Dispose()
        {
            
        }

        public void Play(string name)
        {
            controller.PlayVoice((Voice)Enum.Parse(typeof(Voice), name));
        }

        public void Stop()
        {
            controller.StopVoice();
        }

        public void AddCueSheet(CriAtomCueSheet cueSheet)
        {
            
        }
    }
}