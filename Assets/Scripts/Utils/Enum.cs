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


// ��Ƭ����
public enum Ability
{
    Invalid,
    // �ٻ�����
    SummonMonster,
    // �޸Ĺ�������
    ModifyMonsterAttribute
}

public enum ResourceType
{
    Invalid,
    // �������İ�ť
    PlayerCardButton,
    // ���Ϲ������İ�ť
    MonsterButton,
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
    OnClickPlayerCardButton,
    OnEnterPlayerCardButton,
    OnClickMonsterButton,

    // For test
    OnClickPrintPlayerButton,
    OnClickPrintAIPlayerButton,
    FlushDebugStatus,
}