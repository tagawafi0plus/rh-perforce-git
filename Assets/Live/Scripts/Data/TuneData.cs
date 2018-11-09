using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Live.Scripts.Enum;
using Live.Scripts.View.Note;
using UniRx;
using UnityEngine;

namespace Live.Scripts.Data
{
    public class TuneData : MonoBehaviour
    {
        // ノート配列
        public List<NoteItem> noteItems;

        public readonly Subject<string> onLoad = new Subject<string>();
        private string filePath = "/Live/Stub/kirakira_alamode_final.json";
        private string filePath2 = "/Live/Stub/kirakira_alamode_final2.json";

        // BPM (1分あたりの拍数)
        [SerializeField] private float BPM = 140.01f;

        // JSONファイル名
        [SerializeField] private string fileName = "live/kirakira_alamode_final3.json";

        public TuneData()
        {
        }

        public void SetUp(int mode)
        {
            // ノートの作成
            noteItems = new List<NoteItem>();

            // 固定データテスト
            if (mode == 0)
            {
                BPM = 140.01f;
                SetUpSimpleTune();
                onLoad.OnNext("complete");
            }
            // 固定データテスト
            else if (mode == 1)
            {
                BPM = 140.01f;
                SetUpRandomTune2(9.0f);
                SetUpRandomTune2(41.0f);
                SetUpRandomTune2(73.0f);
                SetUpRandomTune2(105.0f);
                SetUpRandomTune2(137.0f);
                SetUpRandomTune2(169.0f);

                SetUpRandomTune2(201.0f);
                SetUpRandomTune2(233.0f);
                SetUpRandomTune2(265.0f);
                SetUpRandomTune2(297.0f);

                SetUpRandomTune2(319.0f);
                SetUpRandomTune2(351.0f);
                SetUpRandomTune2(383.0f);
                SetUpRandomTune2(415.0f);
                onLoad.OnNext("complete");
            }
            // 外部ファイル読み込み
            else if (mode == 2)
            {
                ReadFromFile(Application.dataPath + filePath);
            }
            // 外部ファイル読み込み
            else if (mode == 3)
            {
                var path = fileName;

                // Android
                if (Application.platform == RuntimePlatform.Android)
                {
                    string filePath = string.Format("{0}/{1}", Application.streamingAssetsPath, path);
                    StartCoroutine("LoadFile", filePath);
                }
                // iOS
                else if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    string filePath = string.Format("{0}/{1}", Application.streamingAssetsPath, path);
                    StartCoroutine("LoadFile", filePath);
                }
                // それ以外
                else
                {
                    var filePath = Application.dataPath + "/StreamingAssets/" + path;
                    StartCoroutine("LoadFile", filePath);
                }
            }
        }

        private void SetUpSimpleTune()
        {
            var noteNum = 60;
            for (var i = 0; i < noteNum; i++)
            {
                var xId = i % 5;
                var index = 10 + i;

                var state = NoteState.KillUnit;
                var note = CreateOneNote(index, xId, state);
                noteItems.Add(note);
            }
        }

        private void SetUpRandomTune2(float startIndex)
        {
            float[] indexItems =
            {
                0,
                1,
                2,
                2.5f,
                3,

                4,
                5,
                6,
                6.5f,
                7,

                8,
                9,
                10,
                10.5f,
                11,

                12,
                13,
                14,
                14.5f,
                15,

                17,
                17.5f,

                18.5f,
                19,

                20,
                20.5f,
                21
            };
//            SetUpTuneByArray(startIndex, indexItems);
        }

        private void SetUpTuneByArray(float startIndex, NoteData[] indexItems)
        {
            var noteNum = indexItems.Length;
            for (var i = 0; i < noteNum; i++)
            {
                var noteData = indexItems[i];
                var index = noteData.index;

                NoteItem parentNode = null;

                var notesLength = noteData.notes.Length;
                for (var j = 0; j < notesLength; j++)
                {
                    var child = noteData.notes[j];
                    var xId = child.x;
                    var state = child.state;
                    var offset = child.offset;

                    var note = CreateOneNote((float) index + offset, xId, state);
                    noteItems.Add(note);

                    // 連結子ノーツを親ノーツに追加
                    switch (state)
                    {
                        case NoteState.KillUnit:
                            // 横連結ノーツの場合
                            if (notesLength > 1)
                            {
                                if (j == 0)
                                {
                                    var lastChild = noteData.notes[notesLength - 1];
                                    note.hasLine = true;
                                    note.lineLength = lastChild.x - xId;

                                    parentNode = note;
                                }
                                else if (j == notesLength - 1)
                                {
                                    parentNode?.AddChildNote(note);
                                }
                            }

                            break;
                        case NoteState.KillLinkStart:
                            parentNode = note;
                            break;
                        case NoteState.KillLinkEnd:
                            parentNode?.AddChildNote(note);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private NoteItem CreateOneNote(float index, int xId, NoteState state)
        {
            var note = new NoteItem();
            // ノートに時間情報を付与する
            note.timeMsec = GetNoteTimeFromIndex(index);
            // 位置IDを付与
            note.xId = xId;
            // stateを付与
            note.state = state;
            return note;
        }

        // --------------------------------------------------
        // util
        // --------------------------------------------------
        private float GetNoteTimeFromIndex(float index)
        {
            // ノートの時間(分)
            var timeMin = index / BPM;
            // ノートの時間(ミリ秒)
            var timeMSec = timeMin * 60 * 1000;
            return timeMSec;
        }

        // 読み込み関数
        private void ReadFromFile(string url)
        {
            // JSON読み込み
            FileInfo fi = new FileInfo(url);
            try
            {
                // 一行毎読み込み
                using (StreamReader sr = new StreamReader(fi.OpenRead(), Encoding.UTF8))
                {
                    var jsonString = sr.ReadToEnd();
                    ReadFromString(jsonString);
                }

                onLoad.OnNext("complete");
            }
            catch (Exception e)
            {
                // 改行コード
                Debug.Log("JSON読み込みエラー");
            }
        }

        // 読み込み関数
        IEnumerator LoadFile(string filePath)
        {
            Debug.Log("読み込み開始:" + filePath);
            // JSON読み込み
            WWW www = new WWW(filePath);
            yield return www;

            try
            {
                var jsonString = "";
                var txtBuffer = "";
                var txtReader = new StringReader(www.text);
                while ((txtBuffer = txtReader.ReadLine()) != null)
                {
                    jsonString = jsonString + txtBuffer;
                }

                ReadFromString(jsonString);

                onLoad.OnNext("complete");
            }
            catch (Exception e)
            {
                // 改行コード
                Debug.Log("JSON読み込みエラー");
            }
        }

        private void ReadFromString(string jsonString)
        {
            Debug.Log(jsonString);
            Item item = JsonUtility.FromJson<Item>(jsonString);

            Debug.Log("item id " + item.id);
            Debug.Log("item file_date " + item.file_date);
            Debug.Log("item name " + item.file_name);
            Debug.Log("item description " + item.description);
            Debug.Log("item bpm " + item.bpm);
            Debug.Log("item record_set " + item.record_set);
            BPM = item.bpm;

            SetUpTuneByArray(0.0f, item.record_set);
        }
    }
}

[Serializable]
public class Item
{
    public int id;
    public string file_date;
    public string file_name;
    public string description;
    public float bpm;
    public NoteData[] record_set;
}

[Serializable]
public class NoteData
{
    public float index;

    public NoteChild[] notes;

    // TODO：可変BPM対応
    public float bpm;
}

[Serializable]
public class NoteChild
{
    public int x;
    public NoteState state;
    public float offset;
}