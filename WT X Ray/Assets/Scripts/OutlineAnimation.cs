using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Uncategorized
{
   [AddComponentMenu(nameof(Project) + "/" + nameof(Uncategorized) + "/OutlineAnimation")]
    public class OutlineAnimation : MonoBehaviour
    {
        #region Temp
        //[Header("Temporary Things", order = 0)]
        #endregion

        #region Fields
        [Header("Fields", order = 1)]
        [SerializeField] private HighlightPlus.HighlightEffect blinkRedYellow;
        [SerializeField] private float speed;
        [SerializeField] private Color colorA;
        [SerializeField] private Color colorB;
        private float time;
        private bool reverse;
        #endregion

        #region Functions

        #endregion



        #region Methods
       
       void Update()
        {
            if (!reverse)
            {
                time += Time.deltaTime * speed;
                if(time >= 1)
                {
                    reverse = true;
                }
            }
            else
            {
                time -= Time.deltaTime * speed;
                if (time <= 0)
                {
                    reverse = false;
                }
            }
            Color blinkColor = Color.Lerp(colorA, colorB, time);
            blinkRedYellow.outlineColor = blinkColor;
        }
        #endregion
    }
}
