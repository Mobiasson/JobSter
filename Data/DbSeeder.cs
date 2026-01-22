using JobSter.Model;
using MongoDB.Driver;

namespace JobSter.Data;

internal static class DbSeeder {
    public static async Task SeedIfEmptyAsync(MongoDbContext context) {
        var companies = context.Companies;
        var jobApps = context.JobApplications;

        var companiesCount = await companies.CountDocumentsAsync(FilterDefinition<Company>.Empty);
        var appsCount = await jobApps.CountDocumentsAsync(FilterDefinition<JobApplication>.Empty);

        if(companiesCount == 0) {
            var seedCompanies = new List<Company>
            {
                new Company { CompanyName = "Contoso Ltd", ContactPerson = "Anna Svensson", Industry = "Software" },
                new Company { CompanyName = "Fabrikam AB", ContactPerson = "Erik Karlsson", Industry = "Manufacturing" }
            };

            await companies.InsertManyAsync(seedCompanies);
        }

        if(appsCount == 0) {
            // Try to attach the first company if present
            var firstCompany = await companies.Find(FilterDefinition<Company>.Empty).FirstOrDefaultAsync();
            var seedApps = new List<JobApplication>
            {
                new JobApplication
                {
                    Title = "Junior Developer",
                    CompanyId = firstCompany?.Id,
                    CompanyName = firstCompany?.CompanyName ?? string.Empty,
                    AppliedAt = DateTime.UtcNow,
                    Status = "Pending"
                }
            };

            await jobApps.InsertManyAsync(seedApps);
        }
    }
}
