using Unity.VisualScripting;
using UnityEngine;

public class AtkOb : MonoBehaviour
{
    Rigidbody2D rigid;
    WeaponSetting weapon;
    SpriteRenderer spriteRenderer;
    bool isFollow = true;
    int dir = 1;
    float AngerDamageCoaf;  //버서커 모드 공격력
    float AngerHealthCoaf;  //버서커 모드 체력
    float MysteryCoaf;  //크리티컬 계수
    float WC; //약화 확률
    float WCD; //약화 데미지
    float VDHC; // VacationDrainHealthCoaf
    float Bleeding;
    HpLevelManager playerHP;
    void Awake(){
        chkStatLevel();
        GameObject hpbar = GameObject.FindGameObjectWithTag("HP"); 
        playerHP = hpbar.GetComponent<HpLevelManager>();
        AngerHealth();
    }
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>(); 
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetDir();
        Destroy(gameObject, weapon.disapearTime);
        Invoke("endFollow", weapon.folloewTime);
        transform.localScale *= (1 + (GameManager.info.allPlayerState.level-1)*0.1f);
    }
    void Update()
    {
        Attack();
    }
    void Attack(){
        Vector3 parentPos = transform.parent.position;
        if(isFollow == true){
            rigid.position = parentPos + weapon.pos *dir;
        }
        else{
            rigid.velocity = weapon.fowardSpeed;
            float angle = Mathf.Atan2(weapon.fowardSpeed.y, weapon.fowardSpeed.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0,angle);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        int LevelCoaf = GameManager.info.allPlayerState.level/5;
        int finalDamage =0;
        float CriticalPercent = GameManager.info.allPlayerState.critical;
        float CriticalCoaf = GameManager.info.allPlayerState.criticalDamage + MysteryCoaf;
        if(Random.value <= CriticalPercent){
            finalDamage = (int)(weapon.damage * (1+ LevelCoaf * 0.5f)* AngerDamageCoaf * CriticalCoaf);
        }else{
            finalDamage = (int)(weapon.damage * (1+ LevelCoaf * 0.5f)* AngerDamageCoaf);
            Bleeding = 0;
        }

        ChkWeakMonster();
        if(other.gameObject.layer == 7){
            Goblin monster = other.gameObject.GetComponent<Goblin>();
            Collider2D monsterCollider = monster.GetComponent<Collider2D>();
            //충돌된 오브젝트의 x,y값 중간에 생성
            Vector3 effectPosition = monsterCollider.bounds.center;
            
            if(monster != null){
                monster.Damaged(finalDamage,WCD, Bleeding);
                DrainHealth(finalDamage);
                Instantiate(weapon.AtkEffect,effectPosition, Quaternion.identity);
            }
        }

        if(other.gameObject.layer==11 || other.gameObject.layer==14){
            BossController BossSc = other.gameObject.GetComponent<BossController>();
            Collider2D monsterCollider = BossSc.GetComponent<Collider2D>();
            Vector3 effectPosition = monsterCollider.bounds.center;

            if(BossSc != null){
                BossSc.Damaged(finalDamage,WCD, Bleeding);
                DrainHealth(finalDamage);
                Instantiate(weapon.AtkEffect,effectPosition, Quaternion.identity);
            }
        }
    }
    void SetDir(){
        var setDir = Physics2D.OverlapArea(transform.position - new Vector3(-5,5,0), transform.position - new Vector3(0,-5,0), LayerMask.GetMask("Player"));
        if(setDir != null){
            spriteRenderer.flipX = true;
            dir = -1;
        }
    }
    void endFollow(){
        isFollow = false;
    }
    public void setState(WeaponSetting state, float WeakCoaf, float WeakCoafDmg, float DrainHealthCoaf, float BleedingCoaf){
        weapon = state;
        WC = WeakCoaf;
        WCD = WeakCoafDmg;
        VDHC = DrainHealthCoaf;
        Bleeding = BleedingCoaf;
    }

    void chkStatLevel(){
        AngerDamageCoaf = 1+((GameManager.info.abilityState.Anger/5 )*GameManager.info.abilityState.AEffectD);
        if(GameManager.info.abilityState.Anger >= 5){
            AngerHealthCoaf = GameManager.info.abilityState.AEffectH;   
        }else{
            AngerHealthCoaf = 0;
        }
        MysteryCoaf = (GameManager.info.abilityState.Mystery/5) * GameManager.info.abilityState.MEffect;  
    }

    void AngerHealth(){
        int selfHarm = (int)(GameManager.info.allPlayerState.maxHP * AngerHealthCoaf);
        if(GameManager.info.abilityState.Anger >= 5 && selfHarm <1)
            selfHarm =1;
        
        if(GameManager.info.allPlayerState.currentHp <= selfHarm){
            GameManager.info.allPlayerState.currentHp = 1;
            playerHP.UpdatePlayerHP();
        }
        else{
            GameManager.info.allPlayerState.currentHp -= selfHarm;
            playerHP.UpdatePlayerHP();
        }
    }

    void DrainHealth(int damage){
        float drainCoaf = (GameManager.info.abilityState.GEffect * (GameManager.info.abilityState.Greed /5));
        drainCoaf += VDHC;
        int restoreHP = (int)(damage * drainCoaf);
        Debug.Log(restoreHP);
        if((GameManager.info.allPlayerState.currentHp + restoreHP) >= GameManager.info.allPlayerState.maxHP){
            GameManager.info.allPlayerState.currentHp = GameManager.info.allPlayerState.maxHP;
            playerHP.UpdatePlayerHP();
        }
        else{
            GameManager.info.allPlayerState.currentExp += restoreHP;
            playerHP.UpdatePlayerHP();
        }
    }

    void ChkWeakMonster(){
        // 확률 계산하여 몬스터 약화 여부 결정
        if(Random.value > WC){
            WCD = 0;
        }
    }
}
