using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CheckPoint �Ľӿ�
/// </summary>
public interface Respawnable
{
    public void OnPlayerRespawn(CheckPoint checkPoint,Player player);
}
