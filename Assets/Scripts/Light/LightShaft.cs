using UnityEngine;
using System.Collections;

public class LightShaft : MonoBehaviour {

    float _range = 0.0f;
    float _angle = 0.0f;
    float _intensity = 0.0f;

    Mesh lightShaft;

    // Use this for initialization
    void Start () {
        lightShaft = new Mesh();

        MeshFilter renderer = GetComponent<MeshFilter>();
        renderer.mesh = lightShaft;

        Light spotLight = GetComponent<Light>();
        _range = spotLight.range;
        _angle = spotLight.spotAngle;
        _intensity = spotLight.intensity;

        createMesh();
    }

    // Create Shaft mesh
    void createMesh() {
        float hNear = Mathf.Tan(((_angle/2.0f) / 360.0f) * 2 * Mathf.PI) * 1.0f;
        float hFar = Mathf.Tan(((_angle/2.0f) / 360.0f) * 2 * Mathf.PI) * _range;

        Vector3 up = this.transform.forward;
        Vector3 right = this.transform.right;
        Vector3 forward = this.transform.up;

        Vector3 corner0 = up * hNear + right * hNear + forward;
        Vector3 corner1 = up * hNear - right * hNear + forward;
        Vector3 corner2 = - up * hNear + right * hNear + forward;
        Vector3 corner3 = - up * hNear - right * hNear + forward;
        Vector3 corner4 = up * hFar + right * hFar + forward * _range;
        Vector3 corner5 = up * hFar - right * hFar + forward * _range;
        Vector3 corner6 = - up * hFar + right * hFar + forward * _range;
        Vector3 corner7 = -up * hFar - right * hFar + forward * _range;


        int nbPlane = (int)(_intensity * _range);
        Vector3[] vertices = new Vector3[nbPlane * 4];
        Vector2[] texcoord = new Vector2[nbPlane * 4];
        int[] indices = new int[nbPlane * 4];

        for (int i = 0; i < nbPlane; i++)
        {
            float index = i / (float)nbPlane;

            vertices[i * 4 + 0] = Vector3.Lerp(corner0, corner4, index);
            vertices[i * 4 + 1] = Vector3.Lerp(corner1, corner5, index);
            vertices[i * 4 + 2] = Vector3.Lerp(corner2, corner6, index);
            vertices[i * 4 + 3] = Vector3.Lerp(corner3, corner7, index);

            texcoord[i * 4 + 0] = Vector2.zero;
            texcoord[i * 4 + 1] = Vector2.up;
            texcoord[i * 4 + 2] = Vector2.right;
            texcoord[i * 4 + 3] = Vector2.one;

            indices[i * 4 + 0] = i * 4 + 0;
            indices[i * 4 + 1] = i * 4 + 1;
            indices[i * 4 + 2] = i * 4 + 3;
            indices[i * 4 + 3] = i * 4 + 2;
        }

        System.Collections.Generic.List<Vector3> vertexList = new System.Collections.Generic.List<Vector3>(vertices);
        lightShaft.SetVertices(vertexList);

        System.Collections.Generic.List<Vector2> texCoordList = new System.Collections.Generic.List<Vector2>(texcoord);
        lightShaft.SetUVs(0, texCoordList);

        lightShaft.SetIndices(indices, MeshTopology.Quads, 0);

        lightShaft.RecalculateNormals();
    }
}
