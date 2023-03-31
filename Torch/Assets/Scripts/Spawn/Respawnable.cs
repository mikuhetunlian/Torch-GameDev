using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CheckPoint µÄ½Ó¿Ú
/// </summary>
public interface Respawnable
{
    public void OnPlayerRespawn(CheckPoint checkPoint,Player player);
}
