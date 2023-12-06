using Dapr.Workflow;
using Invoice.Processor.Workflow.Activities;
using Invoice.Processor.Workflow.Models;

namespace Invoice.Processor.Workflow;


/// <summary>
/// dapr workflow to 'process' an invoice
///   Step 1: Store to state store
///   Step 2: OCR the invoice
///   Step 3: Store invoice details (amount, company, date)
///   Step 4: Mark ready for payment
/// </summary>
public class ProcessInvoiceWorkflow : Workflow<ProcessInvoiceRequest, ProcessInvoiceResult>
{
    public override async Task<ProcessInvoiceResult> RunAsync(WorkflowContext context, ProcessInvoiceRequest request)
    {
        try
        {
            // Ensure that this request is persisted
            var originalRequestPersisted =
                await context.CallActivityAsync<bool>(nameof(PersistInvoiceRequestActivity), request, GetDefaultRetryOptions());

            //var validNewInvoice = await context.CallActivityAsync<bool>(nameof(CheckDuplicateInvoiceActivity), request,
            //    GetDefaultRetryOptions());

            //var totalAmountSpent =
            //    await context.CallActivityAsync<decimal>(nameof(TotalInovicesActivicty), request,
            //        GetDefaultRetryOptions());

            return new ProcessInvoiceResult
            {
                Status = InvoiceProcessingStatus.Complete,
            };
        }
        catch
        {
            return new ProcessInvoiceResult { Status = InvoiceProcessingStatus.Failed };
        }
    }

    public static WorkflowTaskOptions GetDefaultRetryOptions()
    {
        return new WorkflowTaskOptions()
        {
            RetryPolicy = new WorkflowRetryPolicy(
                firstRetryInterval: TimeSpan.FromMinutes(1),
                backoffCoefficient: 2.0,
                maxRetryInterval: TimeSpan.FromHours(1),
                maxNumberOfAttempts: 10
            )
        };
    }
}
