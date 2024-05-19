using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private class PoolItem
    {
        public bool         isActive;   // 오브젝트 활성/ 비활성 정보
        public GameObject   gameObj;    // 화면에 보이는 실제 오브젝트
    }

    private int                 increaseCount = 5;  // 오브젝트 부족시 생성할 갯수
    private int                 maxCount;           // 현재 리스트에 등록된 오브젝트 수
    private int                 activeCount;        // 활성화된 오브젝트 수

    private GameObject          poolObj;            // PoolManager에서 관리하는 프리팹
    private List<PoolItem>      poolItemList;       // 관리되는 모든 오브젝트를 저장하는 리스트
    
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
