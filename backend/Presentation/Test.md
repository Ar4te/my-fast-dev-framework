```MySql
INSERT INTO `RuleEngine`.`Rule` (`Id`, `RuleName`, `Script`, `ScriptLanguageType`, `Description`, `ScriptNeedParameters`) VALUES (1, 'Test', 'return 1+2', 0, 'test', 0);
INSERT INTO `RuleEngine`.`Rule` (`Id`, `RuleName`, `Script`, `ScriptLanguageType`, `Description`, `ScriptNeedParameters`) VALUES (2, 'Test2', 'return 1==2', 0, 'test2', 0);
INSERT INTO `RuleEngine`.`Rule` (`Id`, `RuleName`, `Script`, `ScriptLanguageType`, `Description`, `ScriptNeedParameters`) VALUES (3, 'TestCSharpScript', 'return 1 == 1;', 3, 'TestCSharpScript', 0);


INSERT INTO `RuleEngine`.`RuleFlow` (`Id`, `FlowName`, `Description`) VALUES (2, 'TestFlow', 'TestFlow');


INSERT INTO `RuleEngine`.`RuleFlowNode` (`Id`, `FlowId`, `RuleId`, `NodeType`, `NextNodeId`, `Index`, `UseResultAsNextParameter`) VALUES (3, 2, 3, 1, 4, 1, 0);
INSERT INTO `RuleEngine`.`RuleFlowNode` (`Id`, `FlowId`, `RuleId`, `NodeType`, `NextNodeId`, `Index`, `UseResultAsNextParameter`) VALUES (4, 2, 2, 1, 14, 2, 0);
INSERT INTO `RuleEngine`.`RuleFlowNode` (`Id`, `FlowId`, `RuleId`, `NodeType`, `NextNodeId`, `Index`, `UseResultAsNextParameter`) VALUES (7, 2, 3, 0, NULL, 4, 0);
INSERT INTO `RuleEngine`.`RuleFlowNode` (`Id`, `FlowId`, `RuleId`, `NodeType`, `NextNodeId`, `Index`, `UseResultAsNextParameter`) VALUES (14, 2, 1, 0, 7, 3, 0);























```