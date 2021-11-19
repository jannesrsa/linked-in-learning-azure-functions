namespace LinkedInLearning.Azure.Functions.Models;

public record PhotoUploadModel(string Name, string Description, string[] Tags, string Photo);