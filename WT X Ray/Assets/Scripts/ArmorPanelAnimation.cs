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
        [SerializeField] private MeshRenderer _armorPanelMesh = null;
        [SerializeField] private AnimationCurve _alphaAnimationCurve = default;
        [SerializeField] private Material _armorPanelMat = null;
        [SerializeField] private HighlightPlus.HighlightEffect _whiteOutline;
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
            _whiteOutline.Refresh();
            StartCoroutine(DoAnimation());
        }
        private IEnumerator DoAnimation()
        {

            float time = 0f;
            while (time < _alphaAnimationCurve.keys[_alphaAnimationCurve.length - 1].time)
            {
                time += Time.deltaTime;
                _armorPanelMat.SetColor("_BaseColor", new Color(1, 1, 1, _alphaAnimationCurve.Evaluate(time)));
                _whiteOutline.outline = _alphaAnimationCurve.Evaluate(time)*3;

                yield return null;
            }

            _armorPanelMesh.enabled = false;
            yield return null;
        }
        private void OnCollisionEnter(Collision collision)
        {
            Hit();
        }
        #endregion
    }
}
