using UnityEngine;
using Cainos.PixelArtPlatformer_VillageProps;

public class Box : MonoBehaviour
{
    private Chest currentChest; // 현재 상자

    void Update()
    {
        ItemSelector getI= GetComponent<ItemSelector>();

    // F 키를 누르면 상자를 열기
        if (Input.GetKeyDown(KeyCode.F) && currentChest != null)
            {      
                        
                OpenCloseChest();
                
                ResultUI.GetItem(getI.getItem);
            }
    }

    void OpenCloseChest()
    {
        // 현재 상자가 열려있는지 확인하고 열린 상태로 변경
        if (!currentChest.IsOpened)
            {

                currentChest.Open();
            }
        else
            {
                return;
            }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 상자에 접촉하면 상자를 저장
        if (other.CompareTag("Chest"))
        {
            Chest chest = other.GetComponent<Chest>();
            if (chest != null)
            {
                currentChest = chest;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // 상자에서 벗어나면 현재 상자를 null로 설정
        if (other.CompareTag("Chest"))
        {
                currentChest = null;
        }
    }
    
}

