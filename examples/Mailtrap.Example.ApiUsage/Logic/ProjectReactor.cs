namespace Mailtrap.Example.ApiUsage.Logic;


internal sealed class ProjectReactor : Reactor
{
    private readonly SandboxReactor _sandboxReactor;


    public ProjectReactor(
        SandboxReactor sandboxReactor,
        IMailtrapClient mailtrapClient,
        ILogger<ProjectReactor> logger)
        : base(mailtrapClient, logger)
    {
        _sandboxReactor = sandboxReactor;
    }


    public async Task Process(long accountId)
    {
        IAccountResource accountResource = _mailtrapClient.Account(accountId);

        var projectName = "My Test Project";

        // Get resource for projects collection
        IProjectCollectionResource projectsResource = accountResource.Projects();

        // Get all projects for account
        IList<Project> projects = await projectsResource.GetAll();

        Project? project = projects
            .FirstOrDefault(p => string.Equals(p.Name, projectName, StringComparison.OrdinalIgnoreCase));

        if (project is null)
        {
            _logger.LogWarning("No project found. Creating.");

            // Create project
            var createProjectRequest = new CreateProjectRequest(projectName);
            project = await projectsResource.Create(createProjectRequest);
        }
        else
        {
            _logger.LogInformation("Project found.");
        }

        // Get resource for specific project
        IProjectResource projectResource = accountResource.Project(project.Id);

        // Get details
        project = await projectResource.GetDetails();
        _logger.LogInformation("Project: {Project}", project);

        // Process sandboxes
        await _sandboxReactor.Process(accountId, project.Id);

        // Update project details
        var updateProjectRequest = new UpdateProjectRequest("Updated Project Name");
        Project updatedProject = await projectResource.Update(updateProjectRequest);
        _logger.LogInformation("Updated Project: {Project}", updatedProject);

        // Delete project
        // Beware that project resource becomes invalid after deletion and should not be used anymore
        DeleteProjectResponse deletedProject = await projectResource.Delete();
        _logger.LogInformation("Deleted Project: {Project}", deletedProject);
    }
}
