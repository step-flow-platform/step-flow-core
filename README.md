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
node1[Step1]
label1{"(data.A > 5)"}
node2[Step2]
node3{"(data.B > 10)"}
node4[Step3]
node5{"(data.A >= data.B)"}
node6[Step1]
node7[Step2]
node9[Step4]
endNode((End))

startNode --> node1
node1 --> label1
label1 -->|true| node2
label1 -->|false| node3
node2 --> node3
node3 -->|true| node4
node3 -->|false| node9
node4 --> node5
node5 -->|true| node6
node5 -->|false| node9
node6 --> node7
node7 --> label1
node9 --> endNode
```

