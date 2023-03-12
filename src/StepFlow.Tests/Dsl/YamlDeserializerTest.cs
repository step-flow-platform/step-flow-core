using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StepFlow.Dsl;
using StepFlow.Dsl.Model;

namespace StepFlow.Tests.Dsl;

[TestClass]
public class YamlDeserializerTest
{
    [TestMethod]
    public void DeserializeWorkflowDefinitionWithoutSteps()
    {
        string yaml = File.ReadAllText("Dsl/YamlAssets/workflow-without-steps.yaml");

        WorkflowDefinitionModel? model = Deserializers.Yaml(yaml);

        Assert.IsNotNull(model);
        Assert.AreEqual("DataType", model.Data);
        Assert.IsNull(model.Steps);
    }
}