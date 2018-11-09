using System;
using System.IO;
using System.Collections;
using UnityEngine;
using UniRx;

namespace Scripts.Sound.Cri
{
    public static class CriSoundUtility
    {
        public static bool AddCueSheet(CriAtomCueSheet cueSheet)
        {
            if (cueSheet == null || string.IsNullOrEmpty(cueSheet.name))
                return false;

            CriAtom.AddCueSheet(
                cueSheet.name,
                cueSheet.acbFile,
                cueSheet.awbFile);
            return true;
        }

        public static bool RemoveCueSheet(CriAtomCueSheet cueSheet)
        {
            if (cueSheet == null || string.IsNullOrEmpty(cueSheet.name))
                return false;

            CriAtom.RemoveCueSheet(cueSheet.name);
            return true;
        }

        public static IObservable<byte[]> LoadAcfFile(string fileName)
        {
            string filePath = string.Format("{0}/{1}", Application.streamingAssetsPath, fileName);
            IObservable<byte[]> result = null;
#if UNITY_ANDROID && !UNITY_EDITOR
            var www = new WWW(filePath);
            return www.ToObservable().Select(_ => www.bytes);
#else
            result = Observable
                .Start(() => File.ReadAllBytes(filePath))
                .ObserveOnMainThread();
#endif
            return result;
        }
    }
}
