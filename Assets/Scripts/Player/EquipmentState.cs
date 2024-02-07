using System;

// 플레이어가 장착할 수 있는 장비 타입
[Flags]
public enum EquipmentState : uint
{
    EQUIPMENT_ALL              = 0xFFFFFFFF,      // 모든 장비
    EQUIPMENT_WEAPON_ONEHANDED_SHORT_RANGE = 0xF0000000,     // 한손 단거리 무기
    EQUIPMENT_WEAPON_ONEHANDED_LARGE_RANGE = 0x0F000000,    // 한손 장거리 무기
    EQUIPMENT_WEAPON_TWOHANDED_SHORT_RANGE = 0x00F00000,    // 양손 무기
    EQUIPMENT_WEAPON_TWOHANDED_LARGE_RANGE = 0x000F0000,    // 양손 무기
    EQUIPMENT_ACCESSORY                    = 0x0000F000     // 악세서리
}