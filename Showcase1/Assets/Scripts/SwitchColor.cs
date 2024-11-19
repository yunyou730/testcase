using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ayy
{
    public class SwitchColor : MonoBehaviour
    {
        private Material _mat = null;
        // Start is called before the first frame update
        void Start()
        {
            _mat = GetComponent<MeshRenderer>().material;
            var mesh = GetComponent<MeshFilter>().mesh;
            _mat.SetVector(Shader.PropertyToID("_BoundsMin"),new Vector4(mesh.bounds.min.x,mesh.bounds.min.y,mesh.bounds.min.z,1.0f));
            _mat.SetVector(Shader.PropertyToID("_BoundsMax"),new Vector4(mesh.bounds.max.x,mesh.bounds.max.y,mesh.bounds.max.z,1.0f));
        }
    
        // Update is called once per frame
        void Update()
        {
            //_mat.SetFloat(Shader.PropertyToID(""),);
        }
    }    
}
