using System.Collections.Generic;
using UnityEngine;
using UnityScript.Steps;

namespace LevelGeneration.Interfaces
{
    public interface IMeshGenPostProcess
    {
        /// <summary>
        /// Use this to modify any verts and stuff before
        /// they get passed to the mesh
        /// </summary>
        /// <param name="cell">Cell this is running on</param>
        /// <param name="triOffset">Where the tris for this cell belong in the tris list. All the tris after tris[offsetTris] belong to this cell (including tris[offsetTris])</param>
        /// <param name="vertOffset">Where the verts for this cell belong in the verts list. All the verts after verts[offsetVert] belong to this cell (including verts[offsetVert])</param>
        /// <param name="verts">The whole verts array up til now (includes verts the previous cells)</param>
        /// <param name="tris">The whole tris array up til now (includes tris from previous cells)</param>
        void PostProcess(CellData2D cell, int triOffset, int vertOffset, ref List<Vector3> verts, ref List<int> tris);
    }
}