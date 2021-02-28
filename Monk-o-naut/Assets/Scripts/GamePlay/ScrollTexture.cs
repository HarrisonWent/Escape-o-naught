using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollTexture : MonoBehaviour
{
    public MeshRenderer MR;
    public Vector2 Speed;
    public string TexName;
    public string MaterialName;
    private void Update()
    {
        foreach(Material m in MR.materials)
        {
            if(m.name.Contains(MaterialName))
            {
                Vector2 Currentoffset = m.GetTextureOffset(TexName);
                Currentoffset += (Speed*Time.deltaTime);

                if (Currentoffset.x < 0f) { Currentoffset.x = 1f; }
                else if (Currentoffset.x > 1f) { Currentoffset.x = 0f; }
                if(Currentoffset.y < 0f) { Currentoffset.y = 1f; }
                else if(Currentoffset.y > 1f) { Currentoffset.y = 0f; }

                m.SetTextureOffset(TexName,Currentoffset);

                return;
            }
        }
    }
}
