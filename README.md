# MyShop
Leon Sample Simple Shop Application

Technology used:
  - c#
  - MVC .NET Framework with Razor
  - Unity
  - Jquery, Javascript, bootstrap, Jquery UI
  - xUnit, Shouldly

Highlights of the patterns in Shop:
  - Domain Driven Design with Component based system (Order, Product, Account)
  - Scalable Generic command handler pattern to handle requests to controller (AccountController)
  - Generic Table Creator that can be used to create table on the fly.

Some example of patterns/goodies that was used in the shop:
  - Unity configuration for Generic Interface (Unity.cs)
  - Custom .NET MVC cshtml View finder
  - Repository pattern for entities
  - JQuery UI dialog as a component (all you need is attach it on the page you want to have the dialog)
  - Session with Facade & Strategy pattern to access the specific session for the user
