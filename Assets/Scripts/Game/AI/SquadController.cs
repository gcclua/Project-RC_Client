﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding.RVO;
using Pathfinding;
using GCGame.Table;

/// <summary>
/// 小队行为控制器
/// by Zhengxuesong
/// 2016-11-11
/// </summary>

public class SquadController : BaseController
{
    private RichAI mRichAI;

    // States of the Team 
    private StateMachine mSquadStateMachine;
    // the Squad Date
    private SquadData mSquadData;
    //the Unit Date List
    private List<Transform> mUnitTransformList;
    // EnemySquad List
    private Dictionary<int, SquadController> mEnemySquadDic;

    private Transform mSearchAim;
    private Transform mAttackAim;
    private Transform mOriginalAim;
    //Marching state;
    private bool mIsMarching;

    // the Attack Timer
    private float mAttackSpaceTimer;

    //private float mLastTime;
    private float mMinDistance = 0.5f;

    //the Squad Brain Thinking Time
    private float mThinkTime;
    private const float mThinkTimeSpace = 0.05f;

    //the Squad Brain Thinking Time
    private float mSynTime;
    private const float mSynTimeTimeSpace = 0.5f;

    //Skill
    private Transform mSkill;
    private SkillEffect mSkillController;


    //TeamFormation infor
    //the Unit radius
    float UnitR;
    float UnitSpaceL;
    float UnitSpaceH;

    //the space between two Column
    float DeltaL;
    //the space between two Row
    float DeltaH;
    //the Squad length
    float SquadL;
    //the Squad Width
    float SquadH;
    
    float MaxNumberInRow;
    float MaxNumberInColumn;

    void Awake()
    {
        mRichAI = GetComponent<RichAI>();
        mSquadData = new SquadData();
        mUnitTransformList = new List<Transform>();
        mEnemySquadDic = new Dictionary<int, SquadController>();

        InitAwake();

        mIdleState.mdGetInState += SquadIdleGetInFunc;
        mIdleState.mdGetOutState += SquadIdleGetOutFunc;
        mIdleState.mdExcuteState += SquadIdleExcuteFunc;

        mWalkState.mdGetInState += SquadWalkGetInFunc;
        mWalkState.mdExcuteState += SquadWalkExcuteFunc;
        mWalkState.mdGetOutState += SquadWalkGetOutFunc;

        mAttackState.mdGetInState += SquadAttackGetInFunc;
        mAttackState.mdExcuteState += SquadAttackExcuteFunc;
        mAttackState.mdGetOutState += SquadAttackGetOutFunc;

        mPrepareState.mdGetInState += SquadPrepareGetInFunc;
        mPrepareState.mdExcuteState += SquadPrepareExcuteFunc;
        mPrepareState.mdGetOutState += SquadPrepareGetOutFunc;

        mDieState.mdGetInState += SquadDieGetInFunc;
        mDieState.mdExcuteState += SquadDieExcuteFunc;
        mDieState.mdGetOutState += SquadDieGetOutFunc;

        mStateMachine.StartWorking();
        EventManager.GetInstance().AddEventListener(EventId.SomeSquadDie, SomeSquadDie);
        EventManager.GetInstance().AddEventListener(EventId.ReSetPosition, ReSetPosition);
    }
    public Transform GetAttackAim()
    {
        return mAttackAim;
    }
    public List<Transform> GetUnitControllerList()
    {
        return mUnitTransformList;
    }

    public void AddEnemy(int id, SquadController tSquad)
    {
        mEnemySquadDic.Add(id,tSquad);
    }
    public SquadData GetSquadData()
    {
        return mSquadData;
    }
    public void SetSquadData(SquadData tSquadData)
    {
        mSquadData = tSquadData;
    }

    // Update is called once per frame
    void Update()
    {
        /******************
        Squad StateMachine Running
        *******************/
        if (mThinkTime < float.Epsilon)
        {
            mStateMachine.StateWork();
            mThinkTime = mThinkTimeSpace;
            if (BattleMode.Client)
            {
                SearchAim();
                SearchAttackAim();
            }
        }
        else
        {
            mThinkTime -= Time.deltaTime;
        }
    }
    public void ChangeTarget(Transform tTarget)
    {

        mIsMarching = false;
        mRichAI.target = tTarget;
        mRichAI.UpdatePath();
    }
    public void SetmOriginalAim(Transform tTarget)
    {

        mRichAI.target = tTarget;
        mOriginalAim = mRichAI.target;
        mRichAI.UpdatePath();
    }

