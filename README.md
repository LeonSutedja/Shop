# MyShop
Leon Sample Shop Application

Technology used:
  - c#
  - MVC .NET Framework with Razor
  - Unity
  - Jquery, Javascript, bootstrap, Jquery UI

Some example of patterns/goodies that was used in the shop:
  - Unity configuration for Generic Interface (in Unity.cs)
  - Scalable Generic command handler pattern to handle requests to controller (in AccountController.cs)
  - Domain Driven Design with Component based system (Order, Product, Account)
  - Custom .NET MVC cshtml View finder
  - IoC with Unity
  - Repository pattern for entities
  - JQuery UI dialog as a component (all you need is attach it on the page you want to have the dialog)
  - Session with Facade pattern to access the specific session for the user
