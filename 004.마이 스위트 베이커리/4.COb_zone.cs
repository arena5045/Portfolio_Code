using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class COb_zone : MonoBehaviour
{
    // ����� �۾�����
    public Create_Ob c_ob;                
    // �۾� �ҿ�ð�
    public float takeInterval = 0.2f;
    //������ ����������?
    private bool isCollecting = false;
    //������ ������ �ڷ�ƾ
    private Coroutine collectRoutine;
    //�� ���� ũ�� Ŀ���� �۾����� �ϴ� �ڷ�ƾ
    private Coroutine sizeRoutine;
    //������ ���� Ŀ����
    public float resizeValue;
    //��������Ʈ ������
    private SpriteRenderer sprite_renderer;


    private void Awake()
    {
        sprite_renderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isCollecting)
        {
            if (sizeRoutine != null) StopCoroutine(sizeRoutine);
            //�÷��̾ ������ �ڷ�ƾ ���� + ���� Ŀ��
            sizeRoutine = StartCoroutine(ResizeSprite(new Vector2(resizeValue, resizeValue), 0.2f));
            collectRoutine = StartCoroutine(CollectItemCoroutine(other.GetComponent<Player>()));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        { //�÷��̾ ������ �ڷ�ƾ �ٷ�����
            StopCollecting();
            if (sizeRoutine != null)
                StopCoroutine(sizeRoutine);

            //���� �۾���
            sizeRoutine = StartCoroutine(ResizeSprite(new Vector2(1f, 1f), 0.2f));
        }
    }

    //�ڷ�ƾ ����
    private void StopCollecting()
    {
        if (collectRoutine != null)
        {
            StopCoroutine(collectRoutine);
            collectRoutine = null;
        }
        isCollecting = false;
    }

    //�÷��̾ ������ �����ϴ� �ڷ�ƾ
    private IEnumerator CollectItemCoroutine(Player player )
    {
        isCollecting = true;

        while (true)
        {
            // ���� �������� 1���̻� + �÷��̾ ���� ���������� + ����ִ°� ���ų� ���� ��������
            bool hasItem = c_ob.item_list.Count > 0;
            bool notFull = !player.isMax;
            bool canStackSameItem = player.curitem == null || player.curitem == c_ob.S_itemType;

            if (hasItem && notFull && canStackSameItem)
            {
                // ���� ���� �ƹ� �����۵� �� ��� ������, ���� ���� ����
                if (player.curitem == null)
                {
                    player.curitem = c_ob.S_itemType;
                }

                Debug.Log($"�÷��̾ ���� ȹ���ߴ�!  ");
                GameObject getobs = c_ob.item_list[c_ob.item_list.Count - 1];
                Rigidbody rb = getobs.GetComponent<Rigidbody>();
                rb.useGravity = false;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = true;

                getobs.transform.parent = player.stackTrs;
                c_ob.item_list.RemoveAt(c_ob.item_list.Count - 1);
                player.GetItem(getobs.transform, c_ob.S_itemType.size,0.2f,1f);
                player.CurStack++;
            }
            else
            {
                Debug.Log("���� �����");
            }

            yield return new WaitForSeconds(takeInterval);
        }
    }

    //�ٴ� ������ �������ϴ� �ڷ�ƾ
    private IEnumerator ResizeSprite(Vector2 targetSize, float duration)
    {
        Vector2 startSize = sprite_renderer.size;
        float time = 0f;

        while (time < duration)
        {
            //�ε巴�� ����
            sprite_renderer.size = Vector2.Lerp(startSize, targetSize, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        sprite_renderer.size = targetSize;
    }

}
