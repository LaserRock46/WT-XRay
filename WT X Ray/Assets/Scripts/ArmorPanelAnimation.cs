using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Uncategorized
{
   [AddComponentMenu(nameof(Project) + "/" + nameof(Uncategorized) + "/ArmorPanelAnimation")]
    public class ArmorPanelAnimation : MonoBehaviour
    {
        #region Temp
        //[Header("Temporary Things", order = 0)]
        #endregion

        #region Fields
        [Header("Fields", order = 1)]
        private MeshRenderer _armorPanelMesh = null;
        [SerializeField] private AnimationCurve _alphaAnimationCurve = default;
        [SerializeField] private Material _armorPanelMat = null;
        //[SerializeField] private HighlightPlus.HighlightEffect _whiteOutline;
        [SerializeField] private UnityFx.Outline.OutlineSettings _whiteOutline;
        [SerializeField] private float _outlineWidth = 0.45f;
        #endregion

        #region Functions

        #endregion



        #region Methods
        private void Start()
        {
            _armorPanelMesh = GetComponent<MeshRenderer>();
        }

        [ContextMenu("Hit")]
        public void Hit()
        {
            _armorPanelMesh.enabled = true;
            
            StartCoroutine(DoAnimation());
        }
        private IEnumerator DoAnimation()
        {
            _whiteOutline.OutlineWidth = (int)_outlineWidth;

            float time = 0f;
            while (time < _alphaAnimationCurve.keys[_alphaAnimationCurve.length - 1].time)
            {
                time += Time.deltaTime;
                _armorPanelMat.SetColor("_BaseColor", new Color(1, 1, 1, _alphaAnimationCurve.Evaluate(time)));
                Color whiteColor = _whiteOutline.OutlineColor;
                whiteColor.a = _alphaAnimationCurve.Evaluate(time) * 3;
                _whiteOutline.OutlineColor = whiteColor;

                yield return null;
            }

            _armorPanelMesh.enabled = false;
            yield return null;
        }       
        #endregion
    }
}
