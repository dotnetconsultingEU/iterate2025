// Disclaimer
// Dieser Quellcode ist als Vorlage oder als Ideengeber gedacht. Er kann frei und ohne 
// Auflagen oder Einschr�nkungen verwendet oder ver�ndert werden.
// Jedoch wird keine Garantie �bernommen, dass eine Funktionsf�higkeit mit aktuellen und 
// zuk�nftigen API-Versionen besteht. Der Autor �bernimmt daher keine direkte oder indirekte 
// Verantwortung, wenn dieser Code gar nicht oder nur fehlerhaft ausgef�hrt wird.
// F�r Anregungen und Fragen stehe ich jedoch gerne zur Verf�gung.

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