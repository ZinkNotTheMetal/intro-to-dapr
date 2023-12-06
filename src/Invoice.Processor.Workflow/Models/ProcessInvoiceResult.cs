namespace Invoice.Processor.Workflow.Models;

public class ProcessInvoiceResult
{
    public Guid InvoiceId { get; set; }

    public InvoiceProcessingStatus Status { get; set; }

    public DateTime LastUpdated { get; set; }
}
