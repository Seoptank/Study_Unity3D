using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private class PoolItem
    {
        public GameObject gameObject;     // ȭ�鿡 �ִ� ���� ���� ������Ʈ
        public bool       isActive;       // "gameObject" Ȱ��/��Ȱ��
    }

    private int increaseCnt = 5;            // ������Ʈ ������ ��� �߰� ������ ������Ʈ ����
    private int maxCnt;                     // ���� ����Ʈ�� ��ϵ� ������Ʈ ����
    private int activeCnt;                  // ���� Ȱ��ȭ�� ������Ʈ ����

    private GameObject poolObject;     // PoolManager���� �����ϴ� �����յ�     
    private List<PoolItem> poolItemList;   // �����ϴ� ������Ʈ�� �����ϴ� List

    public int MaxCnt => maxCnt;      // �ܺο��� ���� ����Ʈ�� ��ϵ� ������Ʈ ���� Ȯ�ο� ������Ƽ
    public int ActiveCnt => activeCnt;   // �ܺο��� ���� Ȱ��ȭ�� ������Ʈ Ȯ�ο� ������Ƽ

    public PoolManager(GameObject poolObject)
    {
        maxCnt          = 0;
        activeCnt       = 0;
        this.poolObject = poolObject;

        poolItemList = new List<PoolItem>();

        InstantiateObejcts();
    }

    public void InstantiateObejcts()
    {
        maxCnt += increaseCnt;

        for (int i = 0; i < increaseCnt; ++i)
        {
            PoolItem item = new PoolItem();

            item.isActive = false;
            item.gameObject = GameObject.Instantiate(poolObject);
            item.gameObject.SetActive(false);

            poolItemList.Add(item);
        }
    }

    public void DestroyObjects()
    {
        if (poolItemList == null) return;

        int cnt = poolItemList.Count;

        for (int i = 0; i < cnt; ++i)
        {
            GameObject.Destroy(poolItemList[i].gameObject);
        }

        poolItemList.Clear();
    }

    public GameObject ActivatePoolItem()
    {
        if (poolItemList == null) return null;

        // ���� �����ϴ� ������Ʈ�� ��� Ȱ��ȭ ���� �� ���ο� ������Ʈ ����
        if (maxCnt == activeCnt) InstantiateObejcts();

        int cnt = poolItemList.Count;

        for (int i = 0; i < cnt; ++i)
        {
            PoolItem item = poolItemList[i];

            // �������� ��Ȱ��ȭ ���̸�?
            if(!item.isActive)
            {
                activeCnt++;

                item.isActive = true;
                item.gameObject.SetActive(true);

                return item.gameObject;
            }
        }

        return null;
    }

    public void DeactivatePoolItems(GameObject removeObject)
    {
        if (poolItemList == null || removeObject == null) return;

        int cnt = poolItemList.Count;

        for (int i = 0; i < cnt; ++i)
        {
            PoolItem item = poolItemList[i];

            if(item.gameObject == removeObject)
            {
                activeCnt--;

                item.isActive = false;
                item.gameObject.SetActive(false);


                return;
            }
        }
    }

    public void DeactivateAllPoolItems()
    {
        if (poolItemList == null) return;

        int cnt = poolItemList.Count;

        for (int i = 0; i < cnt; ++i)
        {
            PoolItem item = poolItemList[i];

            // �������� �����ϰ��ְ� Ȱ��ȭ ���� ��
            if(item.gameObject != null && item.isActive)
            {
                item.isActive = false;
                item.gameObject.SetActive(false);
            }
        }

        activeCnt = 0;
    }

}
