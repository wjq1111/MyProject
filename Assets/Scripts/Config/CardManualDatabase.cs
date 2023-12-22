using System.Collections.Generic;	
public class CardManualDatabase
{
	public List<CardManual> info;
}

[System.Serializable]
public class CardManual : Config
{
	/// 卡牌名称 
	public string CardName;
	/// 稀有度 
	public int Rarity;
	/// 卡牌类型 
	public int UseCardType;
	/// 属性卡能力 
	public string Ability;
	/// 描述 
	public string Description;

	/// 打印函数 
	public override string ToString()
	{
		string str = "";
		str += CardName.ToString();
		str += Rarity.ToString();
		str += UseCardType.ToString();
		str += Ability.ToString();
		str += Description.ToString();
		return str;
	}
}
