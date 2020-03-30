using ParserawkaWPF.Model;

namespace ParserawkaWPF.Interfaces
{
    public interface IStatementList
    {
        int AddStatement(Statement statement);
        Statement GetStatementByIndex(int index);
        Statement GetStatementByProgramLine(int programLine);
        int GetIndex(Statement statement);
        int GetIndexByProgramLine(int programLine);
        int GetSize();
    }
}
