using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notes : MonoBehaviour
{
    /*
     * AppData/LocalLow/CompanyName/ProductName (Log files)
     * public override void OnNetworkSpawn(){ } instead of using start
     * cannot use reference types in networking (no string, class, array), structs work
     * public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter{ serializer.serializeValue(ref [var])} to use structs
     * FixedString to send strings
     * 
     */
}
