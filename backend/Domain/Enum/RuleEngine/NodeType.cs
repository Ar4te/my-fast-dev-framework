namespace Domain.Enum.RuleEngine;

/// <summary>
/// 节点类型 <see cref="NodeType"/><br/>
/// 可选项：<br/>
/// <see cref="Rule"/> 执行当前规则<br/>
/// <see cref="Condition"/> 条件判断<br/>
/// <see cref="Parallel"/>并行执行多个规则<br/>
/// </summary>
public enum NodeType
{
    /// <summary>
    /// 执行规则
    /// </summary>
    Rule,
    /// <summary>
    /// 条件判断
    /// </summary>
    Condition,
    /// <summary>
    /// 并行执行多个规则
    /// </summary>
    Parallel
}