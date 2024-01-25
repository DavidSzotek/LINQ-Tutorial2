using System.Linq;

/* QUERY Syntax 
 * does not support two operators so the query has to be wrapped in brackets and called ToList, ToArray, ToDictionary etc.
 */

class Program
{
    private static void Main(string[] args)
    {
        List<Employee> employeeList = Data.GetEmployees();
        List<Department> departmentList = Data.GetDepartments();

        /****** Select and Where operators - Method syntax ******/
        var methodResults = employeeList.Select(e => new
        {
            FullName = e.FirstName + " " + e.LastName,
            AnnualSalary = e.AnnualSalary
        }).Where(e => e.AnnualSalary >= 50000);

        foreach (var result in methodResults)
        {
            Console.WriteLine($"{result.FullName}'s salary is: {result.AnnualSalary}");
        }

        /****** Select and Where operators - Query syntax ******/
        /* has DEFERED EXECUTION meaning the query is executed before the list is traversed, not in line of code*/
        var defferedResults = from employee in employeeList
                           where employee.AnnualSalary >= 50000
                           select new
                           {
                               FullName = employee.FirstName + " " + employee.LastName,
                               AnnualSalary = employee.AnnualSalary
                           };

        employeeList.Add(new Employee
        {
            Id = 5,
            FirstName = "Sam",
            LastName = "Sulek",
            AnnualSalary = 100000.20m,
            IsManager = true,
            DepartmentId = 2
        });

        foreach (var result in defferedResults)
        {
            Console.WriteLine($"Deffered - {result.FullName}'s salary is {result.AnnualSalary}");
        }

        /**** LAZY EXECUTION ****/

        var yieldResults = from employee in employeeList.GetHighSalariedEmployees()
                           select new
                           {
                               FullName = employee.FirstName + " " + employee.LastName,
                               AnnualSalary = employee.AnnualSalary
                           };

        foreach (var result in yieldResults)
        {
            Console.WriteLine($"Lazy - {result.FullName}'s salary is {result.AnnualSalary}");
        }

        /**** IMMEDIATE EXECUTION is achieved by using To conversion method (ToList etc.) ****/
        employeeList.Add(new Employee
        {
            Id = 6,
            FirstName = "Jonah",
            LastName = "Davis",
            AnnualSalary = 120000.20m,
            IsManager = false,
            DepartmentId = 3
        });

        /**** IMMEDIATE EXECUTION ****/

        var immediateResults = (from employee in employeeList.GetHighSalariedEmployees()
                               select new
                               {
                                   FullName = employee.FirstName + " " + employee.LastName,
                                   AnnualSalary = employee.AnnualSalary
                               }).ToList();

        foreach (var result in immediateResults)
        {
            Console.WriteLine($"Immediate - {result.FullName}'s salary is {result.AnnualSalary}");
        }

        Console.ReadKey();
    }
}

public static class EnumerableCollectionExtensionMethods
{
    public static IEnumerable<Employee> GetHighSalariedEmployees(this IEnumerable<Employee> employees)
    {

        foreach(Employee employee in employees)
        {
            Console.WriteLine($"Yield extension - Accessing employee: {employee.FirstName + " " + employee.LastName}");
            if(employee.AnnualSalary >= 50000)
            {
                yield return employee;
            }
        }
    }
}

public class Department
{
    public int Id { get; set; }
    public string ShortName { get; set; }
    public string LongName { get; set; }
}

public class Employee
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public decimal AnnualSalary { get; set; }
    public bool IsManager { get; set; }
    public int DepartmentId { get; set; }
}

public static class Data
{
    public static List<Employee> GetEmployees()
    {
        List<Employee> employees = new List<Employee>();

        Employee employee = new Employee()
        {
            Id = 1,
            FirstName = "Bob",
            LastName = "Jones",
            AnnualSalary = 60000.3m,
            IsManager = true,
            DepartmentId = 1,
        };
        employees.Add(employee);
        employee = new Employee()
        {
            Id = 2,
            FirstName = "Sarah",
            LastName = "Jameson",
            AnnualSalary = 80000.1m,
            IsManager = true,
            DepartmentId = 2,
        };
        employees.Add(employee);
        employee = new Employee()
        {
            Id = 3,
            FirstName = "Douglas",
            LastName = "Roberts",
            AnnualSalary = 40000.2m,
            IsManager = false,
            DepartmentId = 3,
        };
        employees.Add(employee);
        employee = new Employee()
        {
            Id = 4,
            FirstName = "Jane",
            LastName = "Stevens",
            AnnualSalary = 30000.2m,
            IsManager = false,
            DepartmentId = 1,
        };
        employees.Add(employee);

        return employees;
    }

    public static List<Department> GetDepartments()
    {
        List<Department> departments = new List<Department>();

        Department department = new Department()
        {
            Id = 1,
            ShortName = "HR",
            LongName = "Human Resources"
        };
        departments.Add(department);

        department = new Department()
        {
            Id = 2,
            ShortName = "FN",
            LongName = "Finance"
        };
        departments.Add(department);

        department = new Department()
        {
            Id = 3,
            ShortName = "TE",
            LongName = "Technology"
        };
        departments.Add(department);

        return departments;
    }
}