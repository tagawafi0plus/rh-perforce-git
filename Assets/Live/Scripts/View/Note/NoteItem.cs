using System;
using System.Collections.Generic;
using Live.Scripts.Effect;
using Live.Scripts.Enum;
using Live.Scripts.Factory;
using Live.Scripts.Util;
using Scripts.Util.Transform;
using UnityEngine;
using UnityEngine.UI;

namespace Live.Scripts.View.Note
{
    public class NoteItem
    {
        // 時間(ミリ秒)
        public float timeMsec;

        // XID
        public int xId = 0;

        // ビュー
        public GameObject view;

        // 横ライン
        public GameObject connecter;

        // ノーツステート
        public NoteState state = NoteState.KillUnit;

        // 子連結ノーツ配列
        public List<NoteItem> children;

        public LineMeshTest lineEffect;

        private LineMathUtil mathUtil;
        private LineMathUtil lineMathUtil;

        // 表示する時間(ミリ秒)
        private float activeTimeMsec = 2000;
        private Transform panelTransform;

        public NoteItem()
        {
            mathUtil = new LineMathUtil();

            mathUtil.paramA = 0;
            mathUtil.paramB = 45;

            mathUtil.ratioY = 0.185f;

            mathUtil.ix = -360;
            mathUtil.dx = 1.96f;
            mathUtil.ix = 0;
            mathUtil.dx = 0;

            mathUtil.iz = 1719;
            mathUtil.dz = -17.5f;
        }

        public bool hasLine { get; set; }
        public int lineLength { get; set; }

        public float GetDistance(float elapsedFromStartTimeSec)
        {
            var distance = timeMsec - elapsedFromStartTimeSec * 1000;
            return distance;
        }

        public void Destroy()
        {
            // ラインのDestroyad
            if (connecter)
            {
                GameObject.Destroy(connecter.gameObject);
                connecter = null;
            }
            // ラインのDestroyad
            if (lineEffect)
            {
                GameObject.Destroy(lineEffect.gameObject);
                lineEffect = null;
            }

            // 自身のDestroy
            if (view)
            {
                GameObject.Destroy(view.gameObject);
                view = null;
            }
        }

        public bool isStartOnce(float elapsedFromStartTimeSec)
        {
            return view == null && isKill() && isStart(elapsedFromStartTimeSec);
        }

        public bool isStart(float elapsedFromStartTimeSec)
        {
            // タイム判定
            var ratio = GetActiveTimeRatio(elapsedFromStartTimeSec);
            return ratio < 1;
        }

        public bool isEnd(float elapsedFromStartTimeSec)
        {
            // viewが定義されていない場合は終了済み
            if (view == null) return true;

            // タイム判定
            var ratio = GetActiveTimeRatio(elapsedFromStartTimeSec);
            return ratio < -0.2;
        }

        public void SetDebugText()
        {
            if (view == null) return;
            var debugText = view.GetComponentInChildren<Text>();
            debugText.text = "" + timeMsec + "\n" + xId;
            debugText.raycastTarget = false;
        }

        public void CreateView(Transform panelTransform, PrefabFactory factoryPrefab)
        {
            if (factoryPrefab == null) return;
            if (panelTransform == null) return;

            view = factoryPrefab.GetNote();
            view.transform.SetParent(panelTransform);
            view.transform.localScale = new Vector3(1, 1, 1);
        }

        public void CreateSpriteView(Transform panelTransform, PrefabFactory factoryPrefab)
        {
            if (factoryPrefab == null) return;
            if (panelTransform == null) return;

            view = factoryPrefab.GetNoteSprite();
            view.transform.SetParent(panelTransform);
            view.transform.localScale = new Vector3(100, 100, 100);

            mathUtil.ix = (xId - 2) * 320;

            this.panelTransform = panelTransform;

            // ラインインスタンス
            if (state == NoteState.KillLinkStart)
            {
                lineEffect = factoryPrefab.GetLineEffect();
                lineEffect.init();
            }

            // 同時連結ノーツの場合にラインを生成します
            if (hasLine)
            {
                lineMathUtil = mathUtil.Clone();

                var centerX = (float) xId + (float) lineLength * 0.5f;
                lineMathUtil.ix = (centerX - 2) * 320;

                connecter = factoryPrefab.GetNoteConnecter();
                connecter.transform.SetParent(panelTransform);
                connecter.transform.localScale = new Vector3(100, 100, 100);
            }
        }

