export interface RuleFlowNode {
    ruleId: string | null,
    nodeType: NodeType,
    nextNodeId: string | null,
    conditionExpression: string,
    index: number
}

export enum NodeType {
    Rule,
    Condition,
    Parallel
}

export interface Rule {
    id: number,
    ruleName: string,
    script: string,
    scriptLanguageType: ScriptLanguageType,
    description: string | null,
    scriptNeedParameters: boolean
}

export interface RuleAddDto {
    ruleName: string,
    script: string,
    scriptLanguageType: ScriptLanguageType,
    description: string | null,
    scriptNeedParameters: boolean
}

export interface RuleScriptTestDto {
    scriptLanguageType: ScriptLanguageType,
    scriptCode: string,
    inputs: [] | null
}

export enum ScriptLanguageType {
    Javascript = 0,
    Python,
    Lua,
    CSharp,
}