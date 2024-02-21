# broadcast-social-media
This is a project for learning.


Denna rad i ApplicationDbContext.cs ändras från
    public class ApplicationDbContext : IdentityDbContext
till
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>

Det verkar vara nåt man bara gör efter att man skapat sin ApplicationUser. Är det en Best practice bara? Det var nåt kursledaren gjorde men inte förkalarade mer. Han as att det är dependency injection.


ChatGPT gav mig följande svar:

    public class ApplicationDbContext : IdentityDbContext
    
declares a class ApplicationDbContext that inherits from IdentityDbContext. IdentityDbContext is a part of ASP.NET Identity, and it is a generic class that expects a type parameter. When you don't provide a type parameter, it assumes a default type, which is IdentityUser. This means that the ApplicationDbContext is set up to work with the default IdentityUser class for user-related information.

The modified line:

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    
changes the type parameter of IdentityDbContext to ApplicationUser. This indicates that the ApplicationDbContext is now specifically configured to work with a custom class called ApplicationUser for user-related information instead of the default IdentityUser.

In simpler terms, this change is specifying a custom user class (ApplicationUser) to be used for managing user-related data in the application's database instead of the default user class provided by ASP.NET Identity. This can be useful when you want to extend or customize the user properties or behavior in your application.
