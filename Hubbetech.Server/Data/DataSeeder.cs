using Hubbetech.Server.Models;
using Hubbetech.Shared.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Hubbetech.Server.Data
{
    public static class DataSeeder
    {
        public static async Task SeedDataAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await context.Database.EnsureCreatedAsync();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("DataSeeder");

            // Seed Roles
            if (!await roleManager.RoleExistsAsync(Roles.Gestor))
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Gestor));
            }

            if (!await roleManager.RoleExistsAsync(Roles.Funcionario))
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Funcionario));
            }

            // Seed Admin User
            var adminEmail = configuration["AdminSettings:Email"];
            var adminPassword = configuration["AdminSettings:Password"];
            ApplicationUser? adminUser = null;

            if (string.IsNullOrEmpty(adminEmail) || string.IsNullOrEmpty(adminPassword))
            {
                logger.LogWarning("Admin settings not found in configuration. Skipping admin user seeding.");
            }
            else
            {
                adminUser = await userManager.FindByEmailAsync(adminEmail);
                if (adminUser == null)
                {
                    adminUser = new ApplicationUser
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(adminUser, adminPassword);
                    if (result.Succeeded)
                    {
                        logger.LogInformation($"Admin user '{adminEmail}' created successfully.");
                        await userManager.AddToRoleAsync(adminUser, Roles.Gestor);
                        logger.LogInformation($"Role '{Roles.Gestor}' assigned to admin user.");
                    }
                    else
                    {
                        logger.LogError($"Failed to create admin user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                        adminUser = null; // Reset if failed
                    }
                }
                else
                {
                    logger.LogInformation($"Admin user '{adminEmail}' already exists.");
                    if (!await userManager.IsInRoleAsync(adminUser, Roles.Gestor))
                    {
                        await userManager.AddToRoleAsync(adminUser, Roles.Gestor);
                        logger.LogInformation($"Role '{Roles.Gestor}' assigned to existing admin user.");
                    }
                }
            }

            // Seed Application Data
            // Context already retrieved above

            // Seed Demandas
            if (!context.Demandas.Any())
            {
                context.Demandas.AddRange(
                    new Demanda
                    {
                        Title = "Implementar Autenticação JWT",
                        Description = "Configurar autenticação segura com tokens JWT para a API.",
                        Status = Hubbetech.Shared.Enums.StatusDemanda.Feito,
                        AssignedToUserId = adminUser?.Id
                    },
                    new Demanda
                    {
                        Title = "Criar Dashboard",
                        Description = "Desenvolver o dashboard principal com gráficos e estatísticas.",
                        Status = Hubbetech.Shared.Enums.StatusDemanda.EmRevisao,
                        AssignedToUserId = adminUser.Id
                    },
                    new Demanda
                    {
                        Title = "Refatorar CSS do Menu",
                        Description = "Ajustar cores e espaçamento do menu lateral para mobile.",
                        Status = Hubbetech.Shared.Enums.StatusDemanda.Fazendo,
                        AssignedToUserId = adminUser.Id
                    },
                    new Demanda
                    {
                        Title = "Testar Fluxo de Cadastro",
                        Description = "Verificar se novos usuários conseguem se cadastrar corretamente.",
                        Status = Hubbetech.Shared.Enums.StatusDemanda.AFazer,
                        AssignedToUserId = adminUser.Id
                    }
                );
                await context.SaveChangesAsync();
            }

            // Seed Equipamentos
            if (!context.Equipamentos.Any())
            {
                context.Equipamentos.AddRange(
                    new Equipamento { Name = "MacBook Pro 16", SerialNumber = "MBP-2023-001", Status = "Em Uso" },
                    new Equipamento { Name = "Dell XPS 15", SerialNumber = "DELL-XPS-992", Status = "Disponível" },
                    new Equipamento { Name = "Monitor LG Ultrawide", SerialNumber = "LG-34-WK", Status = "Em Uso" },
                    new Equipamento { Name = "Teclado Mecânico Keychron", SerialNumber = "KEY-K2-V2", Status = "Manutenção" }
                );
                await context.SaveChangesAsync();
            }

            // Seed LinksEmpresa
            if (!context.LinksEmpresa.Any())
            {
                context.LinksEmpresa.AddRange(
                    new LinkEmpresa { Title = "Drive Compartilhado", Url = "https://drive.google.com" },
                    new LinkEmpresa { Title = "Portal do Colaborador", Url = "https://portal.hubbetech.com" },
                    new LinkEmpresa { Title = "GitHub Organization", Url = "https://github.com/hubbetech" },
                    new LinkEmpresa { Title = "Figma Design System", Url = "https://figma.com" }
                );
                await context.SaveChangesAsync();
            }
        }
    }
}
