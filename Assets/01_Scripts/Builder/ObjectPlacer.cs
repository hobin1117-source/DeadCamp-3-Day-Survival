using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [Header("Placement Parameters")]
    [SerializeField] private GameObject placeableObjectPrefab;   // 실제 배치될 오브젝트 프리팹
    [SerializeField] private GameObject previewObjectPrefab;     // 미리보기(고스트) 프리팹
    [SerializeField] private Camera playerCamera;                // 플레이어 카메라
    [SerializeField] private LayerMask placementSurfaceLayerMask; // 배치 가능한 바닥 레이어

    [Header("Preview Material")]
    [SerializeField] private Material previewMaterial;           // 미리보기용 머티리얼
    [SerializeField] private Color validColor;     // 배치 가능 색
    [SerializeField] private Color invalidColor;     // 배치 불가 색

    [Header("Raycast Parameters")]
    [SerializeField] private float objectDistanceFromPlayer = 3f;    // 카메라 앞 거리
    [SerializeField] private float raycastStartVerticalOffset = 5f;  // 위에서 아래로 쏠 높이
    [SerializeField] private float raycastDistance = 10f;            // 아래로 쏠 거리

    private GameObject _previewObject = null;
    private Vector3 _currentPlacementPosition = Vector3.zero;
    private bool _inPlacementMode = false;
    private bool _validPreviewState = false;

    private void Update()
    {
        UpdateInput();

        if (_inPlacementMode)
        {
            UpdateCurrentPlacementPosition();

            if (CanPlaceObject())
                SetValidPreviewState();
            else
                SetInvalidPreviewState();
        }
    }

    private void UpdateCurrentPlacementPosition()
    {
        if (playerCamera == null || _previewObject == null)
            return;

        // 카메라의 수평 방향 벡터
        Vector3 cameraForward = new Vector3(playerCamera.transform.forward.x, 0f, playerCamera.transform.forward.z);
        cameraForward.Normalize();

        // 카메라 앞쪽 위치에서 위에서 아래로 레이 쏘기
        Vector3 startPos = playerCamera.transform.position + (cameraForward * objectDistanceFromPlayer);
        startPos.y += raycastStartVerticalOffset;

        RaycastHit hitInfo;
        if (Physics.Raycast(startPos, Vector3.down, out hitInfo, raycastDistance, placementSurfaceLayerMask))
        {
            _currentPlacementPosition = hitInfo.point;
        }

        Quaternion rotation = Quaternion.Euler(0f, playerCamera.transform.eulerAngles.y, 0f);
        _previewObject.transform.position = _currentPlacementPosition;
        _previewObject.transform.rotation = rotation;
    }

    private void UpdateInput()
    {
        // 1키: 배치 모드 들어가기
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EnterPlacementMode();
        }
        // 2키: 배치 모드 나가기
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ExitPlacementMode();
        }
        // 마우스 왼쪽 클릭: 실제로 오브젝트 배치
        else if (Input.GetMouseButtonDown(0))
        {
            PlaceObject();
        }
    }

    private void SetValidPreviewState()
    {
        if (previewMaterial != null)
        {
            previewMaterial.color = validColor;
        }
        _validPreviewState = true;
    }

    private void SetInvalidPreviewState()
    {
        if (previewMaterial != null)
        {
            previewMaterial.color = invalidColor;
        }
        _validPreviewState = false;
    }

    private bool CanPlaceObject()
    {
        if (_previewObject == null)
            return false;

        return _previewObject.GetComponentInChildren<PreviewObjectValidChecker>() .IsValid;

    }

    private void PlaceObject()
    {
        // 배치 모드가 아니거나, 현재 위치가 유효하지 않으면 배치 X
        if (!_inPlacementMode || !_validPreviewState)
            return;

        Quaternion rotation = Quaternion.Euler(0f, playerCamera.transform.eulerAngles.y, 0f);
        Instantiate(placeableObjectPrefab, _currentPlacementPosition, rotation, transform);

        ExitPlacementMode();
    }

    private void EnterPlacementMode()
    {
        if (_inPlacementMode)
            return;

        if (playerCamera == null)
        {
            Debug.LogWarning("ObjectPlacer: playerCamera가 비어있습니다. 인스펙터에서 지정해 주세요.");
            return;
        }

        Quaternion rotation = Quaternion.Euler(0f, playerCamera.transform.eulerAngles.y, 0f);
        _previewObject = Instantiate(previewObjectPrefab, _currentPlacementPosition, rotation, transform);
        _inPlacementMode = true;

        // 필요하다면 여기서 커서 잠금 해제 같은 것도 가능
        // Cursor.lockState = CursorLockMode.None;
        // Cursor.visible = true;
    }

    private void ExitPlacementMode()
    {
        if (_previewObject != null)
        {
            Destroy(_previewObject);
        }

        _previewObject = null;
        _inPlacementMode = false;

        // 필요하다면 커서 다시 잠그기
        // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;
    }
}