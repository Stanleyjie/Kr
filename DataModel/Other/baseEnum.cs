namespace DataModel.Other
{
    /// <summary>
    /// 订单状态
    /// </summary>
    public enum orderStatusEnum
    {
        待处理 = 0,
        已处理 = 1,
        待评价 = 2,
        已评价 = 3,
    }

    //关系
    public enum relationTypeEnum
    {
        自己 = 0,
        父母 = 1,
        配偶 = 2,
        子女 = 3,
        其他 = 4
    }

    //消息状态（1：已读 2：未读）
    public enum msgState
    {
        未读 = 1,
        已读 = 2
    }

    public enum HospitalLevel
    {
        公立三甲 = 0,
        专业机构 = 1,
        公立医院 = 2,
        民营医院 = 3
    }

    public enum MemberSex
    {
        男 = 0,
        女 = 1
    }
}