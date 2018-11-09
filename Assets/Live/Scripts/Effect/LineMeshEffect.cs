using System.Collections.Generic;
using Live.Scripts.Util;
using UnityEngine;

namespace Live.Scripts.Effect
{
    public class LineMeshEffect
    {
        public float paramA = 0;
        public float paramB = 32;

        public float ratioY = 0.14f;

        public float ix = 0.0f;
        public float dx = 0.0f;

        public float iz = 775;
        public float dz = -17.5f;

        public float lineWidth = 100.0f;

        public Mesh mesh;

        private LineMathUtil mathUtil;
        private List<Vector3> vertices = new List<Vector3>();
        private List<Vector2> uvs = new List<Vector2>();
        private List<int> triangles = new List<int>();


        public LineMeshEffect()
        {
        }

        public void updateMesh(int num, float startRatio, float endRatio)
        {
            if (mathUtil == null)
            {
                mathUtil = new LineMathUtil();
            }

            vertices = new List<Vector3>();
            uvs = new List<Vector2>();
            triangles = new List<int>();

            if (!mesh)
            {
                mesh = new Mesh();
            }

            mesh.uv = uvs.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.vertices = vertices.ToArray();

            // 関数カーブ
            buildPolygon(num, startRatio, endRatio);
        }

        private void buildPolygon(int num, float startRatio, float endRatio)
        {
            mathUtil.ix = ix;
            mathUtil.dx = dx;
            mathUtil.iz = iz;
            mathUtil.dz = dz;

            mathUtil.paramA = paramA;
            mathUtil.paramB = paramB;
            mathUtil.ratioY = ratioY;

            var startIndex = startRatio;
            var point = mathUtil.GetFxPos(startIndex);
            var index = 0;

            for (var i = 0; i < num; i++)
            {
                var ratio = (float) i / num;

                // range対応
                if (ratio < startRatio) continue;
                else if (ratio > endRatio) continue;

                var start = point;
                var end = mathUtil.GetFxPos(ratio);

                // 奥行きの太さ対応
                var _lineWidth = ratio * lineWidth;

                AddPolygon(start, end, _lineWidth, index++, num);
                point = end;
            }
        }

        private void AddPolygon(Vector3 start, Vector3 end, float sideLength, int index, float total)
        {
            if (!mesh)
            {
                return;
            }

            // movable camera type
//            var dir = end - start;
//            var cameraPos = Camera.main.transform.position;
//            var side = Vector3.Cross(dir, -Vector3.Normalize(cameraPos));
//            side = Vector3.Normalize(side);

            // fix camera type
            var side = Vector3.right;

            // ----------------------------------
            // Vertex
            // ----------------------------------
            var point0 = start + side * sideLength;
            var point2 = start - side * sideLength;
            var point1 = end + side * sideLength;
            var point3 = end - side * sideLength;

            if (vertices.Count > 3)
            {
                point0 = vertices[vertices.Count - 3];
                point2 = vertices[vertices.Count - 1];
            }

            // 左下
            vertices.Add(point0);
            // 左上
            vertices.Add(point1);
            // 右下
            vertices.Add(point2);
            // 右下
            vertices.Add(point3);

            // ----------------------------------
            // UV
            // ----------------------------------
            var v0 = 0 + 1 / total * index;
            var v1 = 0 + 1 / total * (index + 1);
            // 左下
            uvs.Add(new Vector2(1, v0));
            // 右下
            uvs.Add(new Vector2(1, v1));
            // 左上
            uvs.Add(new Vector2(0, v0));
            // 右上
            uvs.Add(new Vector2(0, v1));

            // ----------------------------------
            // Index
            // ----------------------------------
            var offset = index * 4;
            this.triangles.Add(offset);
            this.triangles.Add(offset + 1);
            this.triangles.Add(offset + 2);

            this.triangles.Add(offset + 1);
            this.triangles.Add(offset + 3);
            this.triangles.Add(offset + 2);

            // ----------------------------------
            // push
            // ----------------------------------
            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.uv = uvs.ToArray();
        }
    }
}