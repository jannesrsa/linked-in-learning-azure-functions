using System;
using System.Collections.Generic;
using System.Text;

namespace LinkedInLearning.Azure.Functions.Models
{
    public record PhotoUloadModel(string Name, string Description, string[] Tags, string Photo);
}
