using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "Scriptable/Player", order = int.MaxValue)]
public class State : ScriptableObject
{
    public string nickName;         // �÷��̾� �г���
    public int level;               // ����
    public int maxHP;               // �ִ� ü��
    public int currentHp;           // ���� ü��

    public int damage;              // ������
    public int defense;             // ����
    public float critical;          // ũ��Ƽ�� Ȯ��
    public float moveSpeed;         // �̵� �ӵ�
    public float jumpPower;         // ������
    public int jumpCount;           // ���� Ƚ��
    public int maxJump;             // �ִ� ���� ȸ��
    public float dashDamage;        // �뽬 ������
    public int strong;              // ������
    public float criticalDamage;    // ũ��Ƽ�� ������
    public float attackSpeed;       // ���� �ӵ�
    public int fixedDamage;         // �߰� ���� ������
    public float defensePersent;    // ��� Ȯ��
    public float avoidPersent;      // ȸ�� Ȯ��
    public float reloadTime;        // ������ �ð�
    public static float satiety;

    public int dashBarCount;        // �뽬 �� ����
    public int food;                // ��� ����
    public int money;               // �÷��̾� ��

    public static Dictionary<string, FieldInfo> datas = new Dictionary<string, FieldInfo>();

    public void Awake()
    {
        // ��� ��� ������ ������
        FieldInfo[] allField = GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

        foreach (FieldInfo field in allField)
        {
            // �ʵ� �̸��� ������ �빮�ڷ� �ٲ�
            string fieldName = field.Name;
            fieldName = char.ToUpper(fieldName[0]) + fieldName.Substring(1);

            // �̹� �����Ͱ� �����ϴ��� Ȯ��
            if (datas.ContainsKey(fieldName))
                continue;

            datas.Add(fieldName, field);
        }
    }
}