using Autofac;
using Autofac.Core;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Sequence_Pro.Application.Database;
using Sequence_Pro.Application.Interfaces;
using Sequence_Pro.Application.Repositories;
using Sequence_Pro.Application.Services;
using Sequence_Pro.Application.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sequence_Pro.Application.IoC;
public class SequenceAnalysisModule : Module
{
    private readonly string _dbConnectionString;

    public SequenceAnalysisModule(string dbConnectionString)
    {
        _dbConnectionString = dbConnectionString;
    }
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<UniprotAPI>().As<IUniprotAPI>();
        builder.RegisterType<SequenceAnalyser>().As<ISequenceAnalyser>();
        builder.RegisterType<RequestValidator>().As<IValidator<string>>();
        builder.RegisterType<SequenceAnalysisRepository>().As<ISequenceAnalysisRepository>();
        builder.RegisterType<SequenceAnalysisService>().As<ISequenceAnalysisService>();

        builder.Register(x =>
        {
            var optionsBuilder = new DbContextOptionsBuilder<SequenceProContext>();
            optionsBuilder.UseNpgsql(_dbConnectionString);
            return new SequenceProContext(optionsBuilder.Options);
        }).InstancePerLifetimeScope();
    }
}
