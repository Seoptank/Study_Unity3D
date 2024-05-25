using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private class PoolItem
    {
        public GameObject gameObject;     // 화면에 있는 실제 게임 오브젝트
        public bool       isActive;       // "gameObject" 활성/비활성
    }

    private int increaseCnt = 5;            // 오브젝트 부족한 경우 추가 생성할 오브젝트 개수
    private int maxCnt;                     // 현재 리스트에 등록된 오브젝트 개수
    private int activeCnt;                  // 현재 활성화된 오브젝트 개수

    private GameObject poolObject;     // PoolManager에서 관리하는 프리팹들     
    private List<PoolItem> poolItemList;   // 관리하는 오브젝트를 저장하는 List

    public int MaxCnt => maxCnt;      // 외부에서 현재 리스트에 등록된 오브젝트 개수 확인용 프로퍼티
    public int ActiveCnt => activeCnt;   // 외부에서 현대 활성화된 오브젝트 확인용 프로퍼티

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

        // 현재 관리하는 오브젝트가 모두 활성화 중일 때 새로운 오브젝트 삽입
        if (maxCnt == activeCnt) InstantiateObejcts();

        int cnt = poolItemList.Count;

        for (int i = 0; i < cnt; ++i)
        {
            PoolItem item = poolItemList[i];

            // 아이템이 비활성화 중이면?
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

            // 아이템이 존재하고있고 활성화 중일 때
            if(item.gameObject != null && item.isActive)
            {
                item.isActive = false;
                item.gameObject.SetActive(false);
            }
        }

        activeCnt = 0;
    }

}
