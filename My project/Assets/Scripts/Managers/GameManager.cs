using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    protected override void Awake()
    {
        base.Awake();
    }

    public void TestGameManagers()
    {
        Debug.Log("Game Manager Is Working!");
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Quit();
    }
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        Debug.Log("Quit Function Is Working");
    }
}
