# StepFlow Core
Workflow engine library

## Architecture
```mermaid
flowchart TB
    Json[[JSON]] -->|Deserialize| Model
    Yaml[[YAML]] -->|Deserialize| Model
    Code[[C# code]] -->WorkflowBuilder
    WorkflowBuilder --> WorkflowDefinition
    Model --> WorkflowDefinition
    WorkflowDefinition --> WorkflowExecutor
```

## Mermaid flowchart generation
```csharp
.Step<Step1>()
.If("label1", data => data.A > 5, _ => _
    .Step<Step2>())
.If(data => data.B > 10, _ => _
    .Step<Step3>()
    .If(data => data.A >= data.B, __ => __
        .Step<Step1>()
        .Step<Step2>()
        .GoTo("label1")))
.Step<Step4>();
```
```mermaid
flowchart TB
startNode((Start))
node2[Step1]
label1{"(data.A > 5)"}
node4[Step2]
node5{"(data.B > 10)"}
node6[Step3]
node7{"(data.A >= data.B)"}
node8[Step1]
node9[Step2]
node11[Step4]
endNode((End))

startNode --> node2
node2 --> label1
label1 -->|true| node4
label1 -->|false| node5
node4 --> node5
node5 -->|true| node6
node5 -->|false| node11
node6 --> node7
node7 -->|true| node8
node7 -->|false| node11
node8 --> node9
node9 --> label1
node11 --> endNode
```

