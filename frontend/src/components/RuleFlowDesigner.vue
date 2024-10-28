<template>
    <div>
        <h2>Rule Flow Designer</h2>
        <label for="flowNameText">Flow Name:</label>
        <input type="text" id="flowNameText" v-model="flowName" />
    </div>
    <div>
        <label>Flow Description:</label>
        <textarea name="flow_description" id="flow_description" v-model="description"></textarea>
    </div>

    <div>
        <h3>Rules</h3>
        <div v-for="(node, index) in ruleFlowNodes" :key="index" class="rule-node">
            <label>Rule ID:</label>
            <input v-model="node.ruleId" type="number" />
            <label>Node Type:</label>
            <select v-model="node.nodeType">
                <option value="rule">Rule</option>
                <option value="condition">Condition</option>
                <option value="parallel">Parallel</option>
            </select>
            <label>Next Node ID:</label>
            <input type="number" v-model="node.nextNodeId" />
            <label v-if="node.nodeType === NodeType.Condition">Condition Expression:</label>
            <input v-if="node.nodeType === NodeType.Condition" v-model="node.conditionExpression" type="text" />
            <!-- <label v-else="node.nodeType === NodeType.Parallel">Parallel Expression:</label>
            <input v-else="node.nodeType === NodeType.Parallel" v-model="node.conditionExpression" type="text" /> -->
            <label for="nodeIndexInput">Node Index:</label>
            <input type="number" v-model="node.index" />
        </div>
        <button @click="addNode">Add Rule Node</button>
    </div>

    <button @click="saveFlow">Save Rule Flow</button>
</template>

<script setup lang="ts">
import { ref, reactive } from "vue";
import { NodeType, RuleFlowNode } from "./RuleFlowNode";
import axios from "axios";

const flowName = ref("");
const description = ref("");
const ruleFlowNodes = reactive<RuleFlowNode[]>([]);

const addNode = () => {
    ruleFlowNodes.push({
        ruleId: null,
        nodeType: NodeType.Rule,
        nextNodeId: null,
        conditionExpression: '',
        index: 1
    });
};

const saveFlow = async () => {
    const ruleFlow = {
        flowName: flowName,
        description: description,
        nodes: ruleFlowNodes
    };

    try {
        const response = await axios.post("api/rule-flow/save", ruleFlow);
        alert(response.data.message);
    } catch (error) {
        console.error("Error saving rule flow:", error);
    }
}
</script>

<style scoped>
.rule-node {
    margin-bottom: 15px;
}
</style>