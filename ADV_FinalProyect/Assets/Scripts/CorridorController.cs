using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorridorController : MonoBehaviour
{
    [SerializeField] Material baseColor;
    [SerializeField] Material baseColorTransparent;
    [SerializeField] Material whiteEmission;
    [SerializeField] Material whiteEmissionTransparent;
    [SerializeField] Material lattice;
    [SerializeField] Material latticeTransparent;
    [SerializeField] Material glass;
    [SerializeField] Material glassTransparent;
    [SerializeField] MeshRenderer[] allLODMeshes;
    [SerializeField] MeshRenderer[] allMeshes;
    [SerializeField] GameObject[] redPosts;
    [SerializeField] MeshRenderer[] meshesLODNord;
    [SerializeField] MeshRenderer[] meshesNord;
    [SerializeField] GameObject[] redPostsNord;
    [SerializeField] MeshRenderer[] meshesLODEast;
    [SerializeField] MeshRenderer[] meshesEast;
    [SerializeField] GameObject[] redPostsEast;
    [SerializeField] MeshRenderer[] meshesLODWest;
    [SerializeField] MeshRenderer[] meshesWest;
    [SerializeField] GameObject[] redPostsWest;
    [SerializeField] MeshRenderer[] meshesLODSouth;
    [SerializeField] MeshRenderer[] meshesSouth;
    [SerializeField] GameObject[] redPostsSouth;
    Material[] _transparentMaterials;
    Material[] _normalMaterials;

    private void Start()
    {
        _transparentMaterials = new Material[4];
        _transparentMaterials[0] = baseColorTransparent; 
        _transparentMaterials[1] = whiteEmissionTransparent; 
        _transparentMaterials[2] = latticeTransparent; 
        _transparentMaterials[3] = glassTransparent; 

        _normalMaterials = new Material[4];
        _normalMaterials[0] = baseColor;
        _normalMaterials[1] = whiteEmission;
        _normalMaterials[2] = lattice;
        _normalMaterials[3] = glass;
    }

    public void SetNord()
    {
        ResetMaterials();
        SetTransparent(meshesLODNord, meshesNord, redPostsNord);
    }

    public void SetEast()
    {
        ResetMaterials();
        SetTransparent(meshesLODEast, meshesEast, redPostsEast);
    }

    public void SetWest()
    {
        ResetMaterials();
        SetTransparent(meshesLODWest, meshesWest, redPostsWest);
    }

    public void SetSouth()
    {
        ResetMaterials();
        SetTransparent(meshesLODSouth, meshesSouth, redPostsSouth);
    }

    public void SetNordWest()
    {
        ResetMaterials();
        SetTransparent(meshesLODNord, meshesNord, redPostsNord);
        SetTransparent(meshesLODWest, meshesWest, redPostsWest);
    }

    public void SetNordEast()
    {
        ResetMaterials();
        SetTransparent(meshesLODNord, meshesNord, redPostsNord);
        SetTransparent(meshesLODEast, meshesEast, redPostsEast);
    }

    public void SetSouthWest()
    {
        ResetMaterials();
        SetTransparent(meshesLODSouth, meshesSouth, redPostsSouth);
        SetTransparent(meshesLODWest, meshesWest, redPostsWest);
    }

    public void SetSouthEast()
    {
        ResetMaterials();
        SetTransparent(meshesLODSouth, meshesSouth, redPostsSouth);
        SetTransparent(meshesLODEast, meshesEast, redPostsEast);
    }

    void SetTransparent(MeshRenderer[] LODMeshes, MeshRenderer[] meshes, GameObject[] redPosts)
    {
        foreach (var mesh in LODMeshes)
            mesh.materials = _transparentMaterials;

        foreach (var mesh in meshes)
            mesh.material = baseColorTransparent;

        foreach (var post in redPosts)
            post.SetActive(false);
    }

    void ResetMaterials()
    {
        foreach (var mesh in allLODMeshes)
            mesh.materials = _normalMaterials;

        foreach (var mesh in allMeshes)
            mesh.material = baseColor;

        foreach (var post in redPosts)
            post.SetActive(true);
    }
}
