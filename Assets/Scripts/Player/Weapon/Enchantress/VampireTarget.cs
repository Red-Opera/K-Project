using System.Collections.Generic;
using UnityEngine;

public class VampireTarget : MyWeapon
{
    [SerializeField] private float detectionRadius = 50.0f;     // 몬스터 탐색 반경

    public override void InitSetting()
    {
        Weapon.AtkObject = null;
        Weapon.AtkEffect = null;
        Weapon.pos = FindClosestMonster();
        Weapon.coolTime = 0.75f;
        Weapon.damage = GameManager.info.allPlayerState.damage;
        Weapon.disapearTime = 5;    
        Weapon.folloewTime = 0.0f;
        Weapon.fowardSpeed = Vector3.zero;
        Weapon.animName = "NULL";
    }

    private Vector2 FindClosestMonster()
    {
        // 탐색 반경 내의 모든 객체를 가져옴
        Collider[] objectsInRange = Physics.OverlapSphere(transform.position, detectionRadius);
        Transform closestMonster = null;        // 가장 가까운 몬스터의 Transform

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

                Goblin monster = obj.GetComponent<Goblin>();

                if (monster != null)
                {
                    int hp = monster.Hp;

                    if (hp <= 0)
                        continue;

                    string objName = (obj.name.Length >= 5 ? obj.name[..5] : obj.name) + "(" + obj.GetInstanceID() + ")";

                    // 체력, 거리 순 큐로 입력
                    findCloseMonster.push(new IntFloatChar32(-hp, -distanceToMonster, objName));
                    monsterToName.Add(objName, obj.transform);
                }
            }
        }

        // 체력, 거리 순으로 작은 몬스터 정보를 가져옴
        IntFloatChar32 clostMonsterInfo = findCloseMonster.top();

        if (clostMonsterInfo.c == "")
            return Vector2.zero;

        // 해당 이름의 몬스터를 반환함
        closestMonster = monsterToName[clostMonsterInfo.c];

        // 가장 가까운 몬스터의 위치를 반환
        Vector2 closestMonsterPos = closestMonster.position;
        Vector2 playerPos = transform.position;

        return closestMonsterPos - playerPos;
    }
}
