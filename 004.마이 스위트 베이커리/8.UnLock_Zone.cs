using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnLock_Zone : MonoBehaviour
{

    //해금비용
    public float cost;
    //해금 ui
    public TMP_Text Unlock_text;
    //돈이펙트용
    public GameObject Moneyob;
    //돈떨어지는 위치
    public Transform monetdrop_trs;
    //해금되는 오브젝트
    public GameObject unlockOb;
    //대신 안보이게되는 오브젝트
    public GameObject lockOb;

    //해금 애니메이션중?
    public bool isUnlocking = false;
    private void Start()
    {
        Unlock_text.text = ((int)cost).ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameManager.Instance.Money > 0 && cost>0)
        {
            StartCoroutine(UnLockCoroutine());
        }
    }
    // 해금 펑션
    public void UnLock()
    {
        if (unlockOb != null)
            unlockOb.SetActive(true);

        var unlockable = unlockOb.GetComponent<IUnlockable>();
        if (unlockable != null)
        {
            //소리실행
            SoundManager.Instance.PlayUnLockSound();

            unlockable.OnUnlocked();
        }

        if (lockOb != null)
            lockOb.SetActive(false);
    }

    //요구치랑 돈 비교해서 해금
    IEnumerator UnLockCoroutine()
    {
        isUnlocking = true;
        
        float value;
        //입장했을때 있었던 돈
        float startmoney = Mathf.Min(GameManager.Instance.Money, cost);
        StartCoroutine(Effect());
        while (startmoney > 0 && cost > 0)
        {
            value = Time.deltaTime * 50f;

            // 둘 중 작은 값으로 보정
            float minValue = Mathf.Min(value, startmoney, cost);

            cost -= minValue;
            startmoney -= minValue;

            GameManager.Instance.Money -= minValue;
            Unlock_text.text = ((int)cost).ToString();
            yield return null;
        }

        //코스트를 다썼으면 해금
        if (cost <= 0)
        {
            UnLock();
        }

        cost = Mathf.Round(cost * 100f) / 100f;
        GameManager.Instance.Money = Mathf.Round(GameManager.Instance.Money * 100f) / 100f;

        Unlock_text.text = ((int)cost).ToString();
        isUnlocking = false;
    }


    //돈다발 먹는 하마 이펙트
    IEnumerator Effect()
    {
        Player player = GameManager.Instance.Player;

        while (isUnlocking)
        {
            //소리실행
            SoundManager.Instance.PlayUseMoneySound();

            GameObject moneys = Instantiate(Moneyob, player.transform.position, Quaternion.Euler(0, 90, 0), transform);
            moneys.transform.localScale = new Vector3(1.5f, 3f, 1.5f);
            float power = Random.Range(2.5f, 3.5f);
            StartCoroutine(MoneyEffect(moneys.transform, 0.1f, power));


            yield return new WaitForSeconds(0.03f);
        }

    }

    //돈다발 하나 쏘는 코루틴
    public IEnumerator MoneyEffect(Transform item, float duration, float arcHeight)
    {

        Vector3 start = item.position;

        float time = 0f;

        while (time < duration)
        {
            float t = time / duration;

            // 수평 위치 보간
            Vector3 flatPos = Vector3.Lerp(start, monetdrop_trs.position, t);

            // 포물선 높이 계산 (중간에 높고, 시작/끝은 낮음)
            float height = Mathf.Sin(t * Mathf.PI) * arcHeight;

            // 최종 위치 = 수평 위치 + 위로 올린 만큼
            item.position = flatPos + Vector3.up * height;


            time += Time.deltaTime;
            yield return null;
        }

        Destroy(item.gameObject);
    }

}
