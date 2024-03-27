using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    [SerializeField] private MeshRenderer background; 
    [SerializeField] private MeshRenderer foreground; 
    [SerializeField] private MeshRenderer titleObj;

    [SerializeField] private float backgroundSpeed;
    [SerializeField] private float foregroundSpeed;
    [SerializeField] private float titleObjSpeed;

    private float time = 0.0f;

    void Start()
    {
        Debug.Assert(background != null, "뒷 배경이 없습니다.");
        Debug.Assert(foreground != null, "앞 배경이 없습니다.");
        Debug.Assert(titleObj != null, "배경 오브젝트이 없습니다.");

    }

    void Update()
    {
        time += Time.deltaTime;

        background.material.mainTextureOffset = new Vector2(time * backgroundSpeed, 0);
        foreground.material.mainTextureOffset = new Vector2(time * foregroundSpeed, 0);
        titleObj.material.mainTextureOffset = new Vector2(time * titleObjSpeed, 0);
    }
}