    //serach Attack Aim;
    public void SearchAim()
    {
        if (mSearchAim == null && mAttackAim == null)
        {
            foreach (var enemySquad in mEnemySquadDic)
            {
                float temDistance = Vector3.Distance(transform.position, enemySquad.Value.transform.position);
                if (temDistance <= mSquadData.GetSquadView())
                {
                    mSearchAim = enemySquad.Value.transform;
                    ChangeTarget(mSearchAim);
                    break;
                }               
            }
        }
    }
    //search final Attack Aim
    public void SearchAttackAim()
    {
        if (mAttackAim == null && mSearchAim != null)
        {
            //search Attack Aim
            foreach (var enemySquad in mEnemySquadDic)
            {
                float temDistance = Vector3.Distance(transform.position, enemySquad.Value.transform.position);
                if (temDistance <= mSquadData.GetSquadAttackRange())
                {
                    mAttackAim = enemySquad.Value.transform;

                    int tTargetID = mAttackAim.GetComponent<SquadController>().GetSquadData().GetID();
                    RefreshUnitEnemyList(tTargetID);
                    UnitAttackEnemy(tTargetID);
                   

                    ChangeTarget(mAttackAim);
                    TryOperState(StateEnum.Prepare, StateOper.Enter);
                    break;
                }
            }
        }
    }
    /******************
    Idle State Function 
    *******************/
    void SquadIdleGetInFunc()
    {
        //LogModule.DebugLog("ID: " + mSquadData.GetID() + "Idle Get In");
        if (mOriginalAim != null)
            ChangeTarget(mOriginalAim);
        ChangeMoving(false);
    }
    void SquadIdleGetOutFunc()
    {
        //LogModule.DebugLog("ID: " + mSquadData.GetID() + "Idle Get Out");
    }
    void SquadIdleExcuteFunc()
    {
        if (mAttackSpaceTimer >= mSquadData.GetAttackSpaceTime())
            mAttackSpaceTimer = mSquadData.GetAttackSpaceTime();
        else
            mAttackSpaceTimer += Time.deltaTime;


        if (mRichAI.enabled && mRichAI.Velocity.sqrMagnitude > 0.01f)
        {
            TryOperState(StateEnum.Walk, StateOper.Enter);
        }
    }
    /******************
    Walk State Function 
    *******************/
    void SquadWalkGetInFunc()
    {
        //LogModule.DebugLog("ID: " + mSquadData.GetID() + "Walk Get In");
        ChangeMoving(true);
    }
    void SquadWalkGetOutFunc()
    {
        //LogModule.DebugLog("ID: " + mSquadData.GetID() + "Walk Get Out");
        mSearchAim = null;
        ChangeMoving(false);
    }
    void SquadWalkExcuteFunc()
    {
        //calculate  Attack time space
        if (mAttackSpaceTimer >= mSquadData.GetAttackSpaceTime())
            mAttackSpaceTimer = mSquadData.GetAttackSpaceTime();
        else
            mAttackSpaceTimer += Time.deltaTime; 

        //if arrive the target Pos change the state
        if (mAttackAim == null && mSearchAim == null)
        {
            //if arrive the target Pos change the state
            if (Vector3.Distance(transform.position, mRichAI.target.transform.position) < mMinDistance)
            {
                TryOperState(StateEnum.Walk, StateOper.Exit);
                return;
            }
        }
        //if there is No Enemy change to Idle
        if (mEnemySquadDic.Count == 0 && mIsMarching == false)
        {
            TryOperState(StateEnum.Walk, StateOper.Exit);
            return;
        }

        if (BattleMode.Client)
        {
            if (mSynTime < float.Epsilon)
            {
                KeepFormation();
                mSynTime = mSynTimeTimeSpace;
            }
            else
            {
                mSynTime -= Time.deltaTime;
            }
        }
    }
    /******************
    Atack State Function 
    *******************/
    void SquadAttackGetInFunc()
    {
        //LogModule.DebugLog("ID: " + mSquadData.GetID() + "Attack Get In");
        transform.LookAt(mAttackAim, Vector3.up);
        mAttackSpaceTimer = 0.0f;
    }
    void SquadAttackExcuteFunc()
    {
        if (mAttackAim != null)
            SquadAttackEffect();
        TryOperState(StateEnum.Attack, StateOper.Exit);
    }
    void SquadAttackGetOutFunc()
    {
        //LogModule.DebugLog("ID: " + mSquadData.GetID() + "Attack Get Out");
    }
    /******************
    Prepare State Function 
    *******************/
    void SquadPrepareGetInFunc()
    {
        //LogModule.DebugLog("ID: " + mSquadData.GetID() + "Prepare Get In");
        transform.LookAt(mAttackAim, Vector3.up);
    }
    void SquadPrepareExcuteFunc()
    {
        if (mAttackAim == null)
            TryOperState(StateEnum.Prepare, StateOper.Exit);
        if (mAttackSpaceTimer >= mSquadData.GetAttackSpaceTime())
        {
            mAttackSpaceTimer = mSquadData.GetAttackSpaceTime();
            TryOperState(StateEnum.Attack, StateOper.Enter);
        }
        mAttackSpaceTimer += Time.deltaTime;
    }
    void SquadPrepareGetOutFunc()
    {
        //LogModule.DebugLog("ID: " + mSquadData.GetID() + "Prepare Get Out");
    }

