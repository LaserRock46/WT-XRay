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
        [SerializeField] private int _currentDurability;
        public enum ComponentType {Other, Ammo, Crew, Fuel}
        [SerializeField] private ComponentType _componentType;

        [SerializeField] private Material[] _xRayMat;
        [SerializeField] private LayerMask _layerXRay;
        [SerializeField] private LayerMask _layerXRayRedOutline;
        [SerializeField] private LayerMask _layerXRayRedAnimated;
        [SerializeField] private HighlightPlus.HighlightEffect _redOutline;

        [SerializeField] private MeshRenderer[] _componentRenderers;

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
                    layer = _layerXRay;
                    xRayMatIndex = 0;
                    break;
                case int p when p < 20:
                    layer = _layerXRay;
                    xRayMatIndex = 1;
                    break;
                case int p when p < 40:
                    layer = _layerXRay;
                    xRayMatIndex = 2;
                    break;
                case int p when p < 60:
                    layer = _layerXRay;
                    xRayMatIndex = 3;
                    break;
                case int p when p < 80:
                    layer = _layerXRay;
                    xRayMatIndex = 4;
                    break;
                case int p when p < 100:
                    layer = _layerXRay;
                    xRayMatIndex = 5;
                    break;
                case 100:
                    layer = _layerXRayRedAnimated;
                    xRayMatIndex = 6;
                    break;
            }


            return (layer, xRayMatIndex);
        }
        #endregion



        #region Methods
        public void ResetComponent()
        {
            _currentDurability = _maxDurability;
            UpdateVisuals();

        }
        void UpdateVisuals()
        {
            var layerAndMat = GetLayerAndMatByDurability(_currentDurability);
            foreach (MeshRenderer meshRenderer in _componentRenderers)
            {
                meshRenderer.material =_xRayMat[layerAndMat.xRayMatIndex];
                meshRenderer.gameObject.layer = layerAndMat.layer;
            }
            _redOutline.Refresh();
        }
        public void Hit(int damage)
        {
            _currentDurability = Mathf.Clamp(_currentDurability - damage, 0, _maxDurability);
            UpdateVisuals();
            Result();
        }
        void Result()
        {
            if(_currentDurability == 0)
            {
                _simulationController.NotifyAboutDestroyedComponent(_componentType);
            }
        }

        #endregion
    }
}
