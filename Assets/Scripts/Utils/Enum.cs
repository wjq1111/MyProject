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

// 属性卡使用目标类型
public enum AttributeCardUseTargetType
{
    MySelf,
    Other,
    // all = myself + other
    All,
}

// 卡片能力
public enum Ability
{
    Invalid,
    SummonMonster,
    HurtMonster
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