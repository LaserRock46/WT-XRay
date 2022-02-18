using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Uncategorized
{
    public class ArmorPanel : MonoBehaviour
    {
        #region Temp
        //[Header("Temporary Things", order = 0)]
        #endregion

        #region Fields
        [Header("Fields", order = 1)]
        [SerializeField] private int _thickness;
        public int Thickness { get { return _thickness; } private set { _thickness = value; } }

        [SerializeField] private ArmorTypesData.ArmorType _armorType;
        public ArmorTypesData.ArmorType ArmorType { get { return _armorType; } private set { _armorType = value; } }

        [SerializeField] private Transform _vehicleCenter;
        [SerializeField] private Transform _armorNormal;
        public Vector3 ArmorNormal { get { return _armorNormal.forward; } private set { _armorNormal.forward = value; } }

        [SerializeField] private float _constructionalAngle;
        public float ConstructionalAngle { get { return _constructionalAngle; } private set { _constructionalAngle = value; } }
        #endregion

        #region Functions
        Vector3 GetRayDirection()
        {
            Vector3 meshBoundsSize = GetComponent<MeshFilter>().sharedMesh.bounds.size;
            Vector3 boundsOrientation = new Vector3();
            if(meshBoundsSize.x > meshBoundsSize.z && meshBoundsSize.x > meshBoundsSize.y)
            {

            }

            return Vector3.zero;
        }
        #endregion

        #region Methods
        [ContextMenu(nameof(CalculateConstructionalAngle))]
        void CalculateConstructionalAngle()
        {
            if(_armorNormal == null)
            {
                GameObject armorNormalGO = new GameObject(nameof(_armorNormal));
                armorNormalGO.transform.SetParent(transform);
                _armorNormal = armorNormalGO.transform;
            }

            
        }
        private void OnDrawGizmosSelected()
        {
            if (_armorNormal)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawRay(_armorNormal.position, ArmorNormal);
            }
        }
        #endregion
    }
}
