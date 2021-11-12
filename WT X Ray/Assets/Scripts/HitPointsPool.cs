using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Uncategorized
{
   [AddComponentMenu(nameof(Project) + "/" + nameof(Uncategorized) + "/HitPointsPool")]
    public class HitPointsPool : MonoBehaviour
    {
        #region Temp
        [Header("Temporary Things", order = 0)]       
        #endregion

        #region Fields
        [Header("Fields", order = 1)]   
        private MaterialPropertyBlock _materialPropertyBlock;
        private int _alphaNameID;
        [SerializeField] private float _hitPointFadeOutTime = 2.5f;
        [SerializeField] private float _hitPointAnimatedFadeOutTime = 1f;
        [SerializeField] private Mesh _hitPointMesh;
        [SerializeField] private float _hitPointScale = 0.075f;
        [SerializeField] private float _hitPointAnimatedScale = 0.2f;
        [SerializeField] private Material _hitPointMat;
        [SerializeField] private int _hitLayer = 11;

        #endregion

        #region Functions

        #endregion

        #region Methods
        void Start()
        {
            _materialPropertyBlock = new MaterialPropertyBlock();
            _alphaNameID = Shader.PropertyToID("_Alpha");
        } 
        public void GetHitPointHere(Vector3 position,bool animatedScale = false)
        {       
            Matrix4x4 matrix = new Matrix4x4();

            matrix.m00 = _hitPointScale;
            matrix.m11 = _hitPointScale;
            matrix.m22 = _hitPointScale;

            matrix.m03 = position.x;
            matrix.m13 = position.y;
            matrix.m23 = position.z;

            StartCoroutine(DrawHitPointFadeOut(matrix,animatedScale));
        }        
        private IEnumerator DrawHitPointFadeOut(Matrix4x4 matrix, bool animatedScale)
        {
            float alpha = animatedScale ? _hitPointAnimatedFadeOutTime : _hitPointFadeOutTime;
            float animatedScaleValue = 0;
            while (alpha >= 0)
            {
                alpha -= Time.deltaTime;
                if (animatedScale)
                {
                    animatedScaleValue = Mathf.Lerp(0, _hitPointAnimatedScale,Mathf.InverseLerp(_hitPointFadeOutTime,0,alpha));
                    matrix.m00 = animatedScaleValue;
                    matrix.m11 = animatedScaleValue;
                    matrix.m22 = animatedScaleValue;
                }
                _materialPropertyBlock.SetFloat(_alphaNameID,alpha);
                Graphics.DrawMesh(_hitPointMesh,matrix,_hitPointMat,_hitLayer,null,0,_materialPropertyBlock,UnityEngine.Rendering.ShadowCastingMode.Off,false);            
                yield return null;
            }          
            yield return null;
        }
        #endregion
    }
}
