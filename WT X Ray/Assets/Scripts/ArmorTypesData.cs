using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Uncategorized
{
    public class ArmorTypesData 
    {
         #region Temp
        //[Header("Temporary Things", order = 0)]
        #endregion

        #region Fields
        //[Header("Fields", order = 1)]
        public enum ArmorType {RolledHomogenousArmor, ConstructionalSteel, GunSteel, CastSteel, Wood, Stalinium, MolybdenumSteel }
        public static readonly Dictionary<ArmorType, float> ArmorTypeRHARatio = new Dictionary<ArmorType, float>
        {
            { ArmorType.RolledHomogenousArmor, 1.0f },
            { ArmorType.ConstructionalSteel, 0.9f },
            { ArmorType.GunSteel, 1.1f },
            { ArmorType.CastSteel, 0.95f },
            { ArmorType.Wood, 0.1f },
            { ArmorType.Stalinium, 1.2137f },
            { ArmorType.MolybdenumSteel, 1.15f },

        };
        #endregion

        #region Functions

        #endregion

        #region Methods

        #endregion
    }
}
