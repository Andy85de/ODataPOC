using ODataPOC;

namespace AntlTests;

public class Tests
{
    [Fact]
    public void Test1()
    {
        var treeBuilder = new TreeBuilder();
        string filter = "$orderby=name";
        var parseTree = treeBuilder.Build(filter);
    }
}
