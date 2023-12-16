using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllocGid : Singleton<AllocGid>
{
    private int gid;

    public override void Init()
    {
        base.Init();
        gid = 0;
    }

    public int Alloc()
    {
        gid += 1;
        return gid;
    }
}
