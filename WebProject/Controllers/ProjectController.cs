using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebProject.Data;
using WebProject.Data.Models;
using WebProject.ViewModels;
//using System.Data.Objects;

namespace WebProject.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ApplicationContext applicationContext;
        private static List<Employee> AddedEmployees { get; set; } = new List<Employee>();
        private static List<int> Employees { get; set; } = new List<int>();
        //private static List<Employee> employeesAdded = new List<Employee>();
        private static int counter = 0;
        private static int counterForAdd = 0;
        public ProjectController(ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
            //Employees = applicationContext.Employees.ToList();
            //AddedEmployees = new List<Employee>();
        }

        [HttpGet]
        public ActionResult AddProject()
        {
            ProjectViewModel projectViewModel = new ProjectViewModel();
            if (counter == 0)
            {
                foreach (var employee in applicationContext.Employees)
                {
                    projectViewModel.Employees.Add(employee);
                }
                counter++;
            }
            else
            {
                projectViewModel.Employees.Clear();
                List<Employee> list = new List<Employee>();
                foreach(var id in Employees)
                {
                    var employeeList = applicationContext.Employees.Where(e => e.Id == id).ToList();
                    foreach(var employee in employeeList)
                    {
                        list.Add(employee);
                    }
                }
                foreach(var employee in list)
                {
                    projectViewModel.Employees.Add(employee);
                }
                //projectViewModel.Employees = Employees;
                //projectViewModel.Employees.Clear();
                //
            }
            projectViewModel.EmployeeSelectedList = new List<SelectListItem>();
            foreach (var employee in projectViewModel.Employees)
            {
                projectViewModel.EmployeeSelectedList.Add(new SelectListItem()
                { Text = employee.Name + " " + employee.Surname + " " + employee.Patronymic, Value = employee.Id.ToString() });
            }
            return View(projectViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddProjectAsync(ProjectViewModel projectViewModel)
        {
            List<Employee> employeeProjects = new List<Employee>();
            EmployeeProject employeeProject = null;
            //using (var dbContextTransaction = applicationContext.Database.BeginTransaction())
            //{
            //    try
            //    {

            //        applicationContext.SaveChanges();

            //        // do another changes
            //        context.SaveChanges();

            //        dbContextTransaction.Commit();
            //    }
            //    catch (Exception ex)
            //    {
            //        //Log, handle or absorbe I don't care ^_^
            //    }
            //}
            if (projectViewModel.Project_Name != null && projectViewModel.Company_Customer != null
                && projectViewModel.Company_Producer != null && projectViewModel.Start_Date != null &&
                projectViewModel.Finish_Date != null && projectViewModel.Selected_Manager != null &&
                (projectViewModel.Priority > 0 && projectViewModel.Priority < 4) && AddedEmployees.Count != 0)
            {

                //using(var transaction = applicationContext.Database.BeginTransaction())
                //{

                //}
                var getManager = applicationContext.Employees.Where(emp => emp.Id == Int32.Parse(projectViewModel.Selected_Manager)).ToList();
                foreach (var employee in getManager)
                {
                    AddedEmployees.Add(employee);
                }
                Project project = new Project()
                {
                    Project_Name = projectViewModel.Project_Name,
                    Company_Customer = projectViewModel.Company_Customer,
                    Company_Producer = projectViewModel.Company_Producer,
                    Start_Date = projectViewModel.Start_Date,
                    Finish_Date = projectViewModel.Finish_Date,
                    ManagerId = Int32.Parse(projectViewModel.Selected_Manager),
                    Priority = projectViewModel.Priority
                    //Employees = AddedEmployees
                    //EmployeeProjects = applicationContext
                };
                for (int i = 0; i < AddedEmployees.Count; i++)
                {
                    //employeeProjects.Add(applicationContext.Employees.FirstOrDefault(id => id.Id == AddedEmployees[i].Id));
                    //AddedEmployees[i].Id = 0;
                    employeeProject = new EmployeeProject()
                    {
                        Employee = AddedEmployees[i],
                        Project = project
                    };
                    applicationContext.Employees.Attach(AddedEmployees[i]);
                    applicationContext.EmployeeProjects.Add(employeeProject);
                }
                //foreach (var emp in AddedEmployees)
                //{
                //    var employeeProject = new EmployeeProject()
                //    {
                //        Employee = emp,
                //        Project= project
                //    };
                //    employeeProjects.Add(employeeProject);
                //    //await applicationContext.SaveChangesAsync();
                //}
                //if()
                applicationContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Projects ON;");
                //applicationContext.SaveChanges();
                applicationContext.Projects.Add(project);
                //foreach (var employee in applicationContext.Employees)
                //{
                //    var empList = applicationContext.Employees.Where(emp => emp.Name == employee.Name
                //    && emp.Surname == employee.Surname
                //    && emp.Patronymic == employee.Patronymic
                //    && emp.Email == employee.Email).ToList();
                //    foreach (var deleteDuplicates in empList)
                //    {
                //        applicationContext.Remove(deleteDuplicates);
                //    }
                //}
                await applicationContext.SaveChangesAsync();
                //applicationContext.Projects.AddRange(employeeProjects);
                AddedEmployees.Clear();
                counter = 0;
                Employees.Clear();
                applicationContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Projects OFF;");
            }
            return Redirect("/Project/ListOfProject");
        }

        [HttpGet]
        public ActionResult ListOfProject()
        {
            ProjectViewModel projectViewModel = new ProjectViewModel();
            projectViewModel.ProjectList = applicationContext.Projects.ToList();
            projectViewModel.EmployeeProjects = applicationContext.EmployeeProjects.ToList();
            projectViewModel.Employees = applicationContext.Employees.ToList();
            //projectViewModel.ManagerList = applicationContext.Employees.ToList();
            //List<Employee> managerList = new List<Employee>();
            //var listOfProjectId = applicationContext.Projects.Select(project => project.Id).ToList();

            foreach (var id in applicationContext.Projects.Select(proj => proj.ManagerId).ToList())
            {
                foreach (var employee in applicationContext.Employees.Where(emp => emp.Id == id))
                {
                    projectViewModel.ManagerList.Add(employee);
                }
            }
            var listOfEmployeeId = applicationContext.Projects.Select(project => project.ManagerId).ToList();
            foreach (var id in listOfEmployeeId)
            {
                var managerList = applicationContext.Employees.Where(employee => employee.Id == id).ToList();
                //foreach(var manager in managerList)
                //{
                //    projectViewModel.ManagerList.Add(id, manager);
                //}
            }
            return View(projectViewModel);
        }

        [HttpGet]
        public ActionResult DeleteProject(int id)
        {
            Project project = applicationContext.Projects.Where(e => e.Id == id).FirstOrDefault();
            return View(project);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProjectAsync(Project project)
        {
            applicationContext.Remove(project);
            //applicationContext.Employees.Remove(project.Employees);
            //applicationContext.Projects.Remove(project);
            //applicationContext.ManagerTables.Remove(project.ManagerTable);
            await applicationContext.SaveChangesAsync();
            return Redirect("/Project/ListOfProject");
        }

        public ActionResult AddEmployeeToList(ProjectViewModel projectViewModel)
        {
            if(counterForAdd == 0)
            {
                foreach (var id in applicationContext.Employees.Select(e => e.Id))
                {
                    Employees.Add(id);
                }
                counterForAdd++;
            }
            //projectViewModel.Employees.Clear();
            //if (counterForAdd == 0)
            //{
            //    foreach (var employee in applicationContext.Employees)
            //    {
            //        Employees.Add(employee);
            //    }
            //    counterForAdd++;
            //}
            //foreach (var employee in applicationContext.Employees)
            //{
            //    Employees.Add(employee);
            //}
            //List<Employee> list = new List<Employee>();
            //List<Employee> removedEmployees = new List<Employee>();
            //Employee employeeDelete = null;
            var employeeSelected = applicationContext.Employees.Where(employee => employee.Id == Int32.Parse(projectViewModel.Selected_Employee)).ToList();
            foreach (var employee in employeeSelected)
            {
                Employees.Remove(employee.Id);
                AddedEmployees.Add(employee);
            }
            //Employees.Remove()
            //Employees = Employees.Distinct().ToList();
            //var addedIds = AddedEmployees.Select(e => e.Id).ToList();
            //foreach (var employee in AddedEmployees)
            //{
            //    Employees.Remove(employee);
            //}

            //var list = Employees.Except(AddedEmployees).ToList();
            //Employees.Clear();
            //foreach(var employee in list)
            //{
            //    Employees.Add(employee);
            //}
            //var list = Employees.Distinct().ToList();

            //foreach (var employee in AddedEmployees)
            //{
            //    list.Remove(employee);
            //}
            //Employees.Clear();
            //Employees = list;
            //list.Clear();
            //foreach(Employee employee in projectViewModel.Employees)
            //{
            //    Employees.Add(employee);
            //}
            //for(int i = 0; i < AddedEmployees.Count(); i++)
            //{
            //    if (Employees.Contains(AddedEmployees[i]))
            //    {
            //        Employees.RemoveAt(i);
            //    }
            //}
            //AddedEmployees.Add(employeeDelete);
            //foreach(var employee in employeeSelected)
            //{

            //}
            //for(int i = 0; i < Employees.Count; i++)
            //{
            //    if (Employees.Contains(AddedEmployees[i]))
            //    {
            //        Employees.Remove(AddedEmployees[i]);
            //    }
            //    //if (!employeesAdded.Contains(AddedEmployees[i]))
            //    //{
            //    //    employeesAdded.Add(AddedEmployees[i]);
            //    //}
            //}
            //for (int i = 0; i < removedEmployees.Count; i++)
            //{
            //    Employees.Remove(removedEmployees[i]);
            //}
            //projectViewModel.Employees = list;
            return Redirect("/Project/AddProject");
        }

        [HttpGet]
        public ActionResult EditProject(int id)
        {
            //Project project = new Project();
            Project project = applicationContext.Projects.Where(p => p.Id == id).FirstOrDefault();
            //foreach(var employee in applicationContext.Employees)
            //{
            //    project.Employees.Add(employee);
            //}
            project.Employees = applicationContext.Employees.ToList();
            project.EmployeeProjects = applicationContext.EmployeeProjects.ToList();
            return View(project);
        }

        [HttpPost]
        public async Task<IActionResult> EditProjectAsync(Project project)
        {
            //Project project = new Project();
            //var employeeList = applicationContext.Projects.Where(e => e.Id == ProjectID).ToList();
            //foreach (var item in employeeList)
            //{
            //    project = item;
            //}
            var managerIdList = applicationContext.Projects.Where(p => p.Id == project.Id).Select(p => p.ManagerId).ToList();
            foreach(var id in managerIdList)
            {
                project.ManagerId = id;
            }

            if (project.Project_Name != null && project.Company_Customer != null
                && project.Company_Producer != null && project.Start_Date != null &&
                project.Finish_Date != null && project.ManagerId != 0 &&
                (project.Priority > 0 && project.Priority < 4))
            {
                applicationContext.Update(project);
                await applicationContext.SaveChangesAsync();
            }
            return Redirect("/Project/ListOfProject");
        }

        public ActionResult DeleteManager(Project project)
        {
            project.ManagerId = 0;
            applicationContext.Update(project);
            applicationContext.SaveChanges();
            return Redirect("/Project/EditProject/" + project.Id);
        }

        public ActionResult AddManager(int ProjectID, int EmployeeID)
        {
            Project project = new Project();
            //Employee employee = new Employee();
            var employeeList = applicationContext.Projects.Where(e => e.Id == ProjectID).ToList();
            foreach (var item in employeeList)
            {
                project = item;
            }
            if (project.Id != 0)
            {
                project.ManagerId = EmployeeID;
                applicationContext.Update(project);
                applicationContext.SaveChanges();
                return Redirect("/Project/EditProject/" + project.Id);
            }
            else
            {
                return Redirect("/Project/ListOfProject");
            }
        }

        public ActionResult DeleteEmployee(int ProjectID, int EmployeeID)
        {
            Project project = new Project();
            var employeeList = applicationContext.Projects.Where(e => e.Id == ProjectID).ToList();
            foreach (var item in employeeList)
            {
                project = item;
            }
            List<EmployeeProject> employeeProjects = applicationContext.EmployeeProjects.Where(ep => ep.ProjectId == project.Id).ToList();
            List<EmployeeProject> employees = employeeProjects.Where(ep => ep.EmployeeId == EmployeeID).ToList();
            foreach(var employeeProject in employees)
            {
                applicationContext.Remove(employeeProject);
            }
            applicationContext.SaveChanges();
            return Redirect("/Project/EditProject/" + project.Id);
        }

        public ActionResult AddEmployee(int ProjectID, int EmployeeID)
        {
            Project project = new Project();
            var employeeList = applicationContext.Projects.Where(e => e.Id == ProjectID).ToList();
            foreach (var item in employeeList)
            {
                project = item;
            }
            List<Employee> employees = applicationContext.Employees.Where(e => e.Id == EmployeeID).ToList();
            foreach(var e in employees)
            {
                EmployeeProject employeeProject = new EmployeeProject()
                {
                    ProjectId = project.Id,
                    EmployeeId = e.Id
                };
                applicationContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Projects ON;");
                applicationContext.Employees.Attach(e);
                applicationContext.EmployeeProjects.Add(employeeProject);
                applicationContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Projects OFF;");
            }
            applicationContext.SaveChanges();
            return Redirect("/Project/EditProject/" + project.Id);
        }
    }
}
