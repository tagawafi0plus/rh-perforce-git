using System.Collections.Generic;
using Live.Scripts.Action;
using Live.Scripts.View.Note;
using Live.Scripts.Data;
using Live.Scripts.Enum;
using Live.Scripts.Factory;
using Live.Scripts.Logic;
using Live.Scripts.Sound;
using UnityEngine;

namespace Live.Scripts.View
{
    public class NotePlayer : MonoBehaviour
    {
        public PrefabFactory prefabFactory { get; set; }
        public CanvasView canvas { get; set; }
        public LiveSoundPlayer soundPlayer { get; set; }

        private List<NoteItem> noteItems;

        // 判定ロジック
        private LiveGameLogic gameLogic;
        private UserAction userAction;

        // --------------------------------------------------
        // SetUp
        // --------------------------------------------------
        public void SetTuneData(TuneData data)
        {
            if (data == null) return;
            noteItems = data.noteItems;
            // ゲームロジックの作成
            gameLogic = new LiveGameLogic();
            gameLogic.noteItems = data.noteItems;

            userAction = new UserAction();
        }

        // --------------------------------------------------
        // API
        // --------------------------------------------------
        public void StartGame()
        {
        }

        public void UpdateGame(float elapsedFromStartTimeSec)
        {
            if (noteItems == null) return;

            // User Action check
            userAction.checkInput();
            var count = userAction.GetCount();
            for (int i = 0; i < count; i++)
            {
                // タップ状態の変更時に1回のみ実行
                var userActionState = userAction.GetInputState(i);
                
                if (userActionState == UserActionState.TapStart)
                {
                    OnButtonTap(i);
                    canvas.OnTapBtn(i, true);
                }
                else if(userActionState == UserActionState.TapEnd)
                {
                    OnButtonOut(i);
                    canvas.OnTapBtn(i, false);
                }
            }

            // canvasの更新
            canvas.UpdateGame(elapsedFromStartTimeSec);

            // ノートの更新
            foreach (var note in noteItems)
            {
                // 消滅判定
                var distance = note.GetDistance(elapsedFromStartTimeSec);
                if (distance < -2000)
                {
                    continue;
                }

                // 生成判定 (必要なタイミングでノートのビューを生成します)
                if (note.isStartOnce(elapsedFromStartTimeSec))
                {
                    note.CreateSpriteView(canvas.panel.transform, prefabFactory);
                    note.UpdateState();
                }

                // ビューの処理
                if (note.view)
                {
                    note.UpdateView(elapsedFromStartTimeSec);
                }

                // debug：オートプレイ
                if (canvas.toggle.isOn && gameLogic.DetectAuto(note, elapsedFromStartTimeSec))
                {
                    gameLogic.DestroyNote(note);
                    onPerfect(note.xId);
                }

                // 消滅判定
                if (note.view && note.isEnd(elapsedFromStartTimeSec))
                {
                    // 連結ノーツ以外消去
                    if (note.state == NoteState.WaitTap)
                    {                    
                        gameLogic.DestroyNote(note);
                    }
                }
            }
        }

        public void EndGame()
        {
        }

        // --------------------------------------------------
        // event
        // --------------------------------------------------
        public void OnButtonTap(int xId)
        {
            var timeSec = soundPlayer.GetTimeSec();
            var scoreIndex = gameLogic.DetectTouchStart(xId, timeSec);
            switch (scoreIndex)
            {
                case 2:
                    onPerfect(xId);
                    break;
                case 1:
                    onNormal(xId);
                    break;
                case 0:
                    onMiss(xId);
                    break;
                default:
                    onMiss(xId);
                    break;
            }
        }

        public void OnButtonOut(int xId)
        {
            var timeSec = soundPlayer.GetTimeSec();
            var scoreIndex = gameLogic.DetectTouchEnd(xId, timeSec);
            switch (scoreIndex)
            {
                case 2:
                    onPerfect(xId);
                    break;
                case 1:
                    onNormal(xId);
                    break;
                case 0:
                    onMiss(xId);
                    break;
                default:
                    break;
            }
        }

        // --------------------------------------------------
        // Score feed back
        // --------------------------------------------------
        private void onPerfect(int xId)
        {
            soundPlayer.PlayPerfect();
            EmitEffect(xId);
        }

        private void onNormal(int xId)
        {
            soundPlayer.PlayUnperfect();
        }

        private void onMiss(int xId)
        {
            soundPlayer.PlayNormal();
        }

        // --------------------------------------------------
        // Effect
        // --------------------------------------------------
        private void EmitEffect(int xId)
        {
            var effect = prefabFactory.GetHitEffect();
            float x = 360 + 300 * xId;
            const float y = -1080 + 176;
            effect.StartTweens(canvas.panel.transform, x, y);
        }
    }
}