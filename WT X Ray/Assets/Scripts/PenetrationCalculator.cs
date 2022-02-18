using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Uncategorized
{
    public class PenetrationCalculator : MonoBehaviour
    {
        #region Temp
        [Header("Temporary Things", order = 0)]
        private Vector3 _shootContactPointIn;
        private Vector3 _shootContactPointOut;
        #endregion

        #region Fields
        [Header("Fields", order = 1)]
        [SerializeField] private SimulationController _simulationController;
        [SerializeField] private PenetrationCalculatorUI _penetrationCalculatorUI;
        [SerializeField] private float _shootDistance;
        [SerializeField] private AnimationCurve _depreciationOverDistance;
        [SerializeField] private LayerMask _armorLayer;
        private RaycastHit _armorHit;
        private ArmorPanel _armorPanel;

        [SerializeField] private float _effectiveThickness;
        [SerializeField] private float _attackAngle;
        [SerializeField] private float _constructionalAngle;

        public enum PenetrationPossibility { Penetration_Possibility_Is_Low, Penetration_Not_Possible, Penetration_Is_Possible }
        [SerializeField] private PenetrationPossibility _penetrationPossibility;

        #endregion

        #region Functions
        RaycastHit ArmorHit()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray,out RaycastHit hit, Mathf.Infinity, _armorLayer, QueryTriggerInteraction.Ignore);
            return hit;
        }
        RaycastHit ArmorHitTransform()
        {
            Ray ray = new Ray(transform.position,transform.forward);
            Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _armorLayer, QueryTriggerInteraction.Ignore);
            return hit;
        }
        Vector3 LinePlaneIntersection(Vector3 shootAttackDirection, Vector3 shootHitPoint, Vector3 armorNormal, Vector3 armorPlaneBackPoint)
        {
            Vector3 diff = shootHitPoint - armorPlaneBackPoint;
            float prod1 = Vector3.Dot(diff, armorNormal);
            float prod2 = Vector3.Dot(shootAttackDirection, armorNormal);
            float prod3 = prod1 / prod2;
            return shootHitPoint - shootAttackDirection * prod3;
        }
        float GetEffectiveThickness(RaycastHit hit)
        {
            ArmorPanel armorPanel = hit.collider.GetComponent<ArmorPanel>();
            float thicknessAsMM = armorPanel.Thickness * 0.001f;
            Vector3 armorFrontSurface = hit.point;
            Vector3 armorBackSurface = hit.point + (hit.normal * -thicknessAsMM);
            Vector3 intersection = LinePlaneIntersection(Camera.main.transform.forward, armorFrontSurface, hit.normal, armorBackSurface);
            float distance = Vector3.Distance(hit.point, intersection) * 1000;

            _shootContactPointIn = hit.point;
            _shootContactPointOut = intersection;

            return distance;
        }
        float GetAttackAngle()
        {
            return Quaternion.Angle(Camera.main.transform.rotation, Quaternion.LookRotation(_armorHit.normal * -1));
        }
        float GetConstructionalAngle()
        {
            float angleLinear = Mathf.InverseLerp(0, 90, Vector3.Angle(_armorHit.normal, Vector3.up));
            float angleGaijinLogic = Mathf.Lerp(90, 0, angleLinear);
            return angleGaijinLogic;
        }
        #endregion

        #region Methods
        void Start()
        {
            
        }
        void Update()
        {
            Calculate();

        }
        private void LateUpdate()
        {
            UIUpdate();
        }
       
        void Calculate()
        {
            if (_simulationController.CurrentMode == SimulationController.Mode.PreviewPenetration)
            {
                _armorHit = ArmorHit();
                if (_armorHit.collider)
                {
                    _armorPanel = _armorHit.collider.GetComponent<ArmorPanel>();
                    _effectiveThickness = GetEffectiveThickness(_armorHit);
                    _attackAngle = GetAttackAngle();
                    _constructionalAngle = GetConstructionalAngle();

                }
            }
          
        }
        void UIUpdate()
        {
            if (_armorHit.collider && _simulationController.CurrentMode == SimulationController.Mode.PreviewPenetration)
            {
            _penetrationCalculatorUI.Show();
                _penetrationCalculatorUI.SetData(_armorPanel.ArmorType, _armorPanel.Thickness, _effectiveThickness, _attackAngle, _constructionalAngle, _penetrationPossibility);
            _penetrationCalculatorUI.UpdatePanelPosition();
            }
            else
            {
                _penetrationCalculatorUI.Hide();
            }

        }
        private void OnDrawGizmosSelected()
        {
            if (_simulationController.CurrentMode == SimulationController.Mode.PreviewPenetration)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(_shootContactPointIn, _shootContactPointOut);
            }
        }
        #endregion
    }
}
