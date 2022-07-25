using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnManager : MonoBehaviour
{
    static public MonsterSpawnManager Instance;

    public Transform SpawnMap;
    public Vector2 PatrolMapSize;
    public Vector2 SpawnMapsize;
    public Vector3 PatrolArea;
    public Vector3 SpawnArea;
    public Transform KingMovePos;
    public Vector2 KingMapSize;
    public Vector3 KingMoveArea;
    int[] MaxCount = new int[3];


    private void Awake()
    {
        Instance = this;
        PatrolMapSize = new Vector2(SpawnMap.localScale.x * 0.5f, SpawnMap.localScale.z *0.5f);
        SpawnMapsize = new Vector2(SpawnMap.localScale.x * 0.25f, SpawnMap.localScale.z * 0.25f);
        KingMapSize = new Vector2(KingMovePos.localScale.x * 0.5f, KingMovePos.localScale.z * 0.5f);
    }

    private void Start()
    {
        for(int i = 0; i < 2; i++)
            for(int j=0; j < 3; j++)
                SpawnMonster(j);

    }

    public void ReservationSpawn(int index)
    {
        MaxCount[index]--;
        StartCoroutine(ReSpawn(index));
    }

    IEnumerator ReSpawn(int index)
    {
        yield return new WaitForSeconds(10.0f);
        SpawnMonster(index);
    }

    public void SpawnMonster(int index)
    {

        if (MaxCount[index] < 3)
        {
            GameObject monster = ObjectPool.Instance.GetMonster(index);
            monster.transform.position = GetSpawnVector();

            MaxCount[index]++;
        }
        else
            return;
    }

    public Vector3 GetSpawnVector()
    {
        SpawnArea = new Vector3(Random.Range(SpawnMap.position.x - SpawnMapsize.x, SpawnMap.position.x + SpawnMapsize.x), 1.0f
           , Random.Range(SpawnMap.position.z - SpawnMapsize.y, SpawnMap.position.z + SpawnMapsize.y));

        return SpawnArea;
    }


    public Vector3 GetPatrolVector(Transform tr)
    {

       PatrolArea = new Vector3(Random.Range(SpawnMap.position.x - PatrolMapSize.x, SpawnMap.position.x + PatrolMapSize.x), tr.transform.position.y
           , Random.Range(SpawnMap.position.z - PatrolMapSize.y, SpawnMap.position.z + PatrolMapSize.y));
    

        return PatrolArea;
    }

    public Vector3 GetKingMovePos()
    {

        KingMoveArea = new Vector3(Random.Range(KingMovePos.position.x - KingMapSize.x, KingMovePos.position.x + KingMapSize.x), KingMovePos.position.y
            , Random.Range(KingMovePos.position.z - KingMapSize.y, KingMovePos.position.z + KingMapSize.y));


        return KingMoveArea;
    }








}
