using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnLock_Zone : MonoBehaviour
{

    //�رݺ��
    public float cost;
    //�ر� ui
    public TMP_Text Unlock_text;
    //������Ʈ��
    public GameObject Moneyob;
    //���������� ��ġ
    public Transform monetdrop_trs;
    //�رݵǴ� ������Ʈ
    public GameObject unlockOb;
    //��� �Ⱥ��̰ԵǴ� ������Ʈ
    public GameObject lockOb;

    //�ر� �ִϸ��̼���?
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
    // �ر� ���
    public void UnLock()
    {
        if (unlockOb != null)
            unlockOb.SetActive(true);

        var unlockable = unlockOb.GetComponent<IUnlockable>();
        if (unlockable != null)
        {
            //�Ҹ�����
            SoundManager.Instance.PlayUnLockSound();

            unlockable.OnUnlocked();
        }

        if (lockOb != null)
            lockOb.SetActive(false);
    }

    //�䱸ġ�� �� ���ؼ� �ر�
    IEnumerator UnLockCoroutine()
    {
        isUnlocking = true;
        
        float value;
        //���������� �־��� ��
        float startmoney = Mathf.Min(GameManager.Instance.Money, cost);
        StartCoroutine(Effect());
        while (startmoney > 0 && cost > 0)
        {
            value = Time.deltaTime * 50f;

            // �� �� ���� ������ ����
            float minValue = Mathf.Min(value, startmoney, cost);

            cost -= minValue;
            startmoney -= minValue;

            GameManager.Instance.Money -= minValue;
            Unlock_text.text = ((int)cost).ToString();
            yield return null;
        }

        //�ڽ�Ʈ�� �ٽ����� �ر�
        if (cost <= 0)
        {
            UnLock();
        }

        cost = Mathf.Round(cost * 100f) / 100f;
        GameManager.Instance.Money = Mathf.Round(GameManager.Instance.Money * 100f) / 100f;

        Unlock_text.text = ((int)cost).ToString();
        isUnlocking = false;
    }


    //���ٹ� �Դ� �ϸ� ����Ʈ
    IEnumerator Effect()
    {
        Player player = GameManager.Instance.Player;

        while (isUnlocking)
        {
            //�Ҹ�����
            SoundManager.Instance.PlayUseMoneySound();

            GameObject moneys = Instantiate(Moneyob, player.transform.position, Quaternion.Euler(0, 90, 0), transform);
            moneys.transform.localScale = new Vector3(1.5f, 3f, 1.5f);
            float power = Random.Range(2.5f, 3.5f);
            StartCoroutine(MoneyEffect(moneys.transform, 0.1f, power));


            yield return new WaitForSeconds(0.03f);
        }

    }

    //���ٹ� �ϳ� ��� �ڷ�ƾ
    public IEnumerator MoneyEffect(Transform item, float duration, float arcHeight)
    {

        Vector3 start = item.position;

        float time = 0f;

        while (time < duration)
        {
            float t = time / duration;

            // ���� ��ġ ����
            Vector3 flatPos = Vector3.Lerp(start, monetdrop_trs.position, t);

            // ������ ���� ��� (�߰��� ����, ����/���� ����)
            float height = Mathf.Sin(t * Mathf.PI) * arcHeight;

            // ���� ��ġ = ���� ��ġ + ���� �ø� ��ŭ
            item.position = flatPos + Vector3.up * height;


            time += Time.deltaTime;
            yield return null;
        }

        Destroy(item.gameObject);
    }

}
