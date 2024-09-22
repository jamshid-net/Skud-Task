using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.CustomExceptions;

public class ModelIsNullException(string modelName, string comment = "") : Exception($"{modelName} is null!" + comment);

