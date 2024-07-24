using MySqlConnector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class SaveSystem : MonoBehaviour
{
    public static Dictionary<string, int> itemID;
    private string[] filePaths;

    private void Awake()
    {
        filePaths = Directory.GetFiles("Assets/Resources/Scriptable/Equid", "*.asset", SearchOption.AllDirectories);
        ItemID();
        DontDestroyOnLoad(gameObject);
    }

    private void ItemID()
    {
        itemID = new Dictionary<string, int>();

        int id = 1;
        foreach (string filePath in filePaths)
        {
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            itemID.Add(fileName, id);
            id++;
        }
    }

    private bool HasColumn(MySqlDataReader reader, string columnName)
    {
        for (int i = 0; i < reader.FieldCount; i++)
        {
            if (reader.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase))
                return true;
        }

        return false;
    }

    private void SaveDefault()
    {
        // INSERT 쿼리 생성
        string infoQuery = "UPDATE PlayerInfo SET Level=@Level, MaxHP=@MaxHP, CurrentHP=@CurrentHP, Damage=@Damage, Defense=@Defense, Critical=@Critical, MoveSpeed=@MoveSpeed, " +
                           "JumpPower=@JumpPower, MaxJump=@MaxJump, DashBarCount=@DashBarCount, Food=@Food, Money=@Money WHERE Name=@Name;";

        MySqlCommand cmd = new MySqlCommand(infoQuery, Login.conn);
        cmd.Parameters.AddWithValue("@Name", Login.currentLoginName);

        State playerState = Resources.Load<State>("Scriptable/Player");
        Debug.Assert(playerState != null, "Error (Null Reference) : 플레이어 정보가 존재하지 않습니다.");

        // Reflection을 사용하여 필드를 파라미터로 추가
        foreach (FieldInfo field in typeof(State).GetFields(BindingFlags.Public | BindingFlags.Instance))
            cmd.Parameters.AddWithValue("@" + (char.ToUpper(field.Name[0]) + field.Name[1..]), field.GetValue(playerState));

        try
        {
            Login.conn.Open();
            cmd.ExecuteNonQuery();
        }

        catch (Exception ex)
        {
            Debug.LogError($"Failed to execute query for PlayerInfo: " + ex.Message);
        }

        finally
        {
            Login.conn.Close();
        }
    }

    private void SaveItem()
    {
        // INSERT 쿼리 생성
        string infoQuery = "INSERT INTO PlayerItem (ID, Name, Item1, Item2, Item3, Item4, Item5, Item6, Item7, Item8, Item9, Item10, " +
                                                             "Item11, Item12, Item13, Item14, Item15, Item16, Item17, Item18, Item19, Item20, Item21, Item22, Item23) " +
                           "VALUES(@ID, @Name, @Item1, @Item2, @Item3, @Item4, @Item5, @Item6, @Item7, @Item8, @Item9, @Item10, " +
                                              "@Item11, @Item12, @Item13, @Item14, @Item15, @Item16, @Item17, @Item18, @Item19, @Item20, @Item21, @Item22, @Item23) " +
                           "ON DUPLICATE KEY UPDATE " +
                           "Name = VALUES(Name), " +
                           "Item1 = VALUES(Item1), " + "Item2 = VALUES(Item2), " + "Item3 = VALUES(Item3), " + "Item4 = VALUES(Item4), " + "Item5 = VALUES(Item5), " + 
                           "Item6 = VALUES(Item6), " + "Item7 = VALUES(Item7), " + "Item8 = VALUES(Item8), " + "Item9 = VALUES(Item9), " + "Item10 = VALUES(Item10), " +
                           "Item11 = VALUES(Item11), " + "Item12 = VALUES(Item12), " + "Item13 = VALUES(Item13), " + "Item14 = VALUES(Item14), " + "Item15 = VALUES(Item15), " +
                           "Item16 = VALUES(Item16), " + "Item17 = VALUES(Item17), " + "Item18 = VALUES(Item18), " + "Item19 = VALUES(Item19), " + "Item20 = VALUES(Item20), " + "Item21 = VALUES(Item21), " + "Item22 = VALUES(Item22), " + "Item23 = VALUES(Item23);";

        MySqlCommand cmd = new MySqlCommand(infoQuery, Login.conn);
        cmd.Parameters.AddWithValue("@ID", Login.currentLoginID);
        cmd.Parameters.AddWithValue("@Name", Login.currentLoginName);

        if (InventroyPosition.inventory == null)
            return;

        // 기본적으로 모든 아이템 번호와 수량을 NULL로 설정
        for (int i = 1; i <= 23; i++)
            cmd.Parameters.AddWithValue("@Item" + i, DBNull.Value);

        // Reflection을 사용하여 필드를 파라미터로 추가
        for (int i = 0; i < InventroyPosition.inventory.transform.childCount; i++)
        {
            Transform slot = InventroyPosition.inventory.transform.GetChild(i);

            if (slot.childCount <= 0 || slot.GetChild(0).childCount <= 0)
                continue;

            string itemName = slot.GetChild(0).GetChild(0).GetComponent<Image>().sprite.name;

            int setID = -1;
            setID = itemID[itemName];

            cmd.Parameters["@Item" + (i + 1)].Value = setID;
        }

        try
        {
            Login.conn.Open();
            cmd.ExecuteNonQuery();
        }

        catch (Exception ex)
        {
            Debug.LogError($"Failed to execute query for PlayerItem: " + ex.Message);
        }

        finally
        {
            Login.conn.Close();
        }
    }

    private void OnApplicationQuit()
    {
        if (Login.currentLoginName == "")
            return;

        SaveDefault();
        SaveItem();
    }

    private void OnDestroy()
    {
        OnApplicationQuit();
    }
}