        public void UpdateView(float elapsedFromStartTimeSec)
        {
            // ヒットまでの割合
            var ratio = GetActiveTimeRatio(elapsedFromStartTimeSec);

            // 直線移動
//            const int ix = 360;
//            const int iy = -905;
//            const int dx = 300;
//            var x = ix + dx * (xId % 5);
//            var y = iy + 1000 * ratio;
//            var z = 0;
            // 移動
//            view.transform.SetLocalPosition(x, y, z);

            // 曲線移動
            var end = mathUtil.GetFxPos(1 - ratio);
            var x = end.x + 960;
            var y = end.y;
            var z = end.z;
            // 移動
            view.transform.SetLocalPosition(x, y, z);

            // スケールの調整
            var scale = 10 + 90 * (1.0f - ratio);
            if (scale > 100) scale = 100;
            view.transform.SetLocalScale(scale, scale, scale);

            // ライン描画
            if (lineEffect)
            {
                lineEffect.transform.SetParent(panelTransform);
                lineEffect.transform.localScale = new Vector3(1, 1, 1);
                lineEffect.transform.localPosition = new Vector3(960, 0, 0);

                var childRatio = children[0].GetActiveTimeRatio(elapsedFromStartTimeSec);

                var minRatio = -0.2f;
                var maxRatio = 1.2f;
                lineEffect.startRatio = Mathf.Clamp(1 - childRatio, minRatio, maxRatio);
                lineEffect.endRatio = Mathf.Clamp(1 - ratio, minRatio, maxRatio);

                lineEffect.lineWidth = 70;

                lineEffect.ix = (xId - 2) * 320;
                lineEffect.draw();
            }

            // 横ラインがある場合
            if (connecter)
            {
                // 移動
                var lineEnd = lineMathUtil.GetFxPos(1 - ratio);
                var lineX = lineEnd.x + 960;
                var lineY = lineEnd.y;
                var lineZ = lineEnd.z;
                connecter.transform.SetLocalPosition(lineX, lineY, lineZ);

                // スケールの調整
                var _ratio = Math.Max(0.0f, ratio);
                var lineScaleX = 100.0f;
                if (_ratio > 0.8)
                {
                    lineScaleX = 100.0f * lineLength * 2.0f * (0.7f + (float)Math.Pow(_ratio, 2.0f));
                }
                else
                {
                    lineScaleX = 100.0f * lineLength * 2.0f * (1.1f + (float)Math.Pow(_ratio, 2.0f));
                }
                
                
                var lineScaleY = scale * 3;
                var lineScaleZ = scale;
                connecter.transform.SetLocalScale(lineScaleX, lineScaleY, lineScaleZ);
            }
        }

        // --------------------------------------------------
        // util
        // --------------------------------------------------
        public float GetActiveTimeRatio(float elapsedFromStartTimeSec)
        {
            // TODO：かけ算
            var ratio = (timeMsec - 1000 * elapsedFromStartTimeSec) / activeTimeMsec;
            return ratio;
        }

        private float GetNotePositionFromTime(float time, float SpeedPerSec)
        {
            const int iy = -1080 + 176;
            var y = iy + SpeedPerSec * time * 0.001f;
            return y;
        }

        public bool isKill()
        {
            return state == NoteState.KillUnit || state == NoteState.KillLinkStart ||
                   state == NoteState.KillLinkEnd;
        }

        public bool isWaitTap()
        {
            return state == NoteState.WaitTap;
        }

        public bool isWaitLinkStart()
        {
            return state == NoteState.WaitLinkStart;
        }

        public bool isLinkStarted()
        {
            return state == NoteState.LinkStarted;
        }

        public bool isWaitLinkEnd()
        {
            return state == NoteState.KillLinkEnd;
        }

        public void UpdateState()
        {
            switch (state)
            {
                default:
                    break;
                // 生成前
                case NoteState.KillUnit:
                    state = NoteState.WaitTap;
                    break;
                case NoteState.KillLinkStart:
                    state = NoteState.WaitLinkStart;
                    break;
                case NoteState.KillLinkEnd:
                    state = NoteState.WaitLinkEnd;
                    break;

                // 生成済み単体タップ待ち
                case NoteState.WaitTap:
                    state = NoteState.End;
                    break;


                // 生成済みTouchStart待ち
                case NoteState.WaitLinkStart:
                    state = NoteState.LinkStarted;
                    Debug.LogWarning("NoteState.LinkStarted");
                    break;
                // 生成済みTouchEnd待ち
                case NoteState.WaitLinkEnd:
                    state = NoteState.End;
                    Debug.LogWarning("NoteState.End");
                    break;

                // 終了
                case NoteState.End:
                    break;
            }

            if (!view) return;
            var debugText = view.GetComponentInChildren<Text>();
            if (debugText)
            {
                debugText.text = "" + state;
                debugText.raycastTarget = false;
            }
        }

        public void AddChildNote(NoteItem note)
        {
            if (children == null)
            {
                children = new List<NoteItem>();
            }

            children.Add(note);
        }
    }
}