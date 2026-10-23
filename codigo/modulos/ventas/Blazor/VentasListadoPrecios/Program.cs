using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

var ConstructorAplicacion = WebApplication.CreateBuilder(args);
ConstructorAplicacion.Services.AddRazorPages();
ConstructorAplicacion.Services.AddServerSideBlazor();

var Aplicacion = ConstructorAplicacion.Build();

if (!Aplicacion.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    Aplicacion.UseHsts();
}

Aplicacion.UseHttpsRedirection();

Aplicacion.UseStaticFiles();

Aplicacion.UseRouting();

Aplicacion.MapBlazorHub();
Aplicacion.MapFallbackToPage("/_Host");

Aplicacion.Run();
