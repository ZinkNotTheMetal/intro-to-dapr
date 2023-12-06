using Dapr.Client;
using Dapr.Workflow;
using Invoice.Processor.Workflow.Models;
using Microsoft.Extensions.Logging;

namespace Invoice.Processor.Workflow.Activities;

public class IntakeInvoiceActivity : WorkflowActivity<ProcessInvoiceRequest, bool>
{
    private readonly ILogger<IntakeInvoiceActivity> _logger;
    private readonly DaprClient _daprClient;

    public IntakeInvoiceActivity(ILogger<IntakeInvoiceActivity> logger, DaprClient daprClient)
    {
        _logger = logger;
        _daprClient = daprClient;
    }

    public override async Task<bool> RunAsync(WorkflowActivityContext context, ProcessInvoiceRequest input)
    {
        _logger.LogDebug("Request is processing...");

        await Task.Delay(TimeSpan.FromSeconds(3));
        return true;
    }
}
