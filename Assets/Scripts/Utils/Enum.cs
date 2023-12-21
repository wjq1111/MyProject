public enum GameStatus
{
    Invalid,
    OnGoing,
    Win,
    Lose
}

// ��Ӫid
public enum CampId
{
    Invalid,
    Myself,
    AI
}

// ���Կ�ʹ��Ŀ������
public enum AttributeCardUseTargetType
{
    MySelf,
    Other,
    // all = myself + other
    All,
}

// ��Ƭ����
public enum Ability
{
    Invalid,
    SummonMonster,
    HurtMonster
}

// ͼ��㼶
public enum FormLayer
{
    InvalidForm = -1,
    LobbyForm,
    StartGameForm,
}

// UI �¼�id
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