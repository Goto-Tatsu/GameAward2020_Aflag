﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] List<GameObject> gameObjects;
    private int ListSize = 0;
    private int NextStageNum = 0;
    private int CurrentStageNum = -1;
    private GameObject StageObject;
    private Quaternion StartRotate;



    public static GameController instance { get; private set; }
    public float StartDelay = 5;
    Water2D.Water2D_Spawner Water2D_Spawner;
    GameObject[] waterdrop;
    Pausable pausable;
    private Goal goal;
    private Ball ball;

    /*
     ステージ遷移したい場合　必要なところでこれを呼ぶ
        GameController.instance.NextScene();
    */

    void Start()
    {
        pausable = gameObject.GetComponent<Pausable>();

        if (instance == null)
        {

            instance = this;
        }
        else
        {

            Destroy(gameObject);
        }

        ListSize = gameObjects.Count;
        StageObject = null;
        NextStageNum = 0;
        CurrentStageNum = -1;
       
    }

    // Update is called once per frame
    void Update()
    {
        if(NextStageNum != CurrentStageNum)
        {
            InstantiateStage(NextStageNum);
        }
        
    }
    void InstantiateStage(int StageNum)
    {
        if (StageNum <= ListSize)
        {
            if (StageObject) { Destroy(StageObject); }
            StageObject = Instantiate(gameObjects[StageNum]);
            StageObject.transform.SetParent(transform);
            CurrentStageNum = StageNum;
            Water2D_Spawner = GameObject.Find("Water2D_Spawner").GetComponent<Water2D.Water2D_Spawner>();
            goal = GameObject.Find("Goal").GetComponent<Goal>();
            ball = GameObject.Find("Ball").GetComponent<Ball>();

            Invoke("DelayMethod_Sawn", StartDelay);
        }
    }

    void DelayMethod_Sawn()
    {
        {
            if (pausable.pausing)
            {
                Invoke("DelayMethod_Sawn", 1.0f);
                Debug.Log("DelayMethod_Sawn_retry");
            }
            else
            {
                Water2D_Spawner.Spawn();
                Debug.Log("DelayMethod_Sawn");
            }
        }
    }
   
    private void EndGame()
    {

    }

    public int NextStage()
    {
        NextStageNum++;
       // if(NextStageNum >= ListSize) { EndGame(); } resultシーンなど
        NextStageNum %= ListSize;

        return NextStageNum;
    }
    public int GetCurret()
    {
        return CurrentStageNum;
    }

    public void Restart()
    {
       
        StageObject.GetComponentInChildren<stage_rotation>().RotZero();
        Water2D_Spawner.DelWater();
        Water2D_Spawner.Spawn();
        //InstantiateStage(CurrentStageNum);
    }
    

    public Goal GetGoal()
    {
        return goal;
    }

    public Pausable GetPausable()
    {
        return pausable;
    }

    public Ball GetBall()
    {
        return ball;
    }
}