    /******************
    Die State Function 
    *******************/
    void SquadDieGetInFunc()
    {
        //LogModule.DebugLog("ID: " + mSquadData.GetID() + "Die Get In");
        ChangeMoving(false);
        //LogModule.DebugLog("Squad Die");
        gameObject.SetActive(false);
    }
    void SquadDieExcuteFunc()
    {

    }
    void SquadDieGetOutFunc()
    {
        //LogModule.DebugLog("ID: " + mSquadData.GetID() + "Die Get Out");
    }
    void SquadAttackEffect()
    {
        if (BattleMode.Client)
        {
            SquadController mAttackAimController = mAttackAim.GetComponent<SquadController>();
            mAttackAimController.SetSquadHP(mAttackAimController.GetSquadData().GetSquadHP() - GetSquadData().GetAttack(), transform);
        }
    }

    public void SetSquadHP(float tHP,Transform tAttackerSquad)
    {
        mSquadData.SetSquadHP(tHP);
        if (BattleMode.Client)
        {
            UnitDie(3, tAttackerSquad);
            //LogModule.DebugLog("剩余人数: " + mUnitTransformList.Count);
            FormationStrategy();
            if (tHP <= 0 && mStateMachine.GetCurrentState().StateType != StateEnum.Die)
            {
                SquadDeath();
            }
        }    
    }
    public void SquadDeath()
    {
        EventManager.GetInstance().SendEvent(EventId.SomeSquadDie, transform);
        TryOperState(StateEnum.Die, StateOper.Switch);
    }
    void TryOperState(StateEnum tAimStateEnum, StateOper tOper)
    {
        if (mStateMachine.GetCurrentState().StateType == StateEnum.Die)
        {
            return;
        }
        mStateMachine.OperState(tAimStateEnum, tOper);
    }
    public void CastSkill(Transform tAttakAim)
    {
        mSkill = GameObjectPool.GetInstance().SpawnGo((GameObject)mSquadData.SkillAsset).transform;
        mSkill.parent = null;
        mSkill.gameObject.SetActive(true);
        mSkill.position = transform.position;
        SetSkill(mSkill);
        if((mSkillController.GetSkillData().GetSkillType()&SkillType.Move)== SkillType.Move)
        {
            if (tAttakAim != null)
            {
                SquadController mAttackAimController = tAttakAim.GetComponent<SquadController>();
                mSkillController.MReceiver = mAttackAimController.transform;
                mSkillController.MScender = transform;
                mSkillController.SetSkillEffectData();
            }
        }
    }
    public void SetSkill(Transform tSkill)
    {
        mSkill = tSkill;
        mSkillController = mSkill.GetComponent<SkillEffect>();
    }
    void SomeSquadDie(object param)
    {
        Transform diedTrans = (Transform)param;
        int tmpID = diedTrans.GetComponent<SquadController>().GetSquadData().GetID();

        if (diedTrans != transform && mEnemySquadDic.ContainsKey(tmpID))
        {
            mEnemySquadDic.Remove(tmpID);
            if (mAttackAim == diedTrans)
                mAttackAim = null;
            if (mSearchAim == diedTrans)
                mSearchAim = null;
            StartCoroutine(IdleKeepFormation());
        }
    }
    // change the position of the Squad in the Embattling
    void ReSetPosition(object param)
    {
        BornPosInfor tBorPos = (BornPosInfor)param;
        if (tBorPos.mSquadID != mSquadData.GetID()|| tBorPos.mSquadID==-1)
            return;

//      UnitMarching(tBorPos.mBornPos);

        transform.position = tBorPos.mBornPos;
        mRichAI.target.position = tBorPos.mBornPos;

        FormationStrategy();

        for (int i = 0; i < mUnitTransformList.Count; i++)
        {
            mUnitTransformList[i].GetComponent<UnitController>().EmbattingSetPos(tBorPos.mBornPos);
        }
    }
    //
    public void ChangeMoving(bool RichAIEnable)
    {
        mRichAI.enabled = RichAIEnable;
    }
    // chose the UnitController has lowest HP 
    public void UnitDie(int count,Transform tAttackSquad)
    {
        if (mUnitTransformList.Count == 0)
            return;

        SquadController attackerSquadController = tAttackSquad.GetComponent<SquadController>();

        //Select the Unit Witch will die
        List<Transform> tDiedTrans = new List<Transform>();
        for (int j = 0; j < count; j++)
        {
            UnitController tAimUnitController = mUnitTransformList[0].GetComponent<UnitController>();
            for (int i = 1; i < mUnitTransformList.Count; i++)
            {
                if (mUnitTransformList[i].GetComponent<UnitController>().GetUnitData().GetUnitHP() < tAimUnitController.GetUnitData().GetUnitHP())
                    tAimUnitController = mUnitTransformList[i].GetComponent<UnitController>();
            }
            mUnitTransformList.Remove(tAimUnitController.transform);

            tDiedTrans.Add(tAimUnitController.transform);
            tAimUnitController.MWillDie =true;
        }
        //Select the dying Unit Witch will be Attack 
        for (int j = 0; j < tDiedTrans.Count; j++)
        {
            for (int i = 0; i < attackerSquadController.mUnitTransformList.Count; i++)
            {
                UnitController tAttackerUnit = attackerSquadController.mUnitTransformList[i].GetComponent<UnitController>();
                if (tAttackerUnit.GetAttackAim() == tDiedTrans[j])
                {
                    tAttackerUnit.MKillOrder = true;
                    tAttackerUnit.SetAttackSpaceTimer(tAttackerUnit.GetUnitData().GetAttackSpaceTime()+1);
                    //LogModule.DebugLog("Foce Attack AttackAim" + tDiedTrans[j].GetHashCode());
                    tDiedTrans.Remove(tDiedTrans[j]);
                    break;
                }
            }
        }
        List<Transform> tDiedRemovedTrans = new List<Transform>();
        //Select the dying Unit and the Attacker 
        for (int j = 0; j < tDiedTrans.Count; j++)
        {
            UnitController tAttackerUnit;
            for (int i = 0; i < attackerSquadController.mUnitTransformList.Count; i++)
            {
                tAttackerUnit = attackerSquadController.mUnitTransformList[i].GetComponent<UnitController>();
                if (tAttackerUnit.MKillOrder == false)
                //if (tAttackerUnit.MKillOrder == false&& (tAttackerUnit.GetUnitData().GetAttackType()& UnitAttackType.RemoteAttack) == UnitAttackType.RemoteAttack)
                {
                    tAttackerUnit.MKillOrder = true;
                    tAttackerUnit.SetAttackSpaceTimer(tAttackerUnit.GetUnitData().GetAttackSpaceTime() + 1);
                    tAttackerUnit.SetAttackAim(tDiedTrans[j]);
                    tDiedRemovedTrans.Add(tDiedTrans[j]);
                    //LogModule.DebugLog("Foce Attack ChangeAim" + tDiedTrans[j].GetHashCode());
                    break;
                }
            }
        }
        for (int i = 0; i < tDiedRemovedTrans.Count; i++)
        {
            tDiedTrans.Remove(tDiedRemovedTrans[i]);
        }
        tDiedRemovedTrans.Clear();

        if (tDiedTrans.Count > 0)
        {
            for (int i = 0; i < tDiedTrans.Count; i++)
            {
                tDiedTrans[i].GetComponent<UnitController>().TryOperState(StateEnum.Die, StateOper.Switch);
                EventManager.GetInstance().SendEvent(EventId.SomeOneDie, tDiedTrans[i]);
                LogModule.DebugLog("死的太快了！"+i);
            }
        }            
    }

