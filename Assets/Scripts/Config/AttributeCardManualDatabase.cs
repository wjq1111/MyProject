using System.Collections.Generic;	
public class AttributeCardManualDatabase
{
	public List<AttributeCardManual> info;
}

[System.Serializable]
public class AttributeCardManual : Config
{
	/// 攻击力 
	public int Attack;
	/// 防御力 
	public int Defense;
	/// 血量 
	public int Hp;
	/// 行动次数 
	public int ActionNum;
	/// 目标数量 
	public int TargetCount;
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
		str += TargetCount.ToString();
		str += Description.ToString();
		return str;
	}
}
