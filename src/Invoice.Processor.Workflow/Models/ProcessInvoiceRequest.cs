using System.ComponentModel.DataAnnotations;

namespace Invoice.Processor.Workflow.Models;

public class ProcessInvoiceRequest
{
    [Required(ErrorMessage = "Company who is doing sent the invoice is required")]
    public string? Company { get; set; }
    public string? DueDate { get; set; }
    public DateTime InvoiceDate { get; set; } = DateTime.UtcNow;
    public decimal? Amount { get; set; }
}
