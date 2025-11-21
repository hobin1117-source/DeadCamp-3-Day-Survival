using UnityEngine;

public class EquipTool : Equip
{
    public float attackRate;
    private bool attacking;
    public float attackDistance;

    [Header("Resource Gathering")]
    public bool doesGatherResources;

    [Header("Combat")]
    public bool doesDealDamage;
    public int damage;
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;    // 도끼 SFX 재생용
    [SerializeField] private AudioClip swingSFX;         // 도끼 휘두르기 소리
    [SerializeField] private AudioClip woodHitSFX;       // 나무 타격 소리

    private Animator animator;
    private Camera camera;
    private void Start()
    {
        camera = Camera.main;
        animator = GetComponent<Animator>();
    }

    public override void OnAttackInput()
    {
        if (!attacking)
        {
            attacking = true;
            animator.SetTrigger("Attack");
            Invoke("OnCanAttack", attackRate);
            if (CharacterManager.Instance.Player.condition)
            {
                attacking = true;
                animator.SetTrigger("Attack");
                PlaySwingSound();
                Invoke("OnCanAttack", attackRate);
            }
        }
    }
    private void PlaySwingSound()
    {
        if (audioSource != null && swingSFX != null)
            audioSource.PlayOneShot(swingSFX);
    }

    void OnCanAttack()
    {
        attacking = false;
    }
    public void OnHit()
    {
        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, attackDistance))
        {
            if (doesGatherResources && hit.collider.TryGetComponent(out Resource resource))
            {
                resource.Gathering(hit.point, hit.normal);
                PlayWoodHitSound();
            }
           
        }
    }
    private void PlayWoodHitSound()
    {
        if (audioSource != null && woodHitSFX != null)
            audioSource.PlayOneShot(woodHitSFX);
    }

}
