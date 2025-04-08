using System;
using System.Collections;
using System.Collections.Generic;
using Blocks;
using UnityEngine;
using User;
using Random = System.Random;

[Serializable]
public class CarUpgradeBehaviour : MonoBehaviour
{
    [SerializeField] List<Material> upgradeMaterials = new List<Material>();
    
    private Dictionary<BlockType, Material> _blockMaterials;

    private void Start()
    {
        Dictionary<BlockType, Material> blockMaterials = new Dictionary<BlockType, Material>();
        for (int i = 0; i < upgradeMaterials.Count; i++)
        {
            blockMaterials.Add((BlockType)i, upgradeMaterials[i]);
        }

        _blockMaterials = blockMaterials;

    }

    private void Upgrade(BlockType upgradeType)
    {
        StartCoroutine(MaterialShine(_blockMaterials[upgradeType]));
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(MaterialShine(upgradeMaterials[0]));
        }
    }

    private IEnumerator MaterialShine(Material material)
    {
        Debug.Log(material);
        Color c = material.color;
        Color white = new Color(1f, 1f, 1f, 1f);

        material.color = white;
        yield return new WaitForSeconds(0.3f);
        material.color = c;
        yield return new WaitForSeconds(0.3f);
        material.color = white;
        yield return new WaitForSeconds(0.3f);
        material.color = c;
        yield return new WaitForSeconds(0.3f);
        material.color = white;
        yield return new WaitForSeconds(0.3f);
        material.color = c;
        yield return new WaitForSeconds(0.3f);
        material.color = white;
        yield return new WaitForSeconds(0.3f);
        material.color = c;
        yield return new WaitForSeconds(0.3f);
        material.color = white;
        yield return new WaitForSeconds(0.3f);
        material.color = c;
        yield return new WaitForSeconds(0.3f);
        material.color = white;
        yield return new WaitForSeconds(0.3f);
        material.color = c;
        yield return new WaitForSeconds(0.3f);
        material.color = white;
        yield return new WaitForSeconds(0.3f);
        material.color = c;
        yield return new WaitForSeconds(0.3f);
    }
}
