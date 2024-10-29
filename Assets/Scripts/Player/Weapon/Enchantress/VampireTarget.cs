using System.Collections.Generic;
using UnityEngine;

public class VampireTarget : MyWeapon
{
    [SerializeField] private float detectionRadius = 50.0f;     // ���� Ž�� �ݰ�

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
        // Ž�� �ݰ� ���� ��� ��ü�� ������
        Collider[] objectsInRange = Physics.OverlapSphere(transform.position, detectionRadius);
        Transform closestMonster = null;        // ���� ����� ������ Transform

        // ü�� ���� ��, �Ÿ� ����� ������ �����ϴ� �켱 ���� ť
        PriorityQueue<IntFloatChar32> findCloseMonster = PriorityQueue<IntFloatChar32>.Create();
        Dictionary<string, Transform> monsterToName = new Dictionary<string, Transform>();

        // Ž�� �ݰ� ���� �� ��ü�� �˻�
        foreach (Collider obj in objectsInRange)
        {
            if (obj.CompareTag("Monster"))
            {
                // ��ü���� ���� ���͸� ����ϰ� �÷��̾� �� ����� ��ü���� ������ ���
                Vector3 directionToMonster = (obj.transform.position - transform.position).normalized;
                float angleToMonster = Vector3.Angle(transform.forward, directionToMonster);

                // ��ü���� �Ÿ��� ���
                float distanceToMonster = Vector3.Distance(transform.position, obj.transform.position);

                Goblin monster = obj.GetComponent<Goblin>();

                if (monster != null)
                {
                    int hp = monster.Hp;

                    if (hp <= 0)
                        continue;

                    string objName = (obj.name.Length >= 5 ? obj.name[..5] : obj.name) + "(" + obj.GetInstanceID() + ")";

                    // ü��, �Ÿ� �� ť�� �Է�
                    findCloseMonster.push(new IntFloatChar32(-hp, -distanceToMonster, objName));
                    monsterToName.Add(objName, obj.transform);
                }
            }
        }

        // ü��, �Ÿ� ������ ���� ���� ������ ������
        IntFloatChar32 clostMonsterInfo = findCloseMonster.top();

        if (clostMonsterInfo.c == "")
            return Vector2.zero;

        // �ش� �̸��� ���͸� ��ȯ��
        closestMonster = monsterToName[clostMonsterInfo.c];

        // ���� ����� ������ ��ġ�� ��ȯ
        Vector2 closestMonsterPos = closestMonster.position;
        Vector2 playerPos = transform.position;

        return closestMonsterPos - playerPos;
    }
}
