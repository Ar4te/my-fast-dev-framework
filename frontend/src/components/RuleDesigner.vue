<template>
    <div style="width: 100%;">
        <div>
            <h2>Rule Designer</h2>
            <label for="ruleNameText">Rule Name:</label>
            <input type="text" id="ruleNameText" v-model="ruleName" />
            <label>Script Language Type:</label>
            <select v-model="scriptLanguageType">
                <option :value="ScriptLanguageType.Javascript" selected>Javascript</option>
                <option :value="ScriptLanguageType.CSharp">CSharp</option>
                <option value="tbd" disabled>Coming soon...</option>
            </select>

            <label style="margin-left: 30px;">Script Need Parameters:</label>
            <input type="radio" v-model="scriptNeedParameters" value="true" />true
            <input type="radio" v-model="scriptNeedParameters" value="false" checked />false
        </div>
        <div>
            <label>Rule script Code:</label>
            <div style="display: flex; width: 100%;">
                <div id="editor" style="height: 400px; width: 100%;"></div>
                <textarea id="result" style="height: 400px; width: 50%;" v-model="scriptResult" readonly
                    disabled></textarea>
            </div>
            <div style="width: 100%;">
                <label>Rule Description:</label>
                <textarea name="ruleDescription" id="ruleDescription" v-model="description"></textarea>
            </div>
            <div style="align-items:center;">
                <button @click="executeScriptCode" id="executeScriptCode"
                    :disabled="scriptCode == '' || scriptCode.length <= 0" style="margin: 50px;">Execute Script
                    Code</button>

                <button @click="saveFlow">Save Rule</button>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { ref, onMounted, computed } from "vue";
import ace from "ace-builds";
import { RuleAddDto, RuleScriptTestDto, ScriptLanguageType } from "./RuleFlowNode";
import axios from "axios";

const ruleName = ref("")
const scriptLanguageType = ref(ScriptLanguageType.Javascript)
const scriptNeedParameters = ref(false)
const scriptCode = ref("")
const description = ref("")
const scriptResult = ref("")
const editor = ref<ace.Ace.Editor | null>(null);

scriptCode.value = computed(() => {
    var val = editor.value?.getValue().replace("// 在此处编写您的脚本代码", "");
    console.log(val);

    return val ?? "";
}).value;

onMounted(() => {
    editor.value = ace.edit('editor');
    editor.value.setTheme('ace/theme/monokai');
    editor.value.session.setMode('ace/mode/javascript'); // 你可以设置不同语言的语法模式
    editor.value.setValue(`// 在此处编写您的脚本代码\r\n`);
    editor.value.session.on('change', () => {
        scriptCode.value = editor.value?.getValue().replace("// 在此处编写您的脚本代码", "") || "";
    })
});

const saveFlow = async () => {
    const ruleAddDto: RuleAddDto = {
        ruleName: ruleName.value,
        script: scriptCode.value,
        scriptLanguageType: scriptLanguageType.value,
        description: description.value,
        scriptNeedParameters: scriptNeedParameters.value
    };

    console.log(ruleAddDto);

    try {
        const response = await axios.post("api/rule-flow/save", ruleAddDto);
        alert(response.data.message);
    } catch (error) {
        console.error("Error saving rule flow:", error);
    }
}

const executeScriptCode = async () => {
    try {
        var data: RuleScriptTestDto = {
            scriptLanguageType: scriptLanguageType.value,
            scriptCode: scriptCode.value,
            inputs: null
        }
        console.log(data);

        const response = await axios.post("api/Rule/TestScriptCode", data);
        scriptResult.value = response.data;
        // console.log(response);
        // alert(`Script Cpde:${data.scriptCode}\r\nResult:\r\n${response.data}`);
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