using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Sound.Cri
{
    public sealed class CriSingleSoundPlayer : IInnerSoundPlayer
    {
        private readonly CriAtomSource source;

        private readonly Dictionary<string, string> cueSheetDictionary = new Dictionary<string, string>();

        public CriSingleSoundPlayer(MonoBehaviour owner)
        {
            source = owner.gameObject.AddComponent<CriAtomSource>();
            source.player.AttachFader();
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(source);
        }

        public void AddCueSheet(CriAtomCueSheet cueSheet)
        {
            if (!CriSoundUtility.AddCueSheet(cueSheet))
                return;

            var acb = CriAtom.GetAcb(cueSheet.name);
            if (acb == null)
            {
                Debug.unityLogger.LogError(GetType().Name, "NotFound cue sheet");
                return;
            }

            foreach (var cue in acb.GetCueInfoList())
            {
                cueSheetDictionary.Add(cue.name, cueSheet.name);
            }
        }

        public void Play(string name)
        {
            string cueSheet = null;
            if (cueSheetDictionary.TryGetValue(name, out cueSheet))
            {
                source.cueSheet = cueSheet;
                source.Play(name);
            }
        }

        public void Stop()
        {
            source.Stop();
        }
    }
}
