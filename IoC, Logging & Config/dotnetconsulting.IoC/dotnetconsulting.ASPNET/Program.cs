// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschränkungen verwendet oder verändert werden.
// Jedoch wird keine Garantie übernommen, dass eine Funktionsfähigkeit mit aktuellen und 
// zukünftigen API-Versionen besteht. Der Autor übernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgeführt wird.
// Für Anregungen und Fragen stehe ich jedoch gerne zur Verfügung.

// Thorsten Kansy, www.dotnetconsulting.eu

using dotnetconsulting.ASPNET.Code;
using dotnetconsulting.ServiceAndInterfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddKeyedTransient<IOrderService, EMailOrderService>("email");
builder.Services.AddKeyedTransient<IOrderService, SnailMailOrderService>("snail");
builder.Services.AddTransient<IPostageService, GermanPostageService>();
builder.Services.AddTransient<IPayment, PayPal>();
builder.Services.AddScoped(o => "Hallo Welt");
// builder.Services.AddSingleton<DateTime>(o => DateTime.Now);

builder.Services.AddTransient<TestInject>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();