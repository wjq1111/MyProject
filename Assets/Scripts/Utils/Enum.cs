public enum GameStatus
{
    Invalid,
    OnGoing,
    Win,
    Lose
}

// 阵营id
public enum CampId
{
    Invalid,
    Myself,
    AI
}


// 卡片能力
public enum Ability
{
    Invalid,
    // 召唤怪物
    SummonMonster,
    // 修改怪物属性
    ModifyMonsterAttribute
}

public enum ResourceType
{
    Invalid,
    // 手牌区的按钮
    PlayerCardButton,
    // 场上怪物区的按钮
    MonsterButton,
}

// 图层层级
public enum FormLayer
{
    InvalidForm = -1,
    LobbyForm,
    StartGameForm,
}

// UI 事件id
public enum EventId
{
    Invalid,
    StartGame,
    LoadGame,

    // UI event callback
    OnClickStartGameButton,
    OnClickEndRoundButton,
    OnClickExitGameButton,
    OnClickPlayerCardButton,
    OnEnterPlayerCardButton,
    OnClickMonsterButton,

    // For test
    OnClickPrintPlayerButton,
    OnClickPrintAIPlayerButton,
    FlushDebugStatus,
}