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
    SummonMonster,
    ModifyMonsterAttribute
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

    // For test
    OnClickPrintPlayerButton,
    OnClickPrintAIPlayerButton,
    FlushDebugStatus,
}