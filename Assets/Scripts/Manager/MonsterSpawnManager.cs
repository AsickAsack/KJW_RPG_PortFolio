using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnManager : MonoBehaviour
{
    static public MonsterSpawnManager Instance;

    public Transform SpawnMap;
    public Vector2 PatrolMapSize;
    public Vector2 SpawnMapsize;
    public Transform KingMovePos;
    public Vector2 KingMapSize;
    public Transform Player;
    public Transform[] PatrolPoint;


    private void Awake()
    {
        Instance = this;
        PatrolMapSize = new Vector2(SpawnMap.localScale.x * 0.5f, SpawnMap.localScale.z *0.5f);
        SpawnMapsize = new Vector2(SpawnMap.localScale.x * 0.25f, SpawnMap.localScale.z * 0.25f);
        KingMapSize = new Vector2(KingMovePos.localScale.x * 0.5f, KingMovePos.localScale.z * 0.5f);
    }

    private void Start()
    {
        int index = 0;
        
        for(int i = 0; i < 3; i++)
            for(int j=0; j < 3; j++)
                SpawnMonster(j,index++);
        
    }

    public void ReservationSpawn(int index, int patrolindex)
    {
        StartCoroutine(ReSpawn(index,patrolindex));
    }

    IEnumerator ReSpawn(int index, int patrolindex)
    {
        yield return new WaitForSeconds(10.0f);
        SpawnMonster(index, patrolindex);
    }

    Soldier soldier;

    public void SpawnMonster(int index,int patrolindex)
    {

        GameObject monster = ObjectPool.Instance.GetMonster(index);
        soldier = monster.GetComponent<Soldier>();
        soldier.patrolIndex = patrolindex;
        soldier.myNavi.Warp(PatrolPoint[patrolindex].position);
        soldier.myNavi.enabled = true;
        soldier.ChangeState(Soldier.S_State.Patrol);
    }

    public Vector3 GetSpawnVector()
    {
        return new Vector3(Random.Range(SpawnMap.position.x - SpawnMapsize.x, SpawnMap.position.x + SpawnMapsize.x), 1.0f
           , Random.Range(SpawnMap.position.z - SpawnMapsize.y, SpawnMap.position.z + SpawnMapsize.y)); ;
    }

    public Vector3 GetPatrolDir(int patrolIndex,Transform tr)
    {
        if (patrolIndex <= 2)
        {
            return new Vector3(Random.Range(PatrolPoint[patrolIndex].position.x - 4.0f, PatrolPoint[patrolIndex].position.x + 4.0f), 1.0f,
                tr.transform.position.z);
        }
        else
        {
            return new Vector3(Random.Range(PatrolPoint[patrolIndex].position.x - 2.5f, PatrolPoint[patrolIndex].position.x + 2.5f), 1.0f,
                Random.Range(PatrolPoint[patrolIndex].position.z - 2.5f, PatrolPoint[patrolIndex].position.z + 2.5f));
        }
        
    }

    public Vector3 GetStandDir(int patrolIndex)
    {
        return PatrolPoint[patrolIndex].position;
    }


    public Vector3 GetKingMovePos()
    {
        return new Vector3(Random.Range(KingMovePos.position.x - KingMapSize.x, KingMovePos.position.x + KingMapSize.x), KingMovePos.position.y
            , Random.Range(KingMovePos.position.z - KingMapSize.y, KingMovePos.position.z + KingMapSize.y));
    }








}
