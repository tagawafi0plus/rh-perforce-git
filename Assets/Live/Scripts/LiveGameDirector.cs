using Live.Scripts.Data;
using Live.Scripts.Factory;
using Live.Scripts.Sound;
using Live.Scripts.View;
using UniRx;
using UnityEngine;

public class LiveGameDirector : MonoBehaviour
{
    // Prefabs.
    public NotePlayer notePlayer;
    public LiveSoundPlayer soundPlayerPrefab;
    public CanvasView canvas;
    public PrefabFactory prefabFactory;

    private LiveSoundPlayer soundPlayer;

    [SerializeField] private TuneData tuneData;

    public void Start()
    {
        Debug.Log("ゲーム開始");
        StartGame();
    }

    void Update()
    {
        UpdateGame();
    }

    void OnDestroy()
    {
        EndGame();
        Debug.Log("ゲーム終了");
    }

    // --------------------------------------------------
    // API
    // --------------------------------------------------
    public void StartGame()
    {
        // 曲データの作成
//        var tuneData = new TuneData();
        tuneData.onLoad.Subscribe(data =>
        {
            Debug.Log(data);
            Debug.Log("--------------end load JSON--------------");
            onLoadData(tuneData);
        });
        Debug.Log("--------------start load JSON--------------");
        // JSON読み込み方式
        tuneData.SetUp(3);
    }

    public void UpdateGame()
    {
        if (soundPlayer == null)
        {
            return;
        }
        // BGMの現在時間(秒)
        var elapsedFromStartTimeSec = soundPlayer.GetTimeSec();
        // ノートプレイヤーに時間をセット
        notePlayer.UpdateGame(elapsedFromStartTimeSec);
    }

    public void EndGame()
    {
        // ゲーム終了処理
        canvas.EndGame();
        soundPlayer.EndGame();
        notePlayer.EndGame();
    }

    private void onLoadData(TuneData tuneData)
    {
        // セットアップ
        soundPlayer = (LiveSoundPlayer) Instantiate(soundPlayerPrefab);
        notePlayer.SetTuneData(tuneData);

        notePlayer.canvas = canvas;
        notePlayer.soundPlayer = soundPlayer;
        notePlayer.prefabFactory = prefabFactory;

        // ゲーム開始
        canvas.StartGame();
        soundPlayer.StartGame();
        notePlayer.StartGame();        
    }
    
}