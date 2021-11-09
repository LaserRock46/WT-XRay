using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Uncategorized
{
   [AddComponentMenu(nameof(Project) + "/" + nameof(Uncategorized) + "/HitPointsPool")]
    public class HitPointsPool : MonoBehaviour
    {
        #region Temp
        //[Header("Temporary Things", order = 0)]
        #endregion

        #region Fields
        [Header("Fields", order = 1)]
        [SerializeField] private int _poolCount = 100;
        [SerializeField] private GameObject _hitPointPrefab;
        private MeshRenderer[] _hitPointRenderers = null;
        private Transform[] _hitPointTransforms = null;
        private int _takenFromPool = -1;      
        private MaterialPropertyBlock _materialPropertyBlock;
        private int _alphaNameID;
        #endregion

        #region Functions

        #endregion



        #region Methods
        void Start()
        {
            _materialPropertyBlock = new MaterialPropertyBlock();
            _alphaNameID = Shader.PropertyToID("_Alpha");
            CreatePool();
        }     
        void CreatePool()
        {
            _hitPointRenderers = new MeshRenderer[_poolCount];
            _hitPointTransforms = new Transform[_poolCount];

            for (int i = 0; i < _poolCount; i++)
            {
                GameObject newHitPoint = Instantiate(_hitPointPrefab,transform.position,Quaternion.identity,transform);
                _hitPointTransforms[i] = newHitPoint.transform;
                _hitPointRenderers[i] = newHitPoint.GetComponent<MeshRenderer>();
            }
        }
        
        public void GetHitPointHere(Vector3 position)
        {
            if (_takenFromPool+1 >= _poolCount)
            {
                _takenFromPool = -1;
            }

            _takenFromPool++;
           
            _hitPointTransforms[_takenFromPool].gameObject.SetActive(true);
            _hitPointTransforms[_takenFromPool].position = position;

            StartCoroutine(FadeOut(_takenFromPool));
        }
        private IEnumerator FadeOut(int indexInPool)
        {                     
            float alphaTime = 2f;
            while (alphaTime >= 0)
            {
                alphaTime -= Time.deltaTime;

                _hitPointRenderers[indexInPool].GetPropertyBlock(_materialPropertyBlock);
                _materialPropertyBlock.SetFloat(_alphaNameID,Mathf.Clamp(alphaTime,0,1));
                _hitPointRenderers[indexInPool].SetPropertyBlock(_materialPropertyBlock);
                yield return null;
            }

            _hitPointTransforms[indexInPool].gameObject.SetActive(false);
            yield return null;
        }
        #endregion
    }
}
