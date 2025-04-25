using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOb_zone : MonoBehaviour
{
    // ����� �ǸŰ���
    public Sell_Ob s_ob;               
    // �۾� �ҿ�ð�
    public float takeInterval = 0.2f;
    public bool isdisplaying = false;

    //�۾� �ڷ�ƾ��
    private Coroutine collectRoutine;
    private Coroutine disableSetRoutine;
    private Coroutine sizeRoutine;
    //���� ũ������ ���
    public float resizeValue;
    private SpriteRenderer sprite_renderer;

    private void Awake()
    {
        sprite_renderer = GetComponent<SpriteRenderer>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isdisplaying)
        {
            Debug.Log($"{other.name}");
            collectRoutine = StartCoroutine(CollectItemCoroutine(other.GetComponent<Player>()));
            s_ob.IsSetting = true;

            if (sizeRoutine != null) StopCoroutine(sizeRoutine);
            sizeRoutine = StartCoroutine(ResizeSprite(new Vector2(resizeValue, resizeValue), 0.2f));

            // ����� ���� �ڷ�ƾ�� ������ ���
            if (disableSetRoutine != null)
            {
                StopCoroutine(disableSetRoutine);
                disableSetRoutine = null;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopCollecting();

            if (sizeRoutine != null) 
                StopCoroutine(sizeRoutine);

            sizeRoutine = StartCoroutine(ResizeSprite(new Vector2(1f, 1f), 0.2f));
            // 0.3�� �� isTrigger ���� �ڷ�ƾ ����
            disableSetRoutine = StartCoroutine(DisableTriggerAfterDelay(0.3f));
        }
    }

    private void StopCollecting()
    {
        
        if (collectRoutine != null)
        {
            StopCoroutine(collectRoutine);
            collectRoutine = null;
        }
        isdisplaying = false;
    }

    //�̺κ� �ٽú���
    private IEnumerator CollectItemCoroutine(Player player)
    {
        isdisplaying = true;

        while (true)
        {
            //  �Ŵ밡 ���������� + ����ִ� �������� 1���̻� + ����ִ°� �Ŵ���̶� ��������
            bool hasItem = player.CurStack > 0;
            bool notFull = s_ob.s_items.Count < s_ob.MaxDisplay;
            bool canStackSameItem =  player.curitem == s_ob.S_itemType;

            if (hasItem && notFull && canStackSameItem)
            {
                Debug.Log($"�÷��̾ �������� �����ߴ�!");
                GameObject getobs = player.stackobs[player.stackobs.Count - 1];

                //�θ� ����
                getobs.transform.parent = s_ob.settingTrs[s_ob.s_items.Count];
                //�÷��̾ �Ѱ��ֱ�
                player.SetItemSO(s_ob, getobs.transform, s_ob.settingTrs[s_ob.s_items.Count], 0.2f, 1f);

                //�÷��̾� ���� ����
                player.CurStack--;
            }
            else
            {
                Debug.Log("���� �����");
            }
            yield return new WaitForSeconds(takeInterval);

        }
    }
    //�Ŵ� ������ �ٷγ������������� ������ ���� ����
    IEnumerator DisableTriggerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        s_ob.IsSetting = false;
        disableSetRoutine = null;

        //����� 
        s_ob.Re_SettingCustomer();
    }

    //���� ũ������
    private IEnumerator ResizeSprite(Vector2 targetSize, float duration)
    {
        Vector2 startSize = sprite_renderer.size;
        float time = 0f;

        while (time < duration)
        {
            sprite_renderer.size = Vector2.Lerp(startSize, targetSize, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        sprite_renderer.size = targetSize;
    }
}
