using System;
using System.Collections.Generic;
using System.IO;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RobotCleaner.Exceptions;
using RobotCleaner.Interfaces;
using RobotCleaner.Models;
using RobotCleaner.Validators;
using Serilog;

namespace RobotCleaner
{
    internal class Program
    {
        public static int Main(string[] args)
        {
            IServiceProvider serviceProvider = Startup.CreateServiceProvider();
            var logger = serviceProvider.GetRequiredService<ILogger>();
            InputArguments inputArguments = ParseInputArguments(args, logger);
            if (inputArguments == null)
            {
                return ExitSequence(logger);
            }

            IInitializedRobot robot = null;
            try
            {
                new InputArgumentsValidator().ValidateAndThrow(inputArguments);

                string fileContent = File.ReadAllText(inputArguments.RobotInstructionsFileLocation);
                var robotInstruction = JsonConvert.DeserializeObject<RobotInstruction>(fileContent);
                robot = serviceProvider.GetRequiredService<IRobot>().InitializeRobot(robotInstruction);

                foreach (RobotCommand robotInstructionCommand in robotInstruction!.Commands)
                {
                    logger.Information($"Executing command: {robotInstructionCommand}");
                    robot.ExecuteCommand(robotInstructionCommand);
                }

                logger.Information("All commands executed");
            }
            catch (ValidationException validationException)
            {
                logger.Error($"Validation errors: {string.Join(',', validationException.Errors)}");
            }
            catch (RobotCleanerException ex)
            {
                logger.Error(ex.Message);
            }
            catch (Exception ex)
            {
                logger.Error($"Error: {ex.Message}");
            }
            finally
            {
                if (robot != null)
                {
                    string robotResult = JsonConvert.SerializeObject(robot.CreateResult(),
                        new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
                    logger.Information($"Result: {robotResult}");
                    File.WriteAllText(inputArguments.RobotResultFileLocation, robotResult);
                }
            }

            return ExitSequence(logger);
        }

        private static int ExitSequence(ILogger logger)
        {
            logger.Information("Press any key to close window");
            Console.ReadKey();
            return 0;
        }

        private static InputArguments ParseInputArguments(IReadOnlyList<string> args, ILogger logger)
        {
            const int requiredNumberOfArguments = 2;
            if (args.Count != requiredNumberOfArguments)
            {
                logger.Error($"Exactly {requiredNumberOfArguments} arguments are needed");
                return null;
            }

            return new InputArguments()
                { RobotInstructionsFileLocation = args[0], RobotResultFileLocation = args[1] };
        }
    }
}