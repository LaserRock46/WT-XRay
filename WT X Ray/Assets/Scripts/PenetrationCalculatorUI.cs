using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Project.Uncategorized
{
    public class PenetrationCalculatorUI : MonoBehaviour
    {
        #region Temp
        //[Header("Temporary Things", order = 0)]
        #endregion

        #region Fields
        [Header("Fields", order = 1)]
        [SerializeField] private TMP_Text _shotDistance;

        [SerializeField] private Vector2 _penetrationDataPanelOffset;
        [SerializeField] private RectTransform _penetrationDataPanel;

        [SerializeField] private TMP_Text _armorType;
        [SerializeField] private TMP_Text _thickness;
        [SerializeField] private TMP_Text _effectiveThickness;
        [SerializeField] private TMP_Text _attackAngle;
        [SerializeField] private TMP_Text _constructionalAngle;
        [SerializeField] private TMP_Text _penetrationPossibility;



        #endregion

        #region Functions

        #endregion

        #region Methods
        public void Hide()
        {
            if(_penetrationDataPanel.gameObject.activeSelf)
            _penetrationDataPanel.gameObject.SetActive(false);
        }
        public void Show()
        {
            if (_penetrationDataPanel.gameObject.activeSelf == false)
                _penetrationDataPanel.gameObject.SetActive(true);
        }
        public void SetData(ArmorTypesData.ArmorType armorType, float thickness, float effectiveThickness, float attackAngle, float constructionalAngle, PenetrationCalculator.PenetrationPossibility penetrationPossibility)
        {
            _armorType.text = armorType.ToString();
            _thickness.text = Mathf.RoundToInt(thickness).ToString();
            _effectiveThickness.text = Mathf.RoundToInt(effectiveThickness).ToString();
            _attackAngle.text = Mathf.RoundToInt(attackAngle).ToString();
            _constructionalAngle.text = Mathf.RoundToInt(constructionalAngle).ToString();
            _penetrationPossibility.text = penetrationPossibility.ToString();

            switch (penetrationPossibility)
            {
                case PenetrationCalculator.PenetrationPossibility.Penetration_Possibility_Is_Low:
                    _penetrationPossibility.color = Color.yellow;
                    break;
                case PenetrationCalculator.PenetrationPossibility.Penetration_Not_Possible:
                    _penetrationPossibility.color = Color.red;
                    break;
                case PenetrationCalculator.PenetrationPossibility.Penetration_Is_Possible:
                    _penetrationPossibility.color = Color.green;
                    break;
            }
        }
        public void UpdatePanelPosition()
        {
            _penetrationDataPanel.position = (Vector2)Input.mousePosition + _penetrationDataPanelOffset;
        }
        #endregion
    }
}
