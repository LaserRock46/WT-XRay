using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Uncategorized
{
   [AddComponentMenu(nameof(Project) + "/" + nameof(Uncategorized) + "/OutlineRefresh")]
    public class OutlineRefresh : MonoBehaviour
    {
        #region Temp
        //[Header("Temporary Things", order = 0)]
        #endregion

        #region Fields
        [Header("Fields", order = 1)]
        //[SerializeField] private HighlightPlus.HighlightEffect _outline;
        [SerializeField] private float _timeSinceLastRefresh;
        [SerializeField] private float _minTimeRefresh;
        [SerializeField] private bool _needRefresh;
        #endregion

        #region Functions

        #endregion



        #region Methods

        void Update()
        {
            _timeSinceLastRefresh += Time.deltaTime;
            if(_needRefresh == true && _timeSinceLastRefresh > _minTimeRefresh)
            {
                Refresh();
            }
        }

        public void NeedRefresh()
        {
            _needRefresh = true;       
        }
        void Refresh()
        {
                //_outline.Refresh();
                _needRefresh = false;
                _timeSinceLastRefresh = 0;
        }
        #endregion
    }
}
