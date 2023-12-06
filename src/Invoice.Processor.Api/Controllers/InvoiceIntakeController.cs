using System.Net.Mime;
using Dapr;
using Dapr.Client;
using Invoice.Processor.Workflow.Models;
using Microsoft.AspNetCore.Mvc;

namespace Invoice.Processor.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class InvoiceIntakeController : ControllerBase
{
    private readonly DaprClient _daprClient;
    private readonly ILogger<InvoiceIntakeController> _logger;

    public InvoiceIntakeController(DaprClient daprClient, ILogger<InvoiceIntakeController> logger)
    {
        _daprClient = daprClient;
        _logger = logger;
    }

    /// <summary>
    /// Main starting point on how to intake an invoice into the system
    /// Goal:
    ///  1. Intake a simple object and write to the dapr pub-sub bus
    /// </summary>
    /// <param name="request" cref="ProcessInvoiceRequest"></param>
    [HttpPost("/invoice/intake")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ProcessInvoiceResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Topic("invoice-pubsub", "intake-invoice")]
    public async Task<ActionResult<ProcessInvoiceResult>> ImportInvoiceHttp(ProcessInvoiceRequest request)
    {
        if (!ModelState.IsValid) { return  BadRequest(ModelState); }

        // Generate a request id to be returned to applicant
        var invoiceId = Guid.NewGuid();

        await PublishInvoiceRequest(request);

        var routedValue = new { invoiceId = invoiceId };

        return new CreatedAtActionResult(nameof(GetInvoiceProcessStatus), null, routedValue,
            new ProcessInvoiceResult
            {
                Status = InvoiceProcessingStatus.New,
                LastUpdated = DateTime.Now,
                InvoiceId = invoiceId
            });
    }

    /// <summary>
    /// Return the status of an invoice that has already been processed by 'invoice/intake'
    /// Goal:
    ///  1. Be able to return the id from the queue and ensure that processing begins
    /// </summary>
    /// <param name="invoiceId">Invoice Id (GUID) Identifier given to user when request was submitted</param>
    [HttpGet(Name = "/invoice/status")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ProcessInvoiceResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProcessInvoiceResult?>> GetInvoiceProcessStatus(string invoiceId)
    {
        // var baseUrl = "http://localhost:5001/api/studentenrollment/enrollmentstatus";
        var baseUrl = "";
        var client = new HttpClient();
        //client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        //client.DefaultRequestHeaders.Add("dapr-app-id", "studentenrollmentapi");

        var status = await client.GetFromJsonAsync<ProcessInvoiceResult>($"{baseUrl}?invoiceId={invoiceId}");

        if (status == null) { return NotFound(); }
        return Ok(status);
    }

    private async Task PublishInvoiceRequest(ProcessInvoiceRequest invoiceRequest)
    {
        const string PUBSUB_NAME = "invoice-pubsub-redis";
        const string TOPIC_NAME = "invoices";

        var source = new CancellationTokenSource();
        var cancellationToken = source.Token;

        await _daprClient.PublishEventAsync(PUBSUB_NAME, TOPIC_NAME, invoiceRequest, cancellationToken);
    }
}