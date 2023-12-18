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
    AI,
    // all = myself + ai
    All,
}

// ���Կ�ʹ��Ŀ������
public enum AttributeCardUseTargetType
{
    MySelf,
    Other,
    // all = myself + other
    All,
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
}