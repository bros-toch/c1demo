using System;
using Composite.Functions;

public partial class C1Function : Composite.AspNet.UserControlFunction
{
    public override string FunctionDescription
    {
        get 
        { 
            return "A demo function that outputs a hello message."; 
        }
    }

    [FunctionParameter(DefaultValue = "World")]
    public string Name { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
}