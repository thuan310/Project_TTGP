using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.TextCore.Text;

public class CatManager : MonoBehaviour
{
    private AudioSource audioSource;
    // Cài đặt các thông số trôi nổi
    [Header("Floating Settings")]
    public float floatHeight = 1.0f;    // Độ cao trung bình
    public float floatSpeed = 1.0f;     // Tốc độ trôi nổi
    public float floatAmount = 0.5f;    // Độ lên xuống

    [Header("Rotation Settings")]
    public bool enableRotation = true;
    public float rotationSpeed = 30f;   // Tốc độ xoay

    private Vector3 startPosition;
    private Animator animator;

    [Header("CatVFX")]
    public GameObject catSpawnVFX;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        startPosition = transform.position;
        animator = GetComponent<Animator>();

        // Tắt root motion nếu có Animator
        if (animator != null)
        {
            animator.applyRootMotion = false;
        }
    }

    private void OnEnable()
    {
        Instantiate(catSpawnVFX, this.transform);
        PLayCatSpawnSFX();
    }
    public void Update()
    {
        // Tính toán vị trí mới với hiệu ứng trôi nổi
        float newY = startPosition.y + floatHeight +
                    Mathf.Sin(Time.time * floatSpeed) * floatAmount;

        // Giữ nguyên vị trí X,Z nếu muốn, hoặc thêm hiệu ứng di chuyển ngang
        Vector3 newPosition = new Vector3(
            transform.position.x,
            newY,
            transform.position.z
        );

        transform.position = newPosition;

        // Thêm xoay nhẹ nếu bật
        if (enableRotation)
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }

    // Nếu có Animator, xử lý root motion
    //public void OnAnimatorMove()
    //{
    //    if (animator != null)
    //    {
    //        // Áp dụng root motion nếu cần, nhưng vẫn giữ hiệu ứng trôi nổi
    //        transform.position += animator.deltaPosition;
    //        transform.rotation *= animator.deltaRotation;
    //    }
    //}

    public void PLayCatSpawnSFX( float volume = 0.3f, bool randomizePitch = true, float pitchRandom = 0.1f)
    {
        AudioClip catSpawnSFX = WorldSoundFXManager.instance.catSpawnSFX;
        AudioClip sparkle = WorldSoundFXManager.instance.sparkle;

        audioSource.PlayOneShot(catSpawnSFX, volume);
        audioSource.PlayOneShot(sparkle, volume);
        // Reset Pitch
        audioSource.pitch = 1;

        if (randomizePitch)
        {
            audioSource.pitch += Random.Range(-pitchRandom, pitchRandom);
        }
    }
}