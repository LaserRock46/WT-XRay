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

        #endregion

        #region Functions
     
        #endregion

        #region Methods
        
        #endregion
    }
}
