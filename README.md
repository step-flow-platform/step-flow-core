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
.If(data => data.A > 5, _ => _
    .Step<Step1>())
.If(data => data.B > 10, _ => _
    .Step<Step1>()
    .If(data => data.A >= data.B, __ => __
        .Step<Step1>()
        .Step<Step2>()
        .Step<Step3>()
        .Step<Step4>()))
.Step<Step2>();
```
```mermaid
flowchart TB
node1[Step1]
node2{"(data.A > 5)"}
node3[Step1]
node4{"(data.B > 10)"}
node5[Step1]
node6{"(data.A >= data.B)"}
node7[Step1]
node8[Step2]
node9[Step3]
node10[Step4]
node11[Step2]

node1 --> node2
node2 -->|true| node3
node3 --> node4
node2 -->|false| node4
node4 -->|true| node5
node5 --> node6
node6 -->|true| node7
node7 --> node8
node8 --> node9
node9 --> node10
node10 --> node11
node4 -->|false| node11
node6 -->|false| node11
```
