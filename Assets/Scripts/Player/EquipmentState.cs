using System;

// �÷��̾ ������ �� �ִ� ��� Ÿ��
[Flags]
public enum EquipmentState : uint
{
    EQUIPMENT_ALL              = 0xFFFFFFFF,      // ��� ���
    EQUIPMENT_WEAPON_ONEHANDED_SHORT_RANGE = 0xF0000000,     // �Ѽ� �ܰŸ� ����
    EQUIPMENT_WEAPON_ONEHANDED_LARGE_RANGE = 0x0F000000,    // �Ѽ� ��Ÿ� ����
    EQUIPMENT_WEAPON_TWOHANDED_SHORT_RANGE = 0x00F00000,    // ��� ����
    EQUIPMENT_WEAPON_TWOHANDED_LARGE_RANGE = 0x000F0000,    // ��� ����
    EQUIPMENT_ACCESSORY                    = 0x0000F000     // �Ǽ�����
}