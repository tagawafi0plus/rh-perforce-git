using UnityEngine;

namespace Live.Scripts.Effect
{
    [ExecuteInEditMode()]
    public class LineMeshTest : MonoBehaviour
    {
        public Material mat;

        [SerializeField, Range(0, 100)] public float paramA = 0;
        [SerializeField, Range(0, 100)] public float paramB = 45.1f;

        [SerializeField, Range(0.01f, 2)] public float ratioY = 0.17f;

        [SerializeField, Range(-1000, 1000)] public float ix = 0.0f;
        [SerializeField, Range(-5, 5)] public float dx = 0.0f;

        [SerializeField, Range(0, 3000)] public float iz = 1719;
        [SerializeField, Range(-100, 100)] public float dz = -17.5f;

        [SerializeField, Range(1, 100)] public float lineWidth = 100.0f;


        [SerializeField, Range(0, 1)] public float startRatio = 0.0f;
        [SerializeField, Range(0, 1)] public float endRatio = 1.0f;


        private LineMeshEffect lineEffect;
        private MeshFilter meshFilter;
        private MeshRenderer meshRenderer;

        void OnGUI()
        {
            // 編集用
            // init();
            // draw();
        }

        public void init()
        {
            meshFilter = GetComponent<MeshFilter>();
            meshRenderer = GetComponent<MeshRenderer>();
            lineEffect = new LineMeshEffect();
        }

        public void draw()
        {
            // パラメーターの更新
            lineEffect.ix = ix;
            lineEffect.dx = dx;
            lineEffect.iz = iz;
            lineEffect.dz = dz;

            lineEffect.paramA = paramA;
            lineEffect.paramB = paramB;
            lineEffect.ratioY = ratioY;

            lineEffect.lineWidth = lineWidth;

            // メッシュの更新
            lineEffect.updateMesh(100, startRatio, endRatio);

            // レンダラーにセット
            meshRenderer.material = mat;
            meshFilter.sharedMesh = lineEffect.mesh;
        }
    }
}