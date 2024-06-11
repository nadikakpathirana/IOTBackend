using Xunit;
using IOTBackend.Application.Services;

namespace IOTBackend.Tests
{
    public class LogicProcessorServiceTests
    {
        private readonly LogicProcessorService _service;

        public LogicProcessorServiceTests()
        {
            _service = new LogicProcessorService();
        }

        // [Fact]
        // public async Task Process_SingleCondition_ReturnsTrue()
        // {
        //     var sensorValue = 10;
        //     var condition = "x > 5";
        //
        //     var result = await _service.Process(sensorValue, condition);
        //
        //     Assert.True(result);
        // }
        //
        // [Fact]
        // public async Task Process_SingleCondition_ReturnsFalse()
        // {
        //     var sensorValue = 3;
        //     var condition = "x > 5";
        //
        //     var result = await _service.Process(sensorValue, condition);
        //
        //     Assert.False(result);
        // }
        //
        // [Fact]
        // public async Task Process_ComplexCondition_ReturnsTrue()
        // {
        //     var sensorValue = 10;
        //     var condition = "x > 5 AND x < 15 OR x == 10";
        //
        //     var result = await _service.Process(sensorValue, condition);
        //
        //     Assert.True(result);
        // }
        //
        // [Fact]
        // public async Task Process_ComplexCondition_ReturnsFalse()
        // {
        //     var sensorValue = 10;
        //     var condition = "x > 15 AND x < 20 OR x == 5";
        //
        //     var result = await _service.Process(sensorValue, condition);
        //
        //     Assert.False(result);
        // }
        //
        // [Fact]
        // public async Task Process_NestedCondition_ReturnsTrue()
        // {
        //     var sensorValue = 10;
        //     var condition = "((x > 5 AND x < 15) OR x == 10)";
        //
        //     var result = await _service.Process(sensorValue, condition);
        //
        //     Assert.True(result);
        // }
        //
        // [Fact]
        // public async Task Process_NestedCondition_ReturnsFalse()
        // {
        //     var sensorValue = 10;
        //     var condition = "((x > 15 AND x < 20) OR x == 5)";
        //
        //     var result = await _service.Process(sensorValue, condition);
        //
        //     Assert.False(result);
        // }
        //
        // [Fact]
        // public async Task Process_NotCondition_ReturnsTrue()
        // {
        //     var sensorValue = 10;
        //     var condition = "!(x < 5)";
        //
        //     var result = await _service.Process(sensorValue, condition);
        //
        //     Assert.True(result);
        // }
        //
        // [Fact]
        // public async Task Process_NotCondition_ReturnsFalse()
        // {
        //     var sensorValue = 10;
        //     var condition = "!(x > 5)";
        //
        //     var result = await _service.Process(sensorValue, condition);
        //
        //     Assert.False(result);
        // }
        //
        // [Fact]
        // public async Task Process_InvalidCondition_ReturnsFalse()
        // {
        //     var sensorValue = 10;
        //     var condition = "x >";
        //
        //     var result = await _service.Process(sensorValue, condition);
        //
        //     Assert.False(result);
        // }
        //
        // [Fact]
        // public async Task Process_InvalidVariableInCondition_ReturnsFalse()
        // {
        //     var sensorValue = 10;
        //     var condition = "invalidVariable > 5";
        //
        //     var result = await _service.Process(sensorValue, condition);
        //
        //     Assert.False(result);
        // }
        //
        // [Fact]
        // public async Task Process_EmptyCondition_ReturnsFalse()
        // {
        //     var sensorValue = 10;
        //     var condition = "";
        //
        //     var result = await _service.Process(sensorValue, condition);
        //
        //     Assert.False(result);
        // }
        //
        // [Fact]
        // public async Task Process_OnlyLogicalOperators_ReturnsFalse()
        // {
        //     var sensorValue = 10;
        //     var condition = "AND OR";
        //
        //     var result = await _service.Process(sensorValue, condition);
        //
        //     Assert.False(result);
        // }
        //
        // [Fact]
        // public async Task Process_MultipleNestedConditions_ReturnsTrue()
        // {
        //     var sensorValue = 10;
        //     var condition = "((x > 5 AND (x < 15 OR x == 10)) AND !(x < 5))";
        //
        //     var result = await _service.Process(sensorValue, condition);
        //
        //     Assert.True(result);
        // }
        //
        // [Fact]
        // public async Task Process_MultipleNestedConditions_ReturnsFalse()
        // {
        //     var sensorValue = 10;
        //     var condition = "((x > 15 AND (x < 20 OR x == 5)) AND !(x < 5))";
        //
        //     var result = await _service.Process(sensorValue, condition);
        //
        //     Assert.False(result);
        // }
        //
        // [Fact]
        // public async Task Process_SensorValueEqualCondition_ReturnsTrue()
        // {
        //     var sensorValue = 10;
        //     var condition = "x == 10";
        //
        //     var result = await _service.Process(sensorValue, condition);
        //
        //     Assert.True(result);
        // }
        //
        // [Fact]
        // public async Task Process_SensorValueNotEqualCondition_ReturnsTrue()
        // {
        //     var sensorValue = 10;
        //     var condition = "x != 5";
        //
        //     var result = await _service.Process(sensorValue, condition);
        //
        //     Assert.True(result);
        // }
        //
        // [Fact]
        // public async Task Process_SensorValueGreaterOrEqualCondition_ReturnsTrue()
        // {
        //     var sensorValue = 10;
        //     var condition = "x >= 10";
        //
        //     var result = await _service.Process(sensorValue, condition);
        //
        //     Assert.True(result);
        // }
        //
        // [Fact]
        // public async Task Process_SensorValueLessOrEqualCondition_ReturnsTrue()
        // {
        //     var sensorValue = 10;
        //     var condition = "x <= 10";
        //
        //     var result = await _service.Process(sensorValue, condition);
        //
        //     Assert.True(result);
        // }
        //
        // [Fact]
        // public async Task Process_SensorValueGreaterOrEqualCondition_ReturnsFalse()
        // {
        //     var sensorValue = 10;
        //     var condition = "x >= 15";
        //
        //     var result = await _service.Process(sensorValue, condition);
        //
        //     Assert.False(result);
        // }
        //
        // [Fact]
        // public async Task Process_SensorValueLessOrEqualCondition_ReturnsFalse()
        // {
        //     var sensorValue = 10;
        //     var condition = "x <= 5";
        //
        //     var result = await _service.Process(sensorValue, condition);
        //
        //     Assert.False(result);
        // }
        //
        // [Fact]
        // public async Task Process_InvalidSyntax_ReturnsFalse()
        // {
        //     var sensorValue = 10;
        //     var condition = "x >> 5";
        //
        //     var result = await _service.Process(sensorValue, condition);
        //
        //     Assert.False(result);
        // }
        //
        // [Fact]
        // public async Task Process_LogicalOperatorWithoutCondition_ReturnsFalse()
        // {
        //     var sensorValue = 10;
        //     var condition = "x > 5 AND";
        //
        //     var result = await _service.Process(sensorValue, condition);
        //
        //     Assert.False(result);
        // }
        //
        // [Fact]
        // public async Task Process_ComplexNestedConditions_ReturnsTrue()
        // {
        //     var sensorValue = 20;
        //     var condition = "((x > 10 AND (x < 25 OR x == 20)) AND (!(x < 15) OR x != 5))";
        //
        //     var result = await _service.Process(sensorValue, condition);
        //
        //     Assert.True(result);
        // }
    }
}
