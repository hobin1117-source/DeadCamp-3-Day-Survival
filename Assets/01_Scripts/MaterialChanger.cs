using UnityEngine;
using System.Collections.Generic; // List<Material>을 위해 필요

public class MaterialChanger : MonoBehaviour
{
    // Mesh Renderer 컴포넌트 참조
    private MeshRenderer meshRenderer;

    // 현재 사용 중인 머티리얼의 인덱스
    private int currentMaterialIndex = 0;

    private void Awake()
    {
        // 1. MeshRenderer 컴포넌트 참조 획득
        // 이 스크립트가 붙은 GameObject에서 MeshRenderer를 찾습니다.
        meshRenderer = GetComponent<MeshRenderer>();

        // 초기 머티리얼 설정을 확인하거나 설정할 수 있습니다.
        // Debug.Log($"초기 머티리얼: {meshRenderer.material.name}");
    }

    private void Update()
    {
        // Space 키를 누르면 다음 머티리얼로 변경
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeMaterial();
        }
    }

    private void ChangeMaterial()
    {
        if (meshRenderer == null || meshRenderer.sharedMaterials.Length == 0)
        {
            Debug.LogWarning("MeshRenderer가 없거나 할당된 머티리얼이 없습니다.");
            return;
        }

        // 1. 다음 머티리얼 인덱스 계산
        // 배열의 끝에 도달하면 0으로 돌아와 반복합니다.
        currentMaterialIndex = (currentMaterialIndex + 1) % meshRenderer.sharedMaterials.Length;

        // 2. Mesh Renderer의 첫 번째 Element에 머티리얼 변경
        // sharedMaterials는 배열 전체를 가져오므로, 변경하려면 새로운 배열을 생성해야 합니다.
        // 하지만 단일 머티리얼만 교체하려면 material 속성을 사용합니다.
        // (단, material 속성은 새 인스턴스를 생성하므로, 성능에 민감한 경우 주의)

        // 🌟 여러 개의 서브메시(Submesh)가 있는 경우 sharedMaterials 배열에 직접 접근하여 교체해야 합니다. 🌟
        // 현재 Tree는 Element 0, 1, 2가 동일한 메시에 적용되는 경우이므로,
        // 첫 번째 서브메시(Element 0)만 교체하는 예시를 보여줍니다.

        // Mesh Renderer의 material 속성은 Element 0에 해당하는 머티리얼을 제어합니다.
        meshRenderer.material = meshRenderer.sharedMaterials[currentMaterialIndex];

        Debug.Log($"머티리얼 변경됨: {meshRenderer.material.name}");
    }
}