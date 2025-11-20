using UnityEngine;

public class WaterResource : MonoBehaviour, IInteractable
{
    [TextArea]
    [SerializeField]
    private string prompt = "호수, 물을 마실 수 있다.\n[E] 키를 눌러 마시기";

    [SerializeField]
    private float drinkAmount = 20f;   // 한 번에 회복할 갈증 수치
    private AudioSource drinkAudioSource;
    public string GetInteractPrompt()
    {
        // Interaction.cs에서 이 문자열을 PromptText에 뿌려줌
        return prompt;
    }
    public void OnInteract()
    {
        // E키를 눌렀을 때 실행되는 부분
        Debug.Log("물을 마셨다!");
        // SFX 재생
        if (drinkAudioSource != null)
            drinkAudioSource.Play();


        var playerCondition = CharacterManager.Instance.Player.condition;
        if (playerCondition != null)
            playerCondition.Drink(drinkAmount);
    }
    
}