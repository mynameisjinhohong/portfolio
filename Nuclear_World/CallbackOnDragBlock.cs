using MG_BlocksEngine2.Block;
using MG_BlocksEngine2.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallbackOnDragBlock : MonoBehaviour
{
    void OnEnable()
    {
        BE2_MainEventsManager.Instance.StartListening(BE2EventTypesBlock.OnDrop, OnDrop);
        BE2_MainEventsManager.Instance.StartListening(BE2EventTypes.OnBlockDrop, OnBlockDrop);
        BE2_MainEventsManager.Instance.StartListening(BE2EventTypesBlock.OnDropAtProgrammingEnv, OnDropAtProgrammingEnv);
        BE2_MainEventsManager.Instance.StartListening(BE2EventTypesBlock.OnDropAtInputSpot, OnDropAtInputSpot);
        BE2_MainEventsManager.Instance.StartListening(BE2EventTypesBlock.OnDropAtStack, OnDropAtStack);
        BE2_MainEventsManager.Instance.StartListening(BE2EventTypesBlock.OnDropDestroy, OnDropDestroy);
        BE2_MainEventsManager.Instance.StartListening(BE2EventTypesBlock.OnDragOut, OnDragOut);
        BE2_MainEventsManager.Instance.StartListening(BE2EventTypesBlock.OnDragFromInputSpot, OnDragFromInputSpot);
        BE2_MainEventsManager.Instance.StartListening(BE2EventTypesBlock.OnDragFromProgrammingEnv, OnDragFromProgrammingEnv);
        BE2_MainEventsManager.Instance.StartListening(BE2EventTypesBlock.OnDragFromStack, OnDragFromStack);
        BE2_MainEventsManager.Instance.StartListening(BE2EventTypesBlock.OnDragFromOutside, OnDragFromOutside);
    }

    void OnDisable()
    {
        BE2_MainEventsManager.Instance.StopListening(BE2EventTypesBlock.OnDrop, OnDrop);
        BE2_MainEventsManager.Instance.StopListening(BE2EventTypes.OnBlockDrop, OnBlockDrop);
        BE2_MainEventsManager.Instance.StopListening(BE2EventTypesBlock.OnDropAtProgrammingEnv, OnDropAtProgrammingEnv);
        BE2_MainEventsManager.Instance.StopListening(BE2EventTypesBlock.OnDropAtInputSpot, OnDropAtInputSpot);
        BE2_MainEventsManager.Instance.StopListening(BE2EventTypesBlock.OnDropAtStack, OnDropAtStack);
        BE2_MainEventsManager.Instance.StopListening(BE2EventTypesBlock.OnDropDestroy, OnDropDestroy);
        BE2_MainEventsManager.Instance.StopListening(BE2EventTypesBlock.OnDragOut, OnDragOut);
        BE2_MainEventsManager.Instance.StopListening(BE2EventTypesBlock.OnDragFromInputSpot, OnDragFromInputSpot);
        BE2_MainEventsManager.Instance.StopListening(BE2EventTypesBlock.OnDragFromProgrammingEnv, OnDragFromProgrammingEnv);
        BE2_MainEventsManager.Instance.StopListening(BE2EventTypesBlock.OnDragFromStack, OnDragFromStack);
        BE2_MainEventsManager.Instance.StopListening(BE2EventTypesBlock.OnDragFromOutside, OnDragFromOutside);
    }

    void OnDrop(I_BE2_Block block)
    {
        Debug.Log("on drop " + block);
    }

    void OnBlockDrop()
    {
        Debug.Log("on block drop ");
    }

    void OnDropAtProgrammingEnv(I_BE2_Block block)
    {
        Debug.Log("on drop at programming env " + block);
    }

    void OnDropAtInputSpot(I_BE2_Block block)
    {
        Debug.Log("on drop at input spot " + block);
    }

    void OnDropAtStack(I_BE2_Block block)
    {
        Debug.Log("on drop at stack " + block);
    }

    void OnDropDestroy(I_BE2_Block block)
    {
        Debug.Log("on drop destroy " + block);
    }

    // 

    void OnDragOut(I_BE2_Block block)
    {
        Debug.Log("on drag out " + block);
    }

    void OnDragFromInputSpot(I_BE2_Block block)
    {
        Debug.Log("on drag from input spot " + block);
    }

    void OnDragFromProgrammingEnv(I_BE2_Block block)
    {
        Debug.Log("on drag from prog env " + block);
    }

    void OnDragFromStack(I_BE2_Block block)
    {
        Debug.Log("on drag from stack " + block);
    }

    void OnDragFromOutside(I_BE2_Block block)
    {
        Debug.Log("on drag from outside " + block);
    }
}