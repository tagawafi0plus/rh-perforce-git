using System;
using System.Collections.Generic;
using Live.Scripts.Enum;
using Live.Scripts.View.Note;
using UnityEngine;

namespace Live.Scripts.Logic
{
    public class LiveGameLogic
    {
        public List<NoteItem> noteItems;

        // Perfect
        private int autoParam = 16 * 3;
        // Perfect
        private int param1 = 300;
        // Great
        private int param2 = 400;
        // Good
        private int param3 = 500;

        /**
         * タッチ開始判定
         * 0: miss
         * 1: normal
         * 2: perfect
         */
        public int DetectTouchStart(int xId, float timeSec)
        {
            // 現在時間
            var elapsedFromStartTimeSec = timeSec;

            // ノート判定
            foreach (var note in noteItems)
            {
                // 位置違い
                if (note.xId != xId) continue;
                // TouchStart待ち判定
                if (!note.isWaitTap() && !note.isWaitLinkStart()) continue;

                // ヒット領域判定
                var distance = note.GetDistance(elapsedFromStartTimeSec);
                distance = Math.Abs(distance);

                if (distance > param3)
                {
                    continue;
                }

                // 単体ノーツインスタンスを削除
                if (note.isWaitTap())
                {
                    Debug.LogWarning("1. 単体ノーツの成功");
                    DestroyNote(note);
                }
                else if (note.isWaitLinkStart())
                {
                    Debug.LogWarning("2. 連結ノーツの開始成功");
                }

                // ステートの更新
                note.UpdateState();

                if (distance < param1)
                {
                    return 2;
                }
                else if (distance < param2)
                {
                    return 1;
                }
                else if (distance < param3)
                {
                    return 0;
                }
            }

            return -1;
        }

        /**
         * タッチ終了判定
         * 0: miss
         * 1: normal
         * 2: perfect
         */
        public int DetectTouchEnd(int xId, float timeSec)
        {
            // 現在時間
            var elapsedFromStartTimeSec = timeSec;

            // ノート判定
            foreach (var note in noteItems)
            {
                // 位置違い
                if (note.xId != xId) continue;

                // 連結ノーツ中判定
                if (!note.isLinkStarted()) continue;

                // 連結した子ノーツで判定する
                var children = note.children;
                foreach (var child in children)
                {
                    // 終了ノーツとの領域判定
                    var distance = child.GetDistance(elapsedFromStartTimeSec);
                    distance = Math.Abs(distance);

                    // 失敗
                    if (distance > param3)
                    {
                        Debug.LogWarning("Miss: 終了ノーツの判定前に離した");
                        // 親ノーツを削除して子ノーツも削除
                        DestroyNote(note);
                        DestroyChildren(note);
                        note.state = NoteState.End;
                        return 0;
                    }

                    // 正常系
                    if (distance < param1)
                    {
                        Debug.LogWarning("3. 連結ノーツの終了成功 2");
                        note.state = NoteState.End;
                        return 2;
                    }
                    else if (distance < param2)
                    {
                        Debug.LogWarning("3. 連結ノーツの終了成功 1");
                        note.state = NoteState.End;
                        return 1;
                    }
                    else if (distance < param3)
                    {
                        Debug.LogWarning("3. 連結ノーツの終了成功 0");
                        note.state = NoteState.End;
                        return 0;
                    }
                }
            }

            return -1;
        }

        public bool DetectAuto(NoteItem note, float timeSec)
        {
            // 現在時間
            var elapsedFromStartTimeSec = timeSec;

            // TouchStart待ち判定
            if (!note.isWaitTap() && !note.isWaitLinkStart()) return false;

            // ヒット領域判定
            var distance = note.GetDistance(elapsedFromStartTimeSec);
            distance = Math.Abs(distance);

            return distance < autoParam;
        }

        public void DestroyNote(NoteItem note)
        {
            if (note == null)
            {
                return;
            }

            // 連結ノーツの開始点をタップ漏れした場合
            if (note.isWaitLinkStart())
            {
                Debug.LogWarning("Miss: 連結ノーツの開始点をタップ漏れ");
                DestroyChildren(note);
            }

            note.Destroy();
            if (!note.isLinkStarted())
            {
                note.state = NoteState.End;
            }
        }

        private void DestroyChildren(NoteItem note)
        {
            if (note == null)
            {
                return;
            }

            // 子ノーツを削除
            var children = note.children;
            foreach (var child in children)
            {
                child.Destroy();
                child.state = NoteState.End;
            }
        }
    }
}