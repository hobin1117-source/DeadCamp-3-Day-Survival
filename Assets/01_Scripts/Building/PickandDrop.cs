using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickandDrop : MonoBehaviour
{
    public Transform holdPoint;       // 물건을 붙일 지점
    public float pickDistance = 3f;   // 집을 수 있는 거리
    public LayerMask pickLayer;       // 집을 수 있는 오브젝트 레이어

    private Rigidbody heldObject;     // 현재 들고 있는 오브젝트

    void Update()
    {
        // 집기 - 왼쪽 클릭(또는 E키 등)
        if (Input.GetMouseButtonDown(0))
        {
            TryPickObject();
        }

        // 놓기 - R 키
        if (Input.GetKeyDown(KeyCode.R))
        {
            DropObject();
        }

        // 들고 있을 때 위치 고정
        if (heldObject != null)
        {
            MoveHeldObject();
        }
    }

    void TryPickObject()
    {
        if (heldObject != null) return; // 이미 들고 있음

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickDistance, pickLayer))
        {
            Rigidbody targetRB = hit.collider.GetComponent<Rigidbody>();
            if (targetRB != null)
            {
                heldObject = targetRB;

                // 물리 끄기
                heldObject.useGravity = false;
                heldObject.velocity = Vector3.zero;
                heldObject.angularVelocity = Vector3.zero;
                heldObject.isKinematic = true;
            }
        }
    }

    void MoveHeldObject()
    {
        // 오브젝트 위치를 홀드 포인트에 고정
        heldObject.transform.position = holdPoint.position;
        heldObject.transform.rotation = holdPoint.rotation;
    }

    void DropObject()
    {
        if (heldObject == null) return;

        // 플레이어 앞 지면을 확인해서 바로 놓기
        Ray ray = new Ray(holdPoint.position, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 5f))
        {
            // 지면 위치에 놓기
            heldObject.transform.position = hit.point;
        }
        else
        {
            // 지면이 없으면 holdPoint 위치 그대로
            heldObject.transform.position = holdPoint.position;
        }

        heldObject.transform.rotation = holdPoint.rotation;

        // 물리 활성화 (중력 켜기)
        heldObject.useGravity = true;
        heldObject.isKinematic = false;

        heldObject = null;
    }
}
