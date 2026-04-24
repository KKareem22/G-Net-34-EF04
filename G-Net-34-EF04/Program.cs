using G_Net_34_EF04.Models;
using G_Net_34_EF04.Models.Enum;
using Microsoft.EntityFrameworkCore;

namespace G_Net_34_EF04
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using BankDbContext dbContext = new();
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine(new string('=', 50));
                Console.WriteLine("\t\tNational Bank _  Management ");
                Console.WriteLine(new string('=', 50));
                Console.WriteLine("1) Add a new Customer .");
                Console.WriteLine("2) Open a new Account for a Customer .");
                Console.WriteLine("3) Update Account Status (Active / Closed) .");
                Console.WriteLine("4) Remove an Account from a Customer .");
                Console.WriteLine("5) List all Customers with their Accounts .");
                Console.WriteLine("0) Exit .");
                Console.WriteLine(new string('_', 50));
                Console.WriteLine("Enter Choice : ");
                string choice = Console.ReadLine()!;
                switch (choice)
                {
                    case "1":
                        AddCustomer(dbContext);
                        break;
                    case "2":
                        OpenAccount(dbContext);
                        break;
                    case "3":
                        UpdateAccountStatus(dbContext);
                        break;
                    case "4":
                        RemoveAccountFromCustomer(dbContext);
                        break;
                    case "5":
                        ListAllCustomer(dbContext);
                        break;
                    case "0":
                        exit = true;
                        Console.WriteLine("Exiting the application. Goodbye!");
                        break;
                    default:
                        Console.WriteLine("Invalid Choice. Please select a valid option.");
                        break;
                }
                if (!exit)
                {
                    Console.WriteLine("\nPress any key to return to menu...");
                    Console.ReadKey();
                }

            }

        }

        static void AddCustomer(BankDbContext dbContext)
        {
            Console.WriteLine("Enter a Full Name : ");
            string FullName = Console.ReadLine()!;
            Console.WriteLine("Enter your NationalId : ");
            string NationalId = Console.ReadLine()!;
            Console.WriteLine("Enter your Phone Number : ");
            string PhoneNumber = Console.ReadLine()!;
            Console.WriteLine("Enter your Email : ");
            string? Email = Console.ReadLine();
            DateOnly dob;
            bool isValid = false;
            Console.WriteLine("--- Address Details ---");
            Console.Write("Enter Street: ");
            string street = Console.ReadLine()!;
            Console.Write("Enter City: ");
            string city = Console.ReadLine()!;
            Console.Write("Enter Country: ");
            string country = Console.ReadLine()!;

            do
            {
                Console.Write("Enter Date Of Birth (YYYY-MM-DD): ");
                string input = Console.ReadLine()!;

   
                isValid = DateOnly.TryParse(input, out dob);

                if (!isValid)
                {
                    Console.WriteLine("Invalid Format! Please use YYYY-MM-DD (e.g., 1995-05-25).");
                }
            }
            while (!isValid);
            CustomerType type;
            do
            {
                Console.WriteLine("Enter Type (1:Individual),(2,Business) : ");
                isValid= Enum.TryParse(Console.ReadLine(), out type);

            }
            while (!isValid || !Enum.IsDefined(type));
            var customer=new Customer
            {
                FullName = FullName,
                NationalId = NationalId,
                PhoneNumber = PhoneNumber,
                Email = Email,
                DateOfBirth = dob,
                CustomerType = type,
                Address = new Address
                    {
                        Street = street,
                        City = city,
                        Country =country
                }


            };
            dbContext.Add(customer);
            dbContext.SaveChanges();
            Console.WriteLine("Customer Added Successfully!");

        }
        static void OpenAccount(BankDbContext dbContext)
        {
            dbContext.ChangeTracker.Clear();//مش دي لي بصراحة

            Console.Write("Account Number: ");
            if (!long.TryParse(Console.ReadLine(), out long accountNumber)) return;

            Console.WriteLine("Account Type: 1) Savings 2) Current 3) Business");
            string typeChoice = Console.ReadLine();
            var accType = typeChoice switch { "1" => AccountType.Savings, "2" => AccountType.Current, "3" => AccountType.Business, _ => AccountType.Savings };

            Console.Write("Branch Code: ");
            string branchCode = Console.ReadLine()!;

            var branch = dbContext.Branches.AsNoTracking().FirstOrDefault(b => b.Code == branchCode);
            if (branch == null)
            { Console.WriteLine("Branch Not Found!"); return; }

            Console.Write("Customer Id: ");
            if (!int.TryParse(Console.ReadLine(), out int customerId)) return;

            var customer = dbContext.Customers.AsNoTracking().FirstOrDefault(c => c.Id == customerId);
            if (customer == null) 
            { Console.WriteLine("Customer Not Found!"); return; }

            Console.WriteLine("Ownership Role: 1) Primary 2) CoHolder");
            string roleChoice = Console.ReadLine();
            var role = (roleChoice == "1") ? OwnershipType.Primary : OwnershipType.CoHolder;

            try
            {
                var existingAccount = dbContext.Accounts.AsNoTracking().FirstOrDefault(a => a.AccountNumber == accountNumber);

                if (existingAccount == null)
                {
                    var newAccount = new Account
                    {
                        AccountNumber = accountNumber,
                        BranchId = branchCode,
                        CurrentBalance = 0,
                        Status = AccountStatus.Active,
                        AccountType = accType,
                    };
                    dbContext.Accounts.Add(newAccount);
                }

                var customerAccount = new CustomerAccount
                {
                    CustomerId = customerId,
                    AccountId = accountNumber,
                    OwnershipStartDate = DateTime.Now,
                    OwnershipType = role,
                    Status = AccountStatus.Active
                };

                dbContext.CustomerAccounts.Add(customerAccount);

                dbContext.SaveChanges();

                Console.WriteLine($"\nValidating branch '{branchCode}' and customer #{customerId}...");
                Console.WriteLine($" Success! Account {accountNumber} created and linked to Customer.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(" Error: " + ex.Message);
            }
        }
        static void ListAllCustomer(BankDbContext dbContext)
        {
            Console.WriteLine("**********Registered Customers & Their Accounts**********");
            var customers=dbContext.Customers
                .Include(c=>c.CustomerAccounts)
                .ThenInclude(ca=>ca.Account)
                .ToList();
            if(!customers.Any())
            {
                Console.WriteLine("No Customers in System !");
                return;
            }
            foreach(var c in customers)
            {
                Console.WriteLine($"Customer Id :{c.Id} | Name :{c.FullName} | NationalId:{c.NationalId} | PhoneNumber :{c.PhoneNumber}\n Type:{c.CustomerType} | DateOfBirth :{c.DateOfBirth}");
                if(c.CustomerAccounts!=null && c.CustomerAccounts.Any())
                {
                    Console.WriteLine("\t\tAccounts");
                    foreach(var ca in c.CustomerAccounts)
                    {
                        Console.WriteLine($"AccountNumber :{ca.AccountId} | Type :{ca.Account.AccountType} | Status :{ca.Status}");
                    }
                }
                else
                {
                    Console.WriteLine("\t\tNo Accounts Linked.");
                }
            }
            Console.WriteLine(new string('_',50));
        }
        static void UpdateAccountStatus(BankDbContext dbContext)
        {
            Console.WriteLine("Enter Account Number : ");
            if(!long.TryParse(Console.ReadLine(),out long accountNumber))
            {
                Console.WriteLine("InVaild Account Number . ");
                return;
            }

            var accountExists = dbContext.Accounts.Any((a => a.AccountNumber == accountNumber));
            if(!accountExists)
            {
                Console.WriteLine("Account Not Found !");
                return;
            }
            var customerAccounts = dbContext.CustomerAccounts
                .Where(ca => ca.AccountId == accountNumber).ToList();
            if(!customerAccounts.Any())
            {
                Console.WriteLine("No Customer Linked to this Account !");
                return;
            }
            Console.WriteLine("Select New Status (1:Active, 2:Closed, 3:Suspended) : ");
            if (!Enum.TryParse(Console.ReadLine(), out AccountStatus NewStatus) || !Enum.IsDefined(NewStatus))
            {
                Console.WriteLine("Invalid Status Selection");
            }
            foreach(var ca in customerAccounts)
            {
                ca.Status = NewStatus;
            }
            dbContext.SaveChanges();
            Console.WriteLine($"Account {accountNumber} status updated to {NewStatus} for all linked customers.");


        }
        static void RemoveAccountFromCustomer(BankDbContext dbContext)
        {
            Console.WriteLine("Remove Account From Customer");
            Console.WriteLine("Enter Customer Id : ");
            if (!int.TryParse(Console.ReadLine(), out int customerId))
            {
                Console.WriteLine("InVaild Customer Id . ");
                return;
            }
            Console.WriteLine("Enter a Account Number : ");
            if(!long.TryParse(Console.ReadLine(),out long accountNumber))
            {
                Console.WriteLine("Invaild Account Number : ");
                return;
            }
            var customerAccount = dbContext.CustomerAccounts
                .FirstOrDefault(ca => ca.CustomerId == customerId && ca.AccountId == accountNumber);
            if (customerAccount == null)
            {
                Console.WriteLine("Error: This link does not exist (Customer is not an owner of this account).");
                return;
            }
            try
            {
                dbContext.CustomerAccounts.Remove(customerAccount);
                Console.WriteLine($"Step 1: Removing the link between Customer {customerId} and Account {accountNumber}...");
                var otherOwners = dbContext.CustomerAccounts
                    .Any(ca => ca.AccountId == accountNumber && ca.CustomerId != customerId);
                if (!otherOwners)
                {
                    var account = dbContext.Accounts.Find(accountNumber);
                    if (account != null)
                    {
                        dbContext.Accounts.Remove(account);
                        Console.WriteLine($"Step 2 :Account {accountNumber} has been removed from the system as it has no more owners.");
                    }
                }
                dbContext.SaveChanges();
                Console.WriteLine($"Customer {customerId} is no longer an owner of Account {accountNumber}.");
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error,"+ex.Message);
            }
        }





    }
    

}
