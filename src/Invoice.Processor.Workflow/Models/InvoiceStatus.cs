namespace Invoice.Processor.Workflow.Models;

public enum InvoiceProcessingStatus
{
    Unknown = 0,
    New = 1,
    Processing = 2,
    Complete = 100,
    Failed = 999,
}
