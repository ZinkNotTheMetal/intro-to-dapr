using Dapr.Client;
using Dapr.Workflow;
using Invoice.Processor.Workflow.Models;
using Microsoft.Extensions.Logging;

namespace Invoice.Processor.Workflow.Activities;
public class PersistInvoiceRequestActivity : WorkflowActivity<ProcessInvoiceRequest, bool>
{
    private readonly ILogger<IntakeInvoiceActivity> _logger;
    private readonly DaprClient _daprClient;

    public PersistInvoiceRequestActivity(ILogger<IntakeInvoiceActivity> logger, DaprClient daprClient)
    {
        _logger = logger;
        _daprClient = daprClient;
    }

    public override async Task<bool> RunAsync(WorkflowActivityContext context, ProcessInvoiceRequest request)
    {
        await _daprClient.SaveStateAsync("invoices", context.InstanceId, request);

        _logger.LogInformation("");
        throw new NotImplementedException();
    }
}