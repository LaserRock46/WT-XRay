using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Uncategorized
{
   [AddComponentMenu(nameof(Project) + "/" + nameof(Uncategorized) + "/VehicleComponent")]
    public class VehicleComponent : MonoBehaviour
    {
        #region Temp
        //[Header("Temporary Things", order = 0)]
        #endregion

        #region Fields
        [Header("Fields", order = 1)]
        [SerializeField] private int _maxDurability;
        [SerializeField] private int _currentDurabilityVisuals;
        [SerializeField] private int _currentDurabilitySimulation;
        public enum DamageMode { Simulation, Visualisation}
        public enum ComponentType {Other, Ammo, Crew, Fuel}
        [SerializeField] private ComponentType _componentType;

        [SerializeField] private Material[] _xRayMat;
        private string _layerXRay = "X Ray";
        private string _layerXRayRedOutline = "X Ray + Red Outline";
        private string _layerXRayRedAnimated = "X Ray + Red Outline Animated";
        [SerializeField] private HighlightPlus.HighlightEffect _redOutline;

        [SerializeField] private MeshRenderer[] _componentRenderers;
        [SerializeField] private SkinnedMeshRenderer[] _componentSkinnedRenderers;

        [SerializeField] private SimulationController _simulationController;
        #endregion

        #region Functions
        (int layer, int xRayMatIndex)GetLayerAndMatByDurability(int currentDurability)
        {
            float damagePercentage = 100 - ((float)currentDurability / (float)_maxDurability) * 100;
            int layer = 0;
            int xRayMatIndex = 0;

            switch ((int)damagePercentage)
            {
                case 0:
                    layer = LayerMask.NameToLayer(_layerXRay);
                    xRayMatIndex = 0;
                    break;
                case int p when p < 20:
                    layer = LayerMask.NameToLayer(_layerXRay);
                    xRayMatIndex = 1;
                    break;
                case int p when p < 40:
                    layer = LayerMask.NameToLayer(_layerXRay);
                    xRayMatIndex = 2;
                    break;
                case int p when p < 60:
                    layer = LayerMask.NameToLayer(_layerXRay);
                    xRayMatIndex = 3;
                    break;
                case int p when p < 80:
                    layer = LayerMask.NameToLayer(_layerXRay);
                    xRayMatIndex = 4;
                    break;
                case int p when p < 100:
                    layer = LayerMask.NameToLayer(_layerXRay);
                    xRayMatIndex = 5;
                    break;
                case 100:
                    layer = LayerMask.NameToLayer(_layerXRayRedAnimated);
                    xRayMatIndex = 6;
                    break;
            }
           
            return (layer, xRayMatIndex);
        }
        #endregion



        #region Methods
        private void Start()
        {
            ResetComponent();
        }
        public void ResetComponent()
        {
            _currentDurabilityVisuals = _maxDurability;
            _currentDurabilitySimulation = _maxDurability;
            UpdateVisuals();

        }
        void UpdateVisuals()
        {
            var layerAndMat = GetLayerAndMatByDurability(_currentDurabilityVisuals);
          
                foreach (MeshRenderer meshRenderer in _componentRenderers)
                {
                    meshRenderer.sharedMaterial = _xRayMat[layerAndMat.xRayMatIndex];
                    meshRenderer.gameObject.layer = layerAndMat.layer;
                }    
                foreach (SkinnedMeshRenderer skinnedMeshRenderer in _componentSkinnedRenderers)
                {
                    skinnedMeshRenderer.sharedMaterial = _xRayMat[layerAndMat.xRayMatIndex];
                    skinnedMeshRenderer.gameObject.layer = layerAndMat.layer;
                }
            
            _redOutline.Refresh();
        }
        public void Hit(int damage, DamageMode damageMode)
        {
            if (damageMode == DamageMode.Visualisation)
            {
                _currentDurabilityVisuals = Mathf.Clamp(_currentDurabilityVisuals - damage, 0, _maxDurability);
                UpdateVisuals();
            }
            else
            {
                _currentDurabilitySimulation = Mathf.Clamp(_currentDurabilitySimulation - damage, 0, _maxDurability);
                Result();
            }
        }
        void Result()
        {
            if(_currentDurabilitySimulation == 0)
            {
                _simulationController.NotifyAboutDestroyedComponent(_componentType);
            }
        }

        #endregion
    }
}
