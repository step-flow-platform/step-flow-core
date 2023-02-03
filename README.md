# StepFlow Core
Workflow engine

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