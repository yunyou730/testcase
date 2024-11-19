using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ayy
{
    public class SwitchMaterial : MonoBehaviour
    {
        public Material[] _materials;

        private int _currentSlot = 0;
        private int _nextSlot = 1;
        
        [SerializeField,Range(0,1)]
        public float _pct = 1.0f;

        public float _speed = 3.0f;
        
        private Renderer _renderer = null;
        
        void Start()
        {
            _renderer = GetComponent<MeshRenderer>();
            
            InitAllMaterials();
            RefreshDisplayingMaterials();
        }
        
        void Update()
        {
            if (_pct < 1.0f)
            {
                float delta = _speed * Time.deltaTime;
                _pct += delta;
                _pct = Mathf.Clamp(_pct, 0, 1);
            }

            //_pct = 0.8f;
            _materials[_currentSlot].SetFloat(Shader.PropertyToID("_TestX"),_pct);
            _materials[_nextSlot].SetFloat(Shader.PropertyToID("_TestX"),_pct);
        }

        private void InitAllMaterials()
        {
            Vector3 boundsMin = GetComponent<MeshFilter>().mesh.bounds.min;
            Vector3 boundsMax = GetComponent<MeshFilter>().mesh.bounds.max;
            foreach (var mat in _materials)
            {
                mat.SetVector(Shader.PropertyToID("_BoundsMin"),new Vector4(boundsMin.x,boundsMin.y,boundsMin.z,1.0f));
                mat.SetVector(Shader.PropertyToID("_BoundsMax"),new Vector4(boundsMax.x,boundsMax.y,boundsMax.z,1.0f));
            }
        }
        
        private void RefreshDisplayingMaterials()
        {
            Material[] displayingMaterials =
            {
                _materials[_currentSlot],
                _materials[_nextSlot],
            };
            _renderer.materials = displayingMaterials;
            
            _materials[_currentSlot].SetFloat(Shader.PropertyToID("_IsGEClip"),1.0f);
            _materials[_nextSlot].SetFloat(Shader.PropertyToID("_IsGEClip"),0.0f);
        }

        public void OnClickSwitchMaterial()
        {
            GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
            Button button = clickedButton.GetComponent<Button>();
            //Debug.Log("btn name" + button.name);
            
            int targetIndex = int.Parse(button.name);
            if (targetIndex == _currentSlot)
            {
                Debug.Log("same color, return");
                return;
            }

            _nextSlot = _currentSlot;
            _currentSlot = targetIndex;
            RefreshDisplayingMaterials();

            _pct = 0.0f;
        }
    }
}
