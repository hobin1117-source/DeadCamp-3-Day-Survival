using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers instance;
    public static Managers Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<Managers>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Init();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 인스펙터 할당
    [SerializeField] private ResourceSpawner resource;
    
    // 외부 접근
    public static ResourceSpawner Resource { get; private set; }

    // 참조 연결
    private void Init()
    {
        Resource = resource;
    }
}