    // Send command to all the Unit of this Squad let them to select a Enemy in the EnemyList to Attack 
    public void UnitAttackEnemy(int tEnemyId)
    {
        for (int i = 0; i < mUnitTransformList.Count; i++)
        {
            mUnitTransformList[i].GetComponent<UnitController>().FindAndAttackEnemy(mEnemySquadDic[tEnemyId].transform);
        }
    }
    // Send command to all the Unit of this Squad let them to refresh EnemyList
    public void RefreshUnitEnemyList(int tEnemyId)
    {
        for (int i = 0; i < mUnitTransformList.Count; i++)
        {
            mUnitTransformList[i].GetComponent<UnitController>().RefreshEnemyList(mEnemySquadDic[tEnemyId].transform);
        }
    }
    // Send command to all the Unit of this Squad let the to Move to the AimPos, Keeping the relative position
    public void UnitMarching(Vector3 tAimPos)
    {
        //Vector3 tDirection = (tAimPos - transform.position).normalized;
        //float tAngle = Mathf.CeilToInt(Mathf.Acos(Vector3.Dot(Vector3.left, tDirection)) * Mathf.Rad2Deg);

        for (int i = 0; i < mUnitTransformList.Count; i++)
        {
            mUnitTransformList[i].GetComponent<UnitController>().UnitMarching(tAimPos, 0f);
        }
        mRichAI.target = mOriginalAim;
        mRichAI.target.position = tAimPos;
        mRichAI.UpdatePath();
        if (mStateMachine.GetCurrentState().StateType == StateEnum.Idle)
            TryOperState(StateEnum.Walk, StateOper.Enter);
        else
            return;
        mIsMarching = true;
    }
    public void SquadAttackEnemy(SquadController tEnemySquad)
    {
        int tTargetID=0;
        if (mAttackAim != tEnemySquad.transform|| mAttackAim==null)
        {
            mAttackAim = tEnemySquad.transform;
            tTargetID = mAttackAim.GetComponent<SquadController>().GetSquadData().GetID();
            RefreshUnitEnemyList(tTargetID);
        }
        UnitAttackEnemy(tTargetID);
    }
    public void SquadAttackPrepare(SquadController tEnemySquad)
    {
        int tTargetID = 0;
        if (mAttackAim != tEnemySquad.transform || mAttackAim == null)
        {
            mAttackAim = tEnemySquad.transform;
            tTargetID = mAttackAim.GetComponent<SquadController>().GetSquadData().GetID();
            RefreshUnitEnemyList(tTargetID);
        }
        UnitAttackEnemy(tTargetID);
        ChangeTarget(mAttackAim);
        TryOperState(StateEnum.Prepare, StateOper.Enter);
    }


