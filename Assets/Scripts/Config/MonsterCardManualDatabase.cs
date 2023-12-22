using System.Collections.Generic;	
public class MonsterCardManualDatabase
{
	public List<MonsterCardManual> info;
}

[System.Serializable]
public class MonsterCardManual : Config
{
	/// 攻击力 
	public int Attack;
	/// 防御力 
	public int Defense;
	/// 血量 
	public int Hp;
	/// 行动次数 
	public int ActionNum;
	/// 描述 
	public string Description;

	/// 打印函数 
	public override string ToString()
	{
		string str = "";
		str += Attack.ToString();
		str += Defense.ToString();
		str += Hp.ToString();
		str += ActionNum.ToString();
		str += Description.ToString();
		return str;
	}
}
