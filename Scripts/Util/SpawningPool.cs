using System.Collections.Generic;
using UnityEngine;

public class SpawningPool : MonoBehaviour
{
    string prefabPath = string.Empty;

    public Queue<GameObject> pooledObj_queue { get; private set; } = new Queue<GameObject>();


    public void SetSpawingPool(int pSpawner_poolAmount, Define.Prefabs pPrefabType, int pPrefabID)
    {
		prefabPath = FindPrefabPath(pPrefabType, pPrefabID);

		// °´Ã¼ »ý¼º
		GameObject go = null;
		for (int j = 0; j < pSpawner_poolAmount; j++)
		{
			go = CreatePooledObject();
			DespawnPooledObject(go);
		}
	}

	public GameObject SpawnPooledObject(Vector3 pPos, Quaternion pQuaternion, Transform _parent = null)
	{
		bool poolExpand = true;
		GameObject pooledObj = null;

		if (pooledObj_queue.Count > 0)
		{
			pooledObj = pooledObj_queue.Dequeue();
		}
		else if (poolExpand)
		{
			pooledObj = CreatePooledObject();
		}
		
		if (pooledObj != null)
        {
			pooledObj.transform.position	= pPos;
			pooledObj.transform.rotation	= pQuaternion;
			pooledObj.transform.parent		= _parent;
			pooledObj.SetActive(true);

			return pooledObj;
		}

		Debug.LogError($"Unable to spawn path({prefabPath}).");
		return null;
	}

	public void DespawnPooledObject(GameObject pPooledObj)
	{
		if (pPooledObj == null)
			return;

		pooledObj_queue.Enqueue(pPooledObj);
		pPooledObj.transform.parent			= transform;
		pPooledObj.transform.localPosition	= Vector3.zero;
		pPooledObj.transform.localRotation	= Quaternion.identity;
		pPooledObj.SetActive(false);
	}

	GameObject CreatePooledObject()
	{
		GameObject pooledObj = GlobalScene.ResourceMng.Instantiate(prefabPath);
		return pooledObj;
	}

    void DestroyPooledObject(GameObject pPooledObj)
    {
        if (pPooledObj != null)
        {
            GlobalScene.ResourceMng.DestroyGameObject(pPooledObj);
			pPooledObj = null;
        }
    }

	string FindPrefabPath(Define.Prefabs pPrefabType, int pPrefabID)
	{
		string prefabName = string.Empty;
		switch (pPrefabType)
		{
			case Define.Prefabs.Character:
				{
					Table.Character.Data characterData = GlobalScene.TableMng.CreateOrGetBaseTable<Table.Character>().GetTableData(pPrefabID);
					prefabName = characterData.prefabName;
				}
				break;
		}

		return $"Prefabs/{pPrefabType.ToString()}/{prefabName}";
	}


    //public static void PlayEffect(GameObject particleEffect, int particlesAmount)
    //{
    //	if (particleEffect.GetComponent<ParticleSystem>())
    //	{
    //		particleEffect.GetComponent<ParticleSystem>().Emit(particlesAmount);
    //	}
    //}

    //public static void PlaySound(GameObject soundSource)
    //{
    //	if (soundSource.GetComponent<AudioSource>())
    //	{
    //		soundSource.GetComponent<AudioSource>().PlayOneShot(soundSource.GetComponent<AudioSource>().GetComponent<AudioSource>().clip);
    //	}
    //}

    //public static class PoolingSystemExtensions
    //{
    //    public static void DespawnOrDestroy(this GameObject myobject)
    //    {
    //        PoolingSystem_Ref.DestroyAPS(myobject);
    //    }

    //    public static void PlayEffect(this GameObject particleEffect, int particlesAmount)
    //    {
    //        PoolingSystem_Ref.PlayEffect(particleEffect, particlesAmount);
    //    }

    //    public static void PlaySound(this GameObject soundSource)
    //    {
    //        PoolingSystem_Ref.PlaySound(soundSource);
    //    }
}

