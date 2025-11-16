using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewObjectValidChecker : MonoBehaviour
{
    [SerializeField] private LayerMask invalidLayers; // 겹치면 안 되는 레이어들
    public bool IsValid { get; private set; } = true;

    // Inspector에서 볼 필요 없으니까 SerializeField 제거
    private HashSet<Collider> _collidingObjects = new HashSet<Collider>();

    private void OnTriggerEnter(Collider other)
    {
        
        // other의 레이어가 invalidLayers에 포함되어 있으면
        if (((1 << other.gameObject.layer) & invalidLayers) != 0)
        {
            Debug.Log(other.gameObject.name + "와(과) 충돌함.");
            _collidingObjects.Add(other);
            IsValid = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & invalidLayers) != 0)
        {
            _collidingObjects.Remove(other);
            IsValid = _collidingObjects.Count <= 0; // 더 이상 겹치는 게 없으면 다시 true
        }
    }
}