    public void CorretSquadInfor(GC_OBJPOS tObjPosList ,SquadController tTargetController=null)
    {
        switch (mStateMachine.GetCurrentState().StateType)
        {
            case StateEnum.Attack:
                break;
            case StateEnum.Idle:
                //
                if((SquadState)tObjPosList.ObjState == SquadState.AI_COMBAT)
                {                
                    if(tTargetController!=null)    
                        SquadAttackPrepare(tTargetController);
                }
                //
                if ((SquadState)tObjPosList.ObjState == SquadState.AI_MARCH)
                {
                    UnitMarching(BattleFunction.SetPosition(tObjPosList.PosX, tObjPosList.PosZ));
                }
                break;
            case StateEnum.Prepare:
                break;
            case StateEnum.Walk:
                UnitMarching(BattleFunction.SetPosition(tObjPosList.PosX, tObjPosList.PosZ));
                break;
        }
        //transform.position=SetPosition(tObjPosList.PosX, tObjPosList.PosZ);
        //for (int i = 0; i < mUnitTransformList.Count; i++)
        //{
        //    mUnitTransformList[i].GetComponent<UnitController>().UnitMarching(transform.position, 0f);
        //}
    }
    public void UnitMarchAttacking(float space, Vector3 tAimPos)
    {
        TakeCountOthers();
        UnitMarching(tAimPos);
    }
    // Take cout of other Unit in the Squad and adjust the Attack distance
    public void TakeCountOthers()
    {
        //float space = DeltaH * Random.Range(0.5f, 1f);
        float space = DeltaH;
        for (int i = 0; i < mUnitTransformList.Count; i++)
        {
            UnitData tUnitData = mUnitTransformList[i].GetComponent<UnitController>().GetUnitData();

            if ((tUnitData.GetAttackType()&UnitAttackType.RemoteAttack) == UnitAttackType.RemoteAttack)
            {
                switch (i / mSquadData.GetUnitCountOfRow())
                {
                    case 0:
                        tUnitData.SetAdjustAttackRange(-2 * space);
                        break;
                    case 1:
                        tUnitData.SetAdjustAttackRange(-space);
                        break;
                    case 2:
                        tUnitData.SetAdjustAttackRange(0);
                        break;
                }
            }
        }
    }
    public void RefreshUnitEnemyList(int tEnemyId,float space )
    {
        TakeCountOthers();
        RefreshUnitEnemyList(tEnemyId);
    }
    //Keep the formation
    public void KeepFormation()
    {
        for (int i = 0; i < mUnitTransformList.Count; i++)
        {
            mUnitTransformList[i].GetComponent<UnitController>().UnitMarching(transform.position);
        }
    }
    IEnumerator IdleKeepFormation()
    {
        foreach (var tUnit in mUnitTransformList)
        {
            yield return tUnit.GetComponent<UnitController>().CheckState();
        }
        //LogModule.DebugLog("All Done!");
        FormationStrategy();
        KeepFormation();
    }


public void FormationStrategy()
    {
        UnitR = TableManager.GetUnitTemplateByID(mSquadData.GetUnitTemplateID())[0].UnitRadius;
        UnitSpaceL = TableManager.GetUnitTemplateByID(mSquadData.GetUnitTemplateID())[0].UnitSpaceL;
        UnitSpaceH = TableManager.GetUnitTemplateByID(mSquadData.GetUnitTemplateID())[0].UnitSpaceH;

        DeltaL = 2 * (UnitR + UnitSpaceL);
        DeltaH = 2 * (UnitR + UnitSpaceH);

        SquadL = TableManager.GetTeamConfigByID(mSquadData.GetTeamFormationID())[0].SquadL;
        SquadH = TableManager.GetTeamConfigByID(mSquadData.GetTeamFormationID())[0].SquadH;

        MaxNumberInRow = Mathf.Floor(SquadL / DeltaL);
        MaxNumberInColumn = Mathf.Floor(SquadH / SquadH);

        int tCurrentUnitCounts = mUnitTransformList.Count;
        float CurrentNumberInColumn = Mathf.Ceil(tCurrentUnitCounts / MaxNumberInRow);
        float CurrentNumberInRow = CurrentNumberInColumn == 1 ? tCurrentUnitCounts : MaxNumberInRow;

        bool IsOddX = ((int)CurrentNumberInRow % 2 )== 1;
        int CenterIndexX = Mathf.FloorToInt(CurrentNumberInRow / 2);
        bool IsOddY = ((int)CurrentNumberInColumn % 2) == 1;
        int CenterIndexZ = Mathf.FloorToInt(CurrentNumberInColumn/2);


        for (int i = 0; i < mUnitTransformList.Count; i++)
        {
            int tRowNumber = Mathf.FloorToInt(i / CurrentNumberInRow);
            int tColumnNumber = Mathf.FloorToInt(i % CurrentNumberInRow);

            mSquadData.SetUnitCountOfRow(tRowNumber);

            float tmpX;
            float tmpZ;
            if (!IsOddX)
            {
                tmpZ = (tColumnNumber - CenterIndexX) * DeltaL + DeltaL / 2 ;
            }else
            {
                tmpZ = (tColumnNumber - CenterIndexX) * DeltaL ;
            }

            if(!IsOddY)
            {
                tmpX =(tRowNumber - CenterIndexZ) * DeltaH + DeltaH / 2 ;
            }
            else
            {
                tmpX =(tRowNumber - CenterIndexZ) * DeltaH ;
            }
            mUnitTransformList[i].GetComponent<UnitController>().RelativePosition = new Vector3(tmpX,0f, tmpZ);
        }
    }
}
