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

        // Ž�� �ݰ� ���� ��� ��ü�� ������
        Collider[] objectsInRange = Physics.OverlapSphere(transform.position, detectionRadiusStatic);
        Transform closestMonster = null;        // ���� ����� ������ Transform

        GameObject targetMonster = null;

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

                Goblin goblin = obj.GetComponent<Goblin>();

                if (goblin != null)
                {
                    if (goblin.Hp <= 0)
                        continue;

                    string objName = (obj.name.Length >= 5 ? obj.name[..5] : obj.name) + "(" + obj.GetInstanceID() + ")";

                    // ü��, �Ÿ� �� ť�� �Է�
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

                        // ü��, �Ÿ� �� ť�� �Է�
                        findCloseMonster.push(new IntFloatChar32(-bossController.hpLevelManager.currentHp, -distanceToMonster, objName));
                        monsterToName.Add(objName, obj.transform);
                    }
                }
            }
        }

        // ü��, �Ÿ� ������ ���� ���� ������ ������
        IntFloatChar32 clostMonsterInfo = findCloseMonster.top();

        if (clostMonsterInfo.c == "")
            return null;

        // �ش� �̸��� ���͸� ��ȯ��
        closestMonster = monsterToName[clostMonsterInfo.c];
        targetMonster = closestMonster.gameObject;

        return targetMonster;
    }
}