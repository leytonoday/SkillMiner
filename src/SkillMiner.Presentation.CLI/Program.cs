// See https://aka.ms/new-console-template for more information

using SkillMiner.Infrastructure.Persistence;
using SkillMiner.Shared;

Console.WriteLine("Hello, World!");

var temp = Utils.IsDevelopment();

Console.WriteLine(temp);
