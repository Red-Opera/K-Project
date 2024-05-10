using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "Scriptable/Player", order = int.MaxValue)]
public class State : ScriptableObject
{
    public string nickName;             // �÷��̾� �г���
    public int level = 0;               // ����
    public int maxHP = 0;               // �ִ� ü��
    public int currentHp = 0;           // ���� ü��
    public int currentExp = 0;          // ���� ����ġ

    public int damage = 0;              // ������
    public int defense = 0;             // ����
    public float critical = 0;          // ġ��Ÿ Ȯ��
    public float moveSpeed = 0;         // �̵� �ӵ�
    public float jumpPower = 0;         // ������
    public int jumpCount = 0;           // ���� Ƚ��
    public int maxJump = 0;             // �ִ� ���� ȸ��
    public float dashDamage = 0;        // �뽬 ������
    public int strong = 0;              // ������
    public float criticalDamage = 0;    // ũ��Ƽ�� ������
    public float attackSpeed = 0;       // ���� �ӵ�
    public int fixedDamage = 0;         // �߰� ���� ������
    public float defensePersent = 0;    // ��� Ȯ��
    public float avoidPersent = 0;      // ȸ�� Ȯ��
    public float reloadTime = 0;        // ������ �ð�
    public static float satiety = 0;

    public int dashBarCount = 0;        // �뽬 �� ����
    public int food = 0;                // ���
    public int money = 0;               // �÷��̾� ��

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