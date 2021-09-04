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

        [SerializeField] private Material[] _xRayMat;
        [SerializeField] private LayerMask _layerXRay;
        [SerializeField] private LayerMask _layerXRayRedOutline;
        [SerializeField] private LayerMask _layerXRayRedAnimated;

        [SerializeField] private MeshRenderer[] _componentRenderers;
        #endregion

        #region Functions

        #endregion



        #region Methods
        public void ResetComponent()
        {
            _currentDurability = _maxDurability;

        }
        void UpdateVisuals()
        {
            
            foreach (MeshRenderer meshRenderer in _componentRenderers)
            {
                meshRenderer.material = _xRayMat[0];
            }           
        }
        #endregion
    }
}
