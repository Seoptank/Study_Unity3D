using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private class PoolItem
    {
        public bool         isActive;   // ������Ʈ Ȱ��/ ��Ȱ�� ����
        public GameObject   gameObj;    // ȭ�鿡 ���̴� ���� ������Ʈ
    }

    private int                 increaseCount = 5;  // ������Ʈ ������ ������ ����
    private int                 maxCount;           // ���� ����Ʈ�� ��ϵ� ������Ʈ ��
    private int                 activeCount;        // Ȱ��ȭ�� ������Ʈ ��

    private GameObject          poolObj;            // PoolManager���� �����ϴ� ������
    private List<PoolItem>      poolItemList;       // �����Ǵ� ��� ������Ʈ�� �����ϴ� ����Ʈ
    
    public PoolManager(GameObject poolObj)
    {
        maxCount = 0;
        activeCount = 0;
        this.poolObj = poolObj;

        poolItemList = new List<PoolItem>();

        InstantiateObjects();
    }

    public void InstantiateObjects()
    {
        maxCount += increaseCount;

        for (int i = 0; i < increaseCount; ++i)
        {
            PoolItem poolItem = new PoolItem();

            poolItem.isActive = false;
            poolItem.gameObj = GameObject.Instantiate(poolObj);
            poolItem.gameObj.SetActive(false);

            poolItemList.Add(poolItem);
        }
    }

    public void DestroyObjects()
    {
        if (poolItemList == null) return;

        int count = poolItemList.Count;

        for (int i = 0; i < count; ++i)
        {
            GameObject.Destroy(poolItemList[i].gameObj);
        }
        poolItemList.Clear();
    }

    public GameObject ActivatePoolItem()
    {
        if (poolItemList == null) return null;

        if(maxCount == activeCount)
        {
            InstantiateObjects();
        }

        int count = poolItemList.Count;

        for (int i = 0; i < count; ++i)
        {
            PoolItem poolItem = poolItemList[i];

            if(!poolItem.isActive)
            {
                activeCount++;

                poolItem.isActive = true;
                poolItem.gameObj.SetActive(true);

                return poolItem.gameObj;
            }
        }

        return null;
    }

    public void DeactivatePoolItem(GameObject removeObj)
    {
        if (poolItemList == null || removeObj == null) return;

        int count = poolItemList.Count;

        for (int i = 0; i < count; ++i)
        {
            PoolItem poolItem = poolItemList[i];

            if(poolItem.gameObj == removeObj)
            {
                activeCount--;

                poolItem.isActive = false;
                poolItem.gameObj.SetActive(false);

                return;
            }
        }
    }

    public void DeactivateAllPoolItem()
    {
        if (poolItemList == null) return;

        int count = poolItemList.Count;

        for (int i = 0; i < count; ++i)
        {
            PoolItem poolItem = poolItemList[i];
            
            if(poolItem.gameObj != null && poolItem.isActive)
            {
                poolItem.isActive = false;
                poolItem.gameObj.SetActive(false);
            }
        }

        activeCount = 0;
    }    
}
