using ErpPaymentAssistant;
using Microsoft.Extensions.AI;
using OllamaSharp;
using System.Net.Http;
using Microsoft.Agents.AI;



Customer customer = new Customer
{
    Id = 1,
    Name = "ABC Teknoloji",
    Email = "info@abc.com"
};

List<Invoice> invoices = new List<Invoice>
{
    new Invoice
    {
        Id = 101,
        CustomerId = 1,
        Amount = 15000,
        DueDate = DateTime.Today.AddDays(-10),
        IsPaid = false
    },

    new Invoice
    {
        Id = 102,
        CustomerId = 1,
        Amount = 8000,
        DueDate = DateTime.Today.AddDays(-5),
        IsPaid = true
    },

    new Invoice
    {
        Id = 103,
        CustomerId = 1,
        Amount = 12000,
        DueDate = DateTime.Today.AddDays(5),
        IsPaid = false
    }
};

InvoiceService invoiceService = new InvoiceService();

List<Invoice> overdueInvoices =
    invoiceService.GetOverdueInvoices(invoices);

Console.WriteLine($"Müşteri: {customer.Name}");
Console.WriteLine("Vadesi geçmiş faturalar:");

foreach (Invoice invoice in overdueInvoices)
{
    Console.WriteLine(
        $"Fatura No: {invoice.Id} - Tutar: {invoice.Amount} TL");
}

decimal totalDebt =
    invoiceService.GetTotalOverdueAmount(1, invoices);

Console.WriteLine($"Toplam gecikmiş borç: {totalDebt} TL");

InvoiceTools invoiceTools =
    new InvoiceTools(invoiceService, invoices);

var overdueDebtTool =
    AIFunctionFactory.Create(
        invoiceTools.GetCustomerOverdueAmount);

Console.WriteLine($"Tool adı: {overdueDebtTool.Name}");
Console.WriteLine($"Tool açıklaması: {overdueDebtTool.Description}");

var httpClient = new HttpClient
{
    BaseAddress = new Uri("http://localhost:11434/"),
    Timeout = TimeSpan.FromMinutes(5)
};

IChatClient chatClient = new OllamaApiClient(
    httpClient,
    "qwen3:4b"
);

var response = await chatClient.GetResponseAsync(
    "Merhaba! Sen kimsin? Türkçe cevap ver."
);

Console.WriteLine("Ollama cevabı:");
Console.WriteLine(response.Text);

AIAgent agent = chatClient.AsAIAgent(
    instructions: "Sen ERP Payment Assistant adlı bir asistansın. " +
                  "Müşteri faturalarıyla ilgili soruları yanıtlarsın. " +
                  "Gecikmiş borç tutarlarını kendin hesaplama. " +
                  "Bunun yerine GetCustomerOverdueAmount aracını kullan. " +
                  "Kullanıcı müşteri numarasını belirtmezse numarayı sor. " +
                  "Sonuçları Türkçe ve anlaşılır şekilde sun.",
    name: "ErpPaymentAssistant",
    tools: [overdueDebtTool]
);
var agentResponse = await agent.RunAsync(
    "ABC Teknoloji müşterisinin gecikmiş borcu ne kadar? Müşteri numarası 1."
);

Console.WriteLine("AI Agent cevabı:");
Console.WriteLine(agentResponse);
Console.WriteLine(await agent.RunAsync(
    "Gecikmiş borcum ne kadar?"
));
