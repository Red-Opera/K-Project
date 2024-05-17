using System.Collections.Generic;
using UnityEngine;

public class GetItems : MonoBehaviour
{
    public string Name { get; set; }
    public string Rarity { get; set; }

    public GetItems(string name, string rarity)
    {
        Name = name;
        Rarity = rarity;
    }
}

public class ItemSelector : MonoBehaviour
{
    public string getItem = "";

    void Start()
    { 
        // 아이템 레어도 리스트 생성
        Dictionary<string, double> rarityProbabilities = new Dictionary<string, double>
        {
            {"rare", 0.4},
            {"epic", 0.3},
            {"unique", 0.2},
            {"legend", 0.1}
        };

        // 아이템 리스트 생성
        List<GetItems> items = new List<GetItems>
        {
            new GetItems("투척용단검","rare"),
            new GetItems("암철검","rare"),
            new GetItems("참마도", "rare"),
            new GetItems("강철대검", "rare"),
            new GetItems("금빛섬광","epic"),
            new GetItems("파산검","epic"),
            new GetItems("아론타이트","epic"),
            new GetItems("오크척살자","epic"),
            new GetItems("무라마사","unique"),
            new GetItems("알마이스","unique"),
            new GetItems("브륀힐드","unique"),
            new GetItems("용의포효","unique"),
            new GetItems("룬셰도우","unique"),
            new GetItems("호푸어드","unique"),  
            new GetItems("다인슬래이프", "legend"),
            new GetItems("프라가라흐","legend"),
            new GetItems("오크척살자","unique"),
            new GetItems("골든링","unique"),
            new GetItems("금속링","unique"), 
            new GetItems("드래곤링","unique"),
            new GetItems("번개링","unique"),
            new GetItems("천둥링","unique"),
            new GetItems("지옥링","unique")
        };

        string selectedRarity = ChooseRandomRarity(rarityProbabilities);

        // 선택된 레어도에 해당하는 아이템 리스트
        List<GetItems> filteredItems = items.FindAll(item => item.Rarity == selectedRarity);

        // 랜덤하게 아이템 선택
        GetItems selectedItem = filteredItems[Random.Range(0, filteredItems.Count)];
        getItem = selectedItem.Name;

        //Debug.Log($"선택된 아이템: {selectedItem.Name}, 레어도: {selectedItem.Rarity}");
    }

    static string ChooseRandomRarity(Dictionary<string, double> rarityProbabilities)
    {
        double randomNumber = UnityEngine.Random.Range(0.0f, 1.0f);
        double cumulativeProbability = 0;

        foreach (var rarity in rarityProbabilities)
        {
            cumulativeProbability += rarity.Value;
            if (randomNumber < cumulativeProbability)
            {
                return rarity.Key;
            }
        }

        return "";
    }
}
