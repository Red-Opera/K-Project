using System.Collections.Generic;
using UnityEngine;

public class MonsterTarget : MonoBehaviour
{
    [SerializeField] private float detectionRadius;

    private static MonsterTarget instance;

    private static float detectionRadiusStatic;

    private void Start()
    {
        instance = this;
        detectionRadiusStatic = detectionRadius;
    }

    private static GameObject FindClosestMonster()
    {
        Transform transform = instance.transform;

        // 탐색 반경 내의 모든 객체를 가져옴
        Collider[] objectsInRange = Physics.OverlapSphere(transform.position, detectionRadiusStatic);
        Transform closestMonster = null;        // 가장 가까운 몬스터의 Transform

        GameObject targetMonster = null;

        // 체력 낮은 순, 거리 가까운 순으로 반한하는 우선 순위 큐
        PriorityQueue<IntFloatChar32> findCloseMonster = PriorityQueue<IntFloatChar32>.Create();
        Dictionary<string, Transform> monsterToName = new Dictionary<string, Transform>();

        // 탐색 반경 내의 각 객체를 검사
        foreach (Collider obj in objectsInRange)
        {
            if (obj.CompareTag("Monster"))
            {
                // 객체와의 방향 벡터를 계산하고 플레이어 앞 방향과 객체와의 각도를 계산
                Vector3 directionToMonster = (obj.transform.position - transform.position).normalized;
                float angleToMonster = Vector3.Angle(transform.forward, directionToMonster);

                // 객체와의 거리를 계산
                float distanceToMonster = Vector3.Distance(transform.position, obj.transform.position);

                Goblin goblin = obj.GetComponent<Goblin>();

                if (goblin != null)
                {
                    if (goblin.Hp <= 0)
                        continue;

                    string objName = (obj.name.Length >= 5 ? obj.name[..5] : obj.name) + "(" + obj.GetInstanceID() + ")";

                    // 체력, 거리 순 큐로 입력
                    findCloseMonster.push(new IntFloatChar32(-goblin.Hp, -distanceToMonster, objName));
                    monsterToName.Add(objName, obj.transform);
                }

                else
                {
                    BossController bossController = obj.GetComponent<BossController>();

                    if (bossController != null)
                    {
                        if (bossController.hpLevelManager.currentHp <= 0)
                            continue;

                        string objName = (obj.name.Length >= 5 ? obj.name[..5] : obj.name) + "(" + obj.GetInstanceID() + ")";

                        // 체력, 거리 순 큐로 입력
                        findCloseMonster.push(new IntFloatChar32(-bossController.hpLevelManager.currentHp, -distanceToMonster, objName));
                        monsterToName.Add(objName, obj.transform);
                    }
                }
            }
        }

        // 체력, 거리 순으로 작은 몬스터 정보를 가져옴
        IntFloatChar32 clostMonsterInfo = findCloseMonster.top();

        if (clostMonsterInfo.c == "")
            return null;

        // 해당 이름의 몬스터를 반환함
        closestMonster = monsterToName[clostMonsterInfo.c];
        targetMonster = closestMonster.gameObject;

        return targetMonster;
    }
